using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace OTDS.RR
{

    /// <summary>
    ///     ''' Response to Authentication/Resource/http Rest API Call
    ///     ''' </summary>
    public class AuthenticationResourceHttpResponse
    {
        /// <exclude />
        public string Token { get; set; }
        /// <exclude />
        public string UserId { get; set; }
        /// <exclude />
        public string Ticket { get; set; }
        /// <exclude />
        public string ResourceID { get; set; }
        /// <exclude />
        public string FailureReason { get; set; }
        /// <exclude />
        public Int64 PasswordExpirationTime { get; set; }
        /// <exclude />
        public bool Continuation { get; set; }
        /// <exclude />
        public string[] ContinuationContext { get; set; }
        /// <exclude />
        public string[] ContinuationData { get; set; }
        /// <exclude />
        public int Status { get; set; }
        /// <exclude />
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }
    public class AuthenticationResourceHttpBody
    {
        /// <exclude />
        public List<GenericValue> headersList { get; set; }
        /// <exclude />
        public List<GenericValue> cookiesList { get; set; }
        /// <exclude />
        public byte[] secureSecret { get; set; }
        /// <exclude />
        public string sourceResourceId { get; set; }
        public byte[] authenticator { get; set; }
    }
    /// <summary>
    ///     ''' Handles /Authentication/Headers Response.
    ///     ''' </summary>
    public class AuthenticationHeadersResponse
    {
        /// <exclude />
        public string Token { get; set; }
        /// <exclude />
        public string UserId { get; set; }
        /// <exclude />
        public string Ticket { get; set; }
        /// <exclude />
        public string ResourceID { get; set; }
        /// <exclude />
        public string FailureReason { get; set; }
        /// <exclude />
        public Int64 PasswordExpirationTime { get; set; }
        /// <exclude />
        public bool Continuation { get; set; }
        /// <exclude />
        public string[] ContinuationContext { get; set; }
        /// <exclude />
        public string[] ContinuationData { get; set; }
        /// <exclude />
        public int Status { get; set; }
        /// <exclude />
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }


    public class AuthenticationTicketforResourceResponse
    {
        public string token { get; set; }
        public string userId { get; set; }
        public string ticket { get; set; }
        public string resourceID { get; set; }
        public string failureReason { get; set; }
        public Int64 passwordExpirationTime { get; set; }
        public bool continuation { get; set; }
        public string[] continuationContext { get; set; }
        public string[] continuationData { get; set; }
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }

    public class AuthenticationTicketforResourceHttpBody
    {
        public string ticket { get; set; }
        public string sourceResourceId { get; set; }
        public string targetResourceId { get; set; }
        public List<GenericValue> clientData { get; set; }
        public string userName { get; set; }
        public string[] authenticator { get; set; }
        public string[] secureSecret { get; set; }
    }


    // Public Class CurrentUserResponseUser
    // Public Property userPartitionID As String
    // Public Property name() As String
    // Public Property location() As String
    // Public Property id() As String
    // Public Property description() As Object
    // Public Property values() As List(Of GenericValue)
    // Public Property objectClass() As String
    // Public Property urlId As String
    // Public Property urlLocation As String
    // Public Property customAttributes As List(Of CustomAttribute)
    // End Class
    public class CurrentUserResponse
    {
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public bool isAdmin { get; set; }
        public bool isSysAdmin { get; set; }
        public User user { get; set; }
    }


    public class Session
    {
        public DateTime currentTime { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime renewedAt { get; set; }
        public DateTime idleExpiresAt { get; set; }
        public DateTime absoluteExpiresAt { get; set; }
        public string ipAddress { get; set; }
        public string userAgent { get; set; }
        public string sessionId { get; set; }
    }

    public class CurrentUserSessionsResponse
    {
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
    }

    public class GenericValue
    {
        public string name { get; set; }
        public List<object> values { get; set; }
    }

    public class CustomAttribute
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }


    public class GenericResponse
    {
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }


    public class GenericStringList
    {
        public List<string> stringList { get; set; }
    }

    public class GeneralReply
    {
        /// <summary>
        ///         ''' Gets or sets the status.
        ///         ''' </summary>
        ///         ''' <value>200 Is Success,  204 is Success (No Reply Needed), 400 Codes are Failures.</value>
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public ErrorDetails errorDetails { get; set; }
    }

    public class ErrorDetails
    {
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }
    }








    public class usersmemberofResponse
    {
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public List<Group> groups { get; set; }
        public string nextPageCookie { get; set; }
        public Int32 requestedPageSize { get; set; }
        public string requestedFilter { get; set; }
        public Int32 actualPageSize { get; set; }
    }

    public class UserIsMemberOfResponse
    {
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public int returnValue { get; set; }
    }

    /// <summary>
    ///     ''' Class Used in the Creation of a new User VIA Post to /Users
    ///     ''' </summary>
    public class UsersPostResponse
    {
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
        public string userPartitionID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string urlId { get; set; }
        public string urlLocation { get; set; }
        public string objectClass { get; set; }
        public List<GenericValue> values { get; set; } = new List<GenericValue>();
        public List<CustomAttribute> customAttributes { get; set; } = new List<CustomAttribute>();
    }

    public class UsersPostRequest
    {
        public string userPartitionID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string urlId { get; set; }
        public string urlLocation { get; set; }
        public string objectClass { get; set; }
        public List<GenericValue> values { get; set; }
        public List<CustomAttribute> customAttributes { get; set; }
    }



    public class User
    {
        public string userPartitionID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public List<CustomAttribute> customAttributes { get; set; }
        public string objectClass { get; set; }
        public List<GenericValue> values { get; set; }
        public string urlId { get; set; }
        public string urlLocation { get; set; }
    }

    public class PatchUser
    {
        public string id { get; set; }
        public List<GenericValue> values { get; set; }
    }


    public class GetUsersResponse
    {
        public List<User> users { get; set; }
        public string requestedFilter { get; set; }
        public Int32 requestedPageSize { get; set; }
        public Int32 actualPageSize { get; set; }
        public string nextPageCookie { get; set; }
        // Public Property nextPageCookie As List(Of String)
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }

    public class Group
    {
        public int numMembers { get; set; }
        public string userPartitionID { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
        public List<GenericValue> values { get; set; }
        public string description { get; set; }
        public string objectClass { get; set; }
        public List<CustomAttribute> customAttributes { get; set; }
        public string urlId { get; set; }
        public string urlLocation { get; set; }
        public string originUUID { get; set; }
        public string uuid { get; set; }
    }

    public class GetGroupsResponse
    {
        public List<Group> groups { get; set; }
        public string requestedFilter { get; set; }
        public Int32 requestedPageSize { get; set; }
        public Int32 actualPageSize { get; set; }
        public string nextPageCookie { get; set; }
        // Public Property nextPageCookie As List(Of String)
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }
    public class GetGroupsGroupMemeberofResponse
    {
        public List<Group> groups { get; set; }
        public string requestedFilter { get; set; }
        public Int32 requestedPageSize { get; set; }
        public Int32 actualPageSize { get; set; }
        public List<string> nextPageCookie { get; set; }
        public string Status { get; set; }
        [JsonPropertyName("error")]
        public string Error1 { get; set; }
    }
}



public class OtdsUser
{
    public string oTExtraAttr0 { get; set; }
    public string oTExtraAttr1 { get; set; }
    public string oTExternalID3 { get; set; }

    public OTDS.RR.User WorkingUser { get; set; }

    public OtdsUser(OTDS.RR.User argUser)
    {
        WorkingUser = argUser;
        OTDS.RR.GenericValue tmpVal;

        tmpVal = argUser.values.FirstOrDefault(y => y.name.ToLower() == "otexternalid3");
        if (string.IsNullOrEmpty((string)tmpVal.values[0]))
            oTExternalID3 = "";
        else
            oTExternalID3 = tmpVal.values[0].ToString();
        tmpVal = argUser.values.FirstOrDefault(y => y.name.ToLower() == "otextraattr0");
        if (string.IsNullOrEmpty((string)tmpVal.values[0]))
            oTExtraAttr0 = "";
        else
            oTExtraAttr0 = tmpVal.values[0].ToString();
        tmpVal = argUser.values.FirstOrDefault(y => y.name.ToLower() == "otextraattr1");
        if (string.IsNullOrEmpty((string)tmpVal.values[0]))
            oTExtraAttr1 = "";
        else
            oTExtraAttr1 = tmpVal.values[0].ToString();
    }
}

