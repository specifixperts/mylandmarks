using AutoMapper;
using MyLandmarks.Api.ViewModels.Location;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLandmarks.Api.Services
{
    public class RestCountriesClientService
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RestCountriesClientService(HttpClient client, AppDbContext context, IMapper mapper)
        {
            _client = client;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryViewModel>> Get(string name)
        {
            var response = await _client.GetAsync($"name/{name}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<CountryViewModel>();
            }
            //response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<CountryViewModel>>(responseStream);
        }
    }
}
