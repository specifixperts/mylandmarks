using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MyLandmarks.Api.Services
{
    public class ApiKeyMessageHandler : DelegatingHandler
    {
        private readonly AppUserManager _appUserManager;

        public ApiKeyMessageHandler(AppUserManager appUserManager)
        {
            _appUserManager = appUserManager;
        }

        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage,
        //    CancellationToken cancellationToken)
        //{
        //    bool validKey = false;
        //    IEnumerable<string> requestHeaders;
        //    var checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
        //    if (checkApiKeyExists)
        //    {
        //        var sentKey = requestHeaders.FirstOrDefault();
        //        var user = _appUserManager.FindByKeyAsync(sentKey);
        //        validKey = user != null;
        //    }
             
        //    if (!validKey)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.Forbidden);
        //    }

        //    var response = await base.SendAsync(httpRequestMessage, cancellationToken);
        //}
    }
}
