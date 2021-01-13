using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLandmarks.Api.Services
{
    public interface IApiKeyService
    {
        Task<string> GetClientId(string apiKey);

        Task<bool> IsAuthorized(string apiKey);
    }
}
