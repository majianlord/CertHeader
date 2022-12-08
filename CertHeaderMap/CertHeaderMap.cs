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
            //Log.LoggerSetup(NLog.LogLevel.Info, false);
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            HttpContext current = HttpContext.Current;

            HttpClientCertificate theHttpCertificate = HttpContext.Current.Request.ClientCertificate;
            string strTicket = "";
            Boolean Tickchecked = false;

            //Grab OTDS Ticket
            HttpCookie Ticket = HttpContext.Current.Request.Cookies["OTDSTicket"];
            if (Ticket != null)
            {
                 strTicket = Ticket.Value;
            }

            //Grab Processing Flag
            HttpCookie TickCheckCookie = HttpContext.Current.Request.Cookies["Tickchecked"];
            if (TickCheckCookie != null)
            {
                Tickchecked = bool.Parse(TickCheckCookie.Value);
            }
           
            //If we have a Certificate
            if (theHttpCertificate != null)
            {
                string EDIPI = theHttpCertificate.Subject.ToString().Right(10);
                Log.Instance.Info(ModuleName + " : " + theHttpCertificate.Subject.ToString().Right(10));
                if (EDIPI != null)
                { 
                    current.Request.Headers.Add("EDIPI", EDIPI);
                }
                //If we already have an OTDS Check lets check if that user has been assigned an CAC ID
                if (!string.IsNullOrEmpty(strTicket))
                {
                    //Setup the Proxy
                    Proxy.OTDS oTDS = new Proxy.OTDS();
                    //Pull the user for this OTDS Ticket
                    CurrentUserResponse User =  oTDS.CurrentUser(strTicket);
                    // We got a User back lets check for the EDIPI info
                    if (User != null)
                    {
                        OtdsUser Userinf = new OtdsUser(User.user);
                        if (string.IsNullOrEmpty(Userinf.oTExtraAttr0))
                        {

                        }
                    }
                }
            }
            else
            {
                Log.Instance.Info(ModuleName + " : No CERT Persented");
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            //HttpApplication application = (HttpApplication)source;
            //HttpContext context = application.Context;
            //string filePath = context.Request.FilePath;
            //string fileExtension =
            //    VirtualPathUtility.GetExtension(filePath);
            //if (fileExtension.Equals(".aspx"))
            //{
            //    context.Response.Write("<hr><h1><font color=red>" +
            //        "HelloWorldModule: End of Request</font></h1>");
            //}
        }



        public void Dispose() { }
    }

}