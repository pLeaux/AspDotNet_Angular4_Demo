using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;  
 

namespace VEGA.Mapping
{
    public class HttpUtils: IHttpUtils
    {
        private readonly IHttpContextAccessor httpContext;

        public HttpUtils (IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public string GetUserID()
        {
            Boolean isAuthenticated =  httpContext.HttpContext.User?.Identity?.IsAuthenticated ?? false; 
            if (isAuthenticated)  
                return (httpContext.HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value ?? "");
            else
                return "";
        }

    }
}
