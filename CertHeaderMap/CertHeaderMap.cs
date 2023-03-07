using System;
using System.Collections.Specialized;
using System.Web;
using CertHeader.Logging;
using NLog;
using OTDS.RR;

namespace CertHeader
{
    public class CertHeaderMap : IHttpModule
    {
        string strTicket = "";
        bool Tickchecked = false;
        string EDIPI;


        public CertHeaderMap()
        {
        }

        public String ModuleName
        {
            get { return "CertHeaderMap"; }
        }

        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            Log.LoggerSetup(NLog.LogLevel.Info, false);
            application.BeginRequest +=
                (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
                (new EventHandler(this.Application_EndRequest));
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            HttpContext current = HttpContext.Current;
            HttpClientCertificate theHttpCertificate = HttpContext.Current.Request.ClientCertificate;


            //Grab OTDS Ticket if one exists
            HttpCookie Ticket = HttpContext.Current.Request.Cookies["OTDSTicket"];
            if (Ticket != null)
            {
                strTicket = Ticket.Value;
            }

            //Grab Processing Flag if one exists
            HttpCookie TickCheckCookie = HttpContext.Current.Request.Cookies["Tickchecked"];
            if (TickCheckCookie != null)
            {
                Tickchecked = bool.Parse(TickCheckCookie.Value);
            }



            //If we have a Certificate grab the EDIPI and Add the Custom header for processing by OTDS
            if (theHttpCertificate != null)
            {
                EDIPI = theHttpCertificate.Subject.ToString().Right(10);
                Log.Instance.Info(ModuleName + " : " + theHttpCertificate.Subject.ToString().Right(10));
                if (EDIPI != null)
                {
                    current.Request.Headers.Add("EDIPI", EDIPI);
                }
            }
            else
            {
                Log.Instance.Info(ModuleName + " : No CERT Persented");
            }

            //If we have already done the OTDS Checks on this Users we can go ahead and Quite and let the system move on.
            if (Tickchecked == true)
            {
                return;
            }





            //If we already have an OTDS Check lets check if that user has been assigned an CAC ID
            if (!string.IsNullOrEmpty(strTicket))
            {
                //Setup the Proxy
                Proxy.OTDS oTDS = new Proxy.OTDS();
                //Pull the user for this OTDS Ticket
                CurrentUserResponse User = oTDS.CurrentUser(strTicket);
                
                // We got a User back lets check for the EDIPI info
                if (User != null)
                {
                    //Grab the users with the easy to find values pulled out.
                    OtdsUser Userinf = new OtdsUser(User.user);
                    

                    //So we have a User with no EDIPI saved for the users
                    if (string.IsNullOrEmpty(Userinf.oTExtraAttr0))
                    {
                        //Here we validate we have an EDIPI to apply to the user
                        if (EDIPI != null)
                        {
                            //here we apply the EDIPI and save the user back to OTDS.
                            GenericValue tempval = new GenericValue();
                            tempval.name = "oTExtraAttr0";
                            tempval.values = new System.Collections.Generic.List<object>();
                            tempval.values.Add(EDIPI);
                            Userinf.WorkingUser.values.Add(tempval);
                            SetValuesonOTDSUser(Userinf.WorkingUser, oTDS);
                            current.Request.Headers.Add("EDIPI", EDIPI);
                        }
                    }
                }


                //We did out checks and updates time to flag this sessions for no more processing
                HttpContext.Current.Response.Cookies["OTDSTicket"].Value = "true";
            }
            else
            {
                //The user is not logged in yet so we cant do any auto mapping.
            }

            
        }



        private bool IsDisabled(User ArgOtdsUser)
        {
            bool retValue = false;
            try
            {
                string chkAttrib = ArgOtdsUser.values.Find(x => x.name.ToLower() == "accountdisabled").values[0].ToString();
                if (chkAttrib != null)
                {
                    if (chkAttrib.ToLower() == "true")
                        retValue = true;
                    else
                        retValue = false;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "IsDisabled Error: " + ex.Message);
            }
            return retValue;
        }

        public int  SetValuesonOTDSUser(OTDS.RR.User argUser, Proxy.OTDS otdsProxy )
        {
            OTDS.RR.UsersPostResponse retVal;
            OTDS.RR.UsersPostRequest setVal = new OTDS.RR.UsersPostRequest();

            try
            {
                setVal.userPartitionID = argUser.userPartitionID;
                setVal.name = argUser.name;
                setVal.location = argUser.location;
                setVal.id = argUser.id;
                setVal.description = argUser.description;
                setVal.urlId = argUser.urlId;
                setVal.urlLocation = argUser.urlLocation;
                setVal.objectClass = argUser.objectClass;
                setVal.values = argUser.values; // As List(Of GenericValue)
                setVal.customAttributes = argUser.customAttributes; // As List(Of CustomAttribute)
                retVal = otdsProxy.PutUsers(setVal);
                if (retVal.Error1.Length > 1)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "SetOtUser Error:");
                return -1;
            }
        }



        private void Application_EndRequest(Object source, EventArgs e)
    {
    }
    public void Dispose() { }
}




}