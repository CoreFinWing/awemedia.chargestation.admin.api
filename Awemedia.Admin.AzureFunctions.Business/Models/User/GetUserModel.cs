using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Awemedia.Admin.AzureFunctions.Business.Models
{

    public class GetUserResponse
    {
        public string odatametadata { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string odatatype { get; set; }
        public string objectType { get; set; }
        public string objectId { get; set; }
        public object deletionTimestamp { get; set; }
        public bool accountEnabled { get; set; }
        public object ageGroup { get; set; }
        public object[] assignedLicenses { get; set; }
        public object[] assignedPlans { get; set; }
        public string city { get; set; }
        public object companyName { get; set; }
        public object consentProvidedForMinor { get; set; }
        public string country { get; set; }
        public DateTime createdDateTime { get; set; }
        public string creationType { get; set; }
        public object department { get; set; }
        public object dirSyncEnabled { get; set; }
        public string displayName { get; set; }
        public object employeeId { get; set; }
        public object facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public object immutableId { get; set; }
        public object isCompromised { get; set; }
        public object jobTitle { get; set; }
        public object lastDirSyncTime { get; set; }
        public object legalAgeGroupClassification { get; set; }
        public string mail { get; set; }
        public string mailNickname { get; set; }
        public string mobile { get; set; }
        public object onPremisesDistinguishedName { get; set; }
        public object onPremisesSecurityIdentifier { get; set; }
        public string[] otherMails { get; set; }
        public string passwordPolicies { get; set; }
        public object passwordProfile { get; set; }
        public object physicalDeliveryOfficeName { get; set; }
        public string postalCode { get; set; }
        public object preferredLanguage { get; set; }
        public object[] provisionedPlans { get; set; }
        public object[] provisioningErrors { get; set; }
        public string[] proxyAddresses { get; set; }
        public DateTime refreshTokensValidFromDateTime { get; set; }
        public bool? showInAddressList { get; set; }
        public Signinname[] signInNames { get; set; }
        public object sipProxyAddress { get; set; }
        public string state { get; set; }
        public string streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }
        public string thumbnailPhotoodatamediaEditLink { get; set; }
        public object usageLocation { get; set; }
        public object[] userIdentities { get; set; }
        public string userPrincipalName { get; set; }
        public string userState { get; set; }
        public DateTime? userStateChangedOn { get; set; }
        public string userType { get; set; }

        public string Email
        {
            get
            {
                if (this.signInNames.Length > 0)
                    return this.signInNames[0].value;
                else
                    return "";
            }
        }
    }

    public class Signinname
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
