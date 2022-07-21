using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OidcApiAuthorization.Models;

namespace OidcApiAuthorization.Abstractions
{
    public interface IApiAuthorization
    {
        Task<ApiAuthorizationResult> AuthorizeAsync(IHeaderDictionary httpRequestHeaders);
        Task<ApiAuthorizationResult> AuthorizeAsync(HttpRequestHeaders httpRequestHeaders);
        Task<ApiAuthorizationResult> AuthorizeAsync(HttpRequestHeaders httpRequestHeaders, string role, string roleClaimName = "extension_UserRoles");
        Task<ApiAuthorizationResult> AuthorizeAsync(IHeaderDictionary httpRequestHeaders, string role, string roleClaimName = "extension_UserRoles");
    }
}
