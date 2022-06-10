using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{

    public class UserModel
    {
        public string Email { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public string password { get; set; }
        public string city { get; set; }
        public object country { get; set; }
        public string givenName { get; set; }
        public object mail { get; set; }
        public object mobile { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public string companyName { get; set; }
        public string role { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class UserProfile
    {
        public bool accountEnabled { get; set; }
        public List<SignInName> signInNames { get; set; }
        public string creationType { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public Passwordprofile passwordProfile { get; set; }
        public string passwordPolicies { get; set; }
        public string city { get; set; }
        public object country { get; set; }
        public object facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public object mail { get; set; }
        public object mobile { get; set; }
        public List<string> otherMails { get; set; }
        public string postalCode { get; set; }
        public object preferredLanguage { get; set; }
        public string state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }

        [JsonProperty("extension_171b808256604925afa4cc036b28c814_UserRoles")]
        private string _UserRoles { get; set; }
        public string UserRoles
        {
            get
            {
                return _UserRoles;
            }
            set
            {
                _UserRoles = value;
            }
        }

    }

    public class Passwordprofile
    {
        public string password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
    }

    public class SignInName
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
