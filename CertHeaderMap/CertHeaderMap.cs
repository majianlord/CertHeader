using System;
using System.Collections.Specialized;
using System.Web;
using CertHeader.Logging;
using NLog;


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

           
            if (theHttpCertificate != null)
            {
                string EDIPI = theHttpCertificate.Subject.ToString().Right(10);
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