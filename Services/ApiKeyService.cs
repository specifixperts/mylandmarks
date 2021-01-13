using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly AppUserManager _appUserManager;
        public ApiKeyService(AppUserManager appUserManager)
        {
            _appUserManager = appUserManager;
        }

        public async Task<string> GetClientId(string apiKey)
        {
            var user = await _appUserManager.FindByKeyAsync(apiKey);
            return user.Id;
        }

        public async Task<bool> IsAuthorized(string apiKey)
        {
            var user = await _appUserManager.FindByKeyAsync(apiKey);
            return user != null;
        }
    }
}
