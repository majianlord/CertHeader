using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using RestSharp;
using System.Web;
using OTDS.RR;

namespace Proxy
{
    public class OTDS
    {
        public RestResponse LastResponse;
        public RestRequest LastRequest;
        public string CurrentError = "";
        public Int64 LastStatus = 0;
        private string STRBaseUrl;
        private string STRMaster_Account;
        private string STRMasterOTDSTicket;
        private string STRMasterOTDSToken;
        private DateTime STRMasterOTDSTicketExpiration = DateTime.Now;
        private string STRUserOTDSTicket;
        private string authPassword;


        public OTDS()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.DefaultConnectionLimit = 25;
                        STRBaseUrl = "http://cshdevcs.centralus.cloudapp.azure.com:8002/";
                        STRMaster_Account = "otadmin@otds.admin";
        }


        /// <summary>
        ///         ''' Base Function for Connecting to and using the OTDS Authenticaion System
        ///         ''' </summary>
        ///         ''' <typeparam name="T">Custom Result type</typeparam>
        ///         ''' <param name="request">Request to be Sent</param>
        ///         ''' <returns></returns>
        public T Execute<T>(RestRequest request, bool needAuth = true) where T : new()
        {
            var Varmystuff = ExecuteAsync<T>(request, needAuth);
            return Varmystuff.Result;
        }

        public Task<T> ExecuteAsync<T>(RestRequest Request, bool NeedAuth = true) where T : new()
        {
            LastStatus = (long)System.Net.HttpStatusCode.BadRequest;
            RestClient client = new RestClient(STRBaseUrl);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            if (NeedAuth == true)
            {
                SetupMasterOtds();
                client.AddDefaultParameter("OTDSTicket", STRMasterOTDSTicket, ParameterType.HttpHeader);
            }

            // used on every request
            Task<RestResponse<T>> response1 = null;
            response1 = client.ExecuteAsync<T>(Request);
            response1.Wait();
            var response = response1.Result;

            LastStatus = (long)response.StatusCode;


            if (response.ErrorException != null)
            {
                switch (LastStatus)
                {
                    case 401:
                        {
                            break;
                        }

                    case 3007:
                        {
                            break;
                        }

                    case (long)System.Net.HttpStatusCode.NoContent:
                        {
                            break;
                        }

                    case 404:
                        {
                            break;
                        }

                    default:
                        {
                            string message = "Error retrieving response.  Check inner details for more info. ";
                            try
                            {
                                var prop = response.GetType().GetProperty("Content");
                                message += " (" + prop.GetValue(response) + ")";
                            }
                            catch (Exception ex)
                            {
                            }
                            ApplicationException otdsProxyException = new ApplicationException(message, response.ErrorException);
                            throw otdsProxyException;
                            break;
                        }
                }
            }
            return Task.FromResult(response.Data);
        }

        // Function CheckForErrors(Response As RestResponse) As Object
        // LastResponse = Response
        // Return Response
        // End Function





        public AuthenticationHeadersResponse AuthenticationUNPW(string UserName, string Password)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.Resource = "otdsws/rest/authentication/credentials";
            request.AddHeader("Content-Type", "application/json");
            request.Method = Method.Post;
            var js = @"{{""userName"": ""{UserName}"",""password"": ""{Password}"",""ticketType"": ""OTDSTICKET""}}";
            request.AddStringBody(js, DataFormat.Json);
            return Execute<AuthenticationHeadersResponse>(request, false);
        }




        public AuthenticationHeadersResponse AuthenticationHeaders(string userID)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.Resource = "otdsws/rest/authentication/headers";
            request.AddHeader("EDIPI", userID);
            return Execute<AuthenticationHeadersResponse>(request, false);
        }





        public CurrentUserResponse CurrentUser(string OTDSTICKET)
        {
            RestRequest request = new RestRequest();
            request.Resource = "otdsws/rest/currentuser";
            request.AddHeader("OTDSTicket", OTDSTICKET);
            return Execute<CurrentUserResponse>(request, false);
        }

        /// <summary>
        ///         ''' This call is the POST call to Users otdsws/rest/users and is used to create a new user in OTDS.
        ///         ''' </summary>
        ///         ''' <param name="UserInfo">All User Info should be Provided it this Class, For a List Of Avliable Options see help documentation for Class</param>
        ///         ''' <returns></returns>
        public UsersPostResponse Users(UsersPostRequest UserInfo)
        {
            RestRequest request = new RestRequest()
            {
                Method = Method.Post,
                Resource = "otdsws/rest/users"
            };
            request.AddJsonBody(UserInfo);
            LastRequest = request;
            return Execute<UsersPostResponse>(request, true);
        }
       

        /// <summary>
        ///         ''' Get Call to OTDS to Get a Specific User
        ///         ''' </summary>
        ///         ''' <param name="Userid">EDIPI Number of the user your looking to get</param>
        ///         ''' <returns>Returns a User Object Or Error Code,  Error Code 3007 USER NOT FOUND</returns>
        public UsersPostResponse GetUsers(string Userid)
        {
            RestRequest request = new RestRequest();
            request.Resource = "otdsws/rest/users/{user_id}";
            request.AddParameter("user_id", Userid, ParameterType.UrlSegment);
            UsersPostResponse Obj = Execute<UsersPostResponse>(request, true);
            if (Obj == null)
                return new UsersPostResponse();
            else
                return Obj;
        }



        public UsersPostResponse PatchUsers(PatchUser UserInfo)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Patch;
            request.Resource = "otdsws/rest/users/{user_id}";
            request.AddParameter("user_id", UserInfo.id, ParameterType.UrlSegment);
            request.AddJsonBody(UserInfo);
            return Execute<UsersPostResponse>(request, true);
        }

        public UsersPostResponse PutUsers(UsersPostRequest UserInfo)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Put;
            request.Resource = "otdsws/rest/users/{user_id}";
            request.AddParameter("user_id", UserInfo.id, ParameterType.UrlSegment);
            request.AddJsonBody(UserInfo);
            return Execute<UsersPostResponse>(request, true);
        }

        public GeneralReply TwoFactorStatusSet(Twofactorstate TwoFactor, string UserID)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Put;
            request.Resource = "otdsws/rest/users/{user_id}/twofactorstate";
            request.AddParameter("user_id", UserID, ParameterType.UrlSegment);
            request.AddJsonBody(TwoFactor);
            return Execute<GeneralReply>(request, true);
        }


        public Twofactorstate TwoFactorStatusGet(string UserID)
        {
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.Resource = "otdsws/rest/users/{user_id}/twofactorstate";
            request.AddParameter("user_id", UserID, ParameterType.UrlSegment);
            return Execute<Twofactorstate>(request, true);
        }










        private void SetupMasterOtds()
        {
            if (string.IsNullOrEmpty(STRMasterOTDSTicket))
            {
                var result = AuthenticationHeaders(STRMaster_Account);
                STRMasterOTDSTicket = result.Ticket;
                STRMasterOTDSToken = result.Token;
                STRMasterOTDSTicketExpiration = DateTime.Now.AddMinutes(10);
            }
            else
            {
                DateTime Timecheck = DateTime.Now.AddMinutes(-2);
                if (STRMasterOTDSTicketExpiration >= Timecheck)
                {
                    var result = AuthenticationHeaders(STRMaster_Account);
                    STRMasterOTDSTicket = result.Ticket;
                    STRMasterOTDSToken = result.Token;
                    STRMasterOTDSTicketExpiration = DateTime.Now.AddMinutes(10);
                }
            }
        }
    }
}
