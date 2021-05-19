using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.ActionConstraints
{
    [AttributeUsage(AttributeTargets.All, Inherited =true, AllowMultiple =true)]
    public class RequestHeaderMatchesMediaTypeAttribute: Attribute, IActionConstraint
    {
        private readonly string requestHeaderToMatch;
        private readonly string[] mediaTypes;

        public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch, string[] mediaTypes)
        {
            this.requestHeaderToMatch = requestHeaderToMatch;
            this.mediaTypes = mediaTypes;
        }

        public int Order { 
            get { return 0;  }
        }

        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey(this.requestHeaderToMatch))
            {
                return false;
            }

            foreach (var mediaType in mediaTypes)
            {
                var headerValues = requestHeaders[requestHeaderToMatch].ToString().Split(',').ToList();
                foreach (var headerValue in headerValues)
                {
                    if(string.Equals(headerValue, mediaType, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
