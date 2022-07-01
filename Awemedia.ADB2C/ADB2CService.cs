using Awemedia.ADB2C.Models;
using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Awemedia.ADB2C
{
    public class ADB2CService
    {
        static GraphServiceClient graphClient;
        static ADB2CService()
        {
            // Initialize the client credential auth provider
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(Environment.GetEnvironmentVariable("b2c-Tenant"),Environment.GetEnvironmentVariable("b2c-ClientId"),Environment.GetEnvironmentVariable("b2c-ClientSecret"));
            graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        }
        public static async Task<List<User>> ListUsers(GraphServiceClient graphClient)
        {
            List<User> usersList = new List<User>();
            try
            {
               
                // Get all users
                var users = await graphClient.Users
                    .Request()
                    .GetAsync();

                // Iterate over all the users in the directory
                var pageIterator = PageIterator<User>
                    .CreatePageIterator(
                        graphClient,
                        users,
                        // Callback executed for each user in the collection
                        (user) =>
                        {
                            usersList.Add(user);
                            return true;
                        },
                        // Used to configure subsequent page requests
                        (req) =>
                        {
                            Console.WriteLine($"Reading next page of users...");
                            return req;
                        }
                    );

                await pageIterator.IterateAsync();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            return usersList;
        }
       
        public static async Task ListUsersWithCustomAttribute(GraphServiceClient graphClient, string b2cExtensionAppClientId)
        {
            if (string.IsNullOrWhiteSpace(b2cExtensionAppClientId))
            {
                throw new ArgumentException("B2cExtensionAppClientId (its Application ID) is missing from appsettings.json. Find it in the App registrations pane in the Azure portal. The app registration has the name 'b2c-extensions-app. Do not modify. Used by AADB2C for storing user data.'.", nameof(b2cExtensionAppClientId));
            }

            // Declare the names of the custom attributes
            const string customAttributeName1 = "FavouriteSeason";
            const string customAttributeName2 = "LovesPets";

            // Get the complete name of the custom attribute (Azure AD extension)
            Helpers.B2cCustomAttributeHelper helper = new Helpers.B2cCustomAttributeHelper(b2cExtensionAppClientId);
            string favouriteSeasonAttributeName = helper.GetCompleteAttributeName(customAttributeName1);
            string lovesPetsAttributeName = helper.GetCompleteAttributeName(customAttributeName2);

            Console.WriteLine($"Getting list of users with the custom attributes '{customAttributeName1}' (string) and '{customAttributeName2}' (boolean)");
            Console.WriteLine();

            // Get all users (one page)
            var result = await graphClient.Users
                .Request()
                .Select($"id,displayName,identities,{favouriteSeasonAttributeName},{lovesPetsAttributeName}")
                .GetAsync();

            foreach (var user in result.CurrentPage)
            {
                Console.WriteLine(JsonSerializer.Serialize(user));

                // Only output the custom attributes...
                //Console.WriteLine(JsonSerializer.Serialize(user.AdditionalData));
            }
        }

        public static async Task GetUserById(GraphServiceClient graphClient)
        {
            Console.Write("Enter user object ID: ");
            string userId = Console.ReadLine();

            Console.WriteLine($"Looking for user with object ID '{userId}'...");

            try
            {
                // Get user by object ID
                var result = await graphClient.Users[userId]
                    .Request()
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();

                if (result != null)
                {
                    Console.WriteLine(JsonSerializer.Serialize(result));
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static async Task GetUserBySignInName(GraphServiceClient graphClient)
        {
            Console.Write("Enter user sign-in name (username or email address): ");
            string userId = Console.ReadLine();

            Console.WriteLine($"Looking for user with sign-in name '{userId}'...");

            try
            {
                // Get user by sign-in name
                var result = await graphClient.Users
                    .Request()
                    .Filter($"identities/any(c:c/issuerAssignedId eq '{userId}' and c/issuer eq '{Environment.GetEnvironmentVariable("b2c-TenantId")}')")
                    .Select(e => new
                    {
                        e.DisplayName,
                        e.Id,
                        e.Identities
                    })
                    .GetAsync();

                if (result != null)
                {
                    Console.WriteLine(JsonSerializer.Serialize(result));
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static async Task DeleteUserById(GraphServiceClient graphClient)
        {
            Console.Write("Enter user object ID: ");
            string userId = Console.ReadLine();

            Console.WriteLine($"Looking for user with object ID '{userId}'...");

            try
            {
                // Delete user by object ID
                await graphClient.Users[userId]
                   .Request()
                   .DeleteAsync();

                Console.WriteLine($"User with object ID '{userId}' successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static async Task SetPasswordByUserId(GraphServiceClient graphClient)
        {
            Console.Write("Enter user object ID: ");
            string userId = Console.ReadLine();

            Console.Write("Enter new password: ");
            string password = Console.ReadLine();

            Console.WriteLine($"Looking for user with object ID '{userId}'...");

            var user = new User
            {
                PasswordPolicies = "DisablePasswordExpiration,DisableStrongPassword",
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = password,
                }
            };

            try
            {
                // Update user by object ID
                await graphClient.Users[userId]
                   .Request()
                   .UpdateAsync(user);

                Console.WriteLine($"User with object ID '{userId}' successfully updated.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        public static async Task UpdateUser(string serializedUser, string userId)
        {
            AweMediaUser user = JsonSerializer.Deserialize<AweMediaUser>(serializedUser);
            // Get the complete name of the custom attribute (Azure AD extension)
            Helpers.B2cCustomAttributeHelper helper = new Helpers.B2cCustomAttributeHelper(Environment.GetEnvironmentVariable("b2c-ExtensionAppId"));

            // Declare the names of the custom attributes
            const string userRolesAttribute = "UserRoles";
            const string tagAttribute = "Tag";

            string userRolesAttributeName = helper.GetCompleteAttributeName(userRolesAttribute);
            string tagAttributeName = helper.GetCompleteAttributeName(tagAttribute);

            // Fill custom attributes
            IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
            extensionInstance.Add(userRolesAttributeName, user.Role);
            extensionInstance.Add(tagAttributeName, "awemedia");


            var adb2cUser = new User
            {
                GivenName = user.Name,
                Surname = user.Name,
                DisplayName = user.Name,
                AdditionalData = extensionInstance,
                MobilePhone = user.Mobile,
                State = user.State,
                City = user.City,
                Country = user.CountryName,
            };

            try
            {
                // Update user by object ID
                await graphClient.Users[userId]
                   .Request()
                   .UpdateAsync(adb2cUser);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }


        public static async Task<Tuple<string,string>> CreateUserWithCustomAttribute(string serializedUser)
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("b2c-ExtensionAppId")))
            {
                throw new ArgumentException("B2C Extension App ClientId (ApplicationId) is missing in the appsettings.json. Get it from the App Registrations blade in the Azure portal. The app registration has the name 'b2c-extensions-app. Do not modify. Used by AADB2C for storing user data.'.");
            }

            // Declare the names of the custom attributes
            const string userRolesAttribute = "UserRoles";
            const string tagAttribute = "Tag";

            // Get the complete name of the custom attribute (Azure AD extension)
            Helpers.B2cCustomAttributeHelper helper = new Helpers.B2cCustomAttributeHelper(Environment.GetEnvironmentVariable("b2c-ExtensionAppId"));
            string userRolesAttributeName = helper.GetCompleteAttributeName(userRolesAttribute);
            string tagAttributeName = helper.GetCompleteAttributeName(tagAttribute);

            AweMediaUser user= JsonSerializer.Deserialize<AweMediaUser>(serializedUser);

            // Fill custom attributes
            IDictionary<string, object> extensionInstance = new Dictionary<string, object>();
            extensionInstance.Add(userRolesAttributeName, user.Role);
            extensionInstance.Add(tagAttributeName, "awemedia");

            try
            {
                // Create user
                var password = Helpers.PasswordHelper.GenerateNewPassword(4, 8, 4);
                var result = await graphClient.Users
                .Request()
                .AddAsync(new User
                {
                    GivenName = user.Name,
                    Surname = user.Name,
                    DisplayName = user.Name,
                    Identities = new List<ObjectIdentity>
                    {
                        new ObjectIdentity()
                        {
                            SignInType = "emailAddress",
                            Issuer = Environment.GetEnvironmentVariable("b2c-Tenant"),
                            IssuerAssignedId = user.Email
                        }
                    },
                    PasswordProfile = new PasswordProfile()
                    {
                        Password = password,
                        ForceChangePasswordNextSignIn=true
                    },
                    PasswordPolicies = "DisablePasswordExpiration",
                    AdditionalData = extensionInstance,
                    Mail = user.Email,
                    MobilePhone = user.Mobile,
                    State = user.State,
                    City = user.City,
                    Country = user.CountryName,
                });

                string userId = result.Id;

                // Get created user by object ID
                result = await graphClient.Users[userId]
                    .Request()
                    .Select($"id,givenName,surName,displayName,identities,{userRolesAttributeName},{tagAttributeName}")
                    .GetAsync();

                if (result != null)
                {
                    return new Tuple<string, string>(password, userId);
                }
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
