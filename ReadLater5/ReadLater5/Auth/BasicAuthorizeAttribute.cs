using Entity.AuthEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Interface;
using System.Linq;
using System.Web.Mvc;

namespace ReadLater5.Auth
{
    public class BasicAuthorizeAttribute : FilterAttribute, Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        private readonly Permission _userPermission;

        public BasicAuthorizeAttribute(Permission userPermission)
        {
            _userPermission = userPermission;
        }
        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.ASCII.GetString(base64EncodedBytes);
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var clientService = filterContext.HttpContext.RequestServices.GetService(typeof(IClientService))
               as IClientService;
            var authHeader = filterContext.HttpContext.Request.Headers["Authorization"];

            if (authHeader.Any() && authHeader[0].Contains("Basic"))
            {
                var hash = authHeader[0][6..];

                var encodedString = Base64Decode(hash);

                var splitData = encodedString.Split(":");

                var User = splitData[0];
                var Key = splitData[1];

                var client = clientService.GetClientbyUser(User);
                if (client != null && client.UserAccess == Permission.ReadWrite)
                    return;

                if (client == null)
                {
                    filterContext.Result = new ObjectResult("Client not found") { StatusCode = 400 };
                }
                else if (client != null && client.Key != Key)
                {
                    filterContext.Result = new ObjectResult("Unauthorized") { StatusCode = 401 };
                }
                else if (client != null && client.UserAccess != _userPermission)
                {
                    filterContext.Result = new ObjectResult("Forbidden") { StatusCode = 403 };
                }
            }
            else
            {
                filterContext.Result = new ObjectResult("Unauthorized") { StatusCode = 401 };
            }
        }
    }
}
