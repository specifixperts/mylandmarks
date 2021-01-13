using AutoMapper;
using MyLandmarks.Api.Models;
using MyLandmarks.Api.ViewModels.Location;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace MyLandmarks.Api.Services
{
    public class GeoCitiesClientService
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GeoCitiesClientService(HttpClient client, AppDbContext context, IMapper mapper)
        {
            _client = client;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityViewModel>> Get(string name)
        {
            var response = await _client.GetAsync($"cities?namePrefix={name}");

            response.EnsureSuccessStatusCode();

            var responseStream = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseStream["data"].ToObject<IEnumerable<CityViewModel>>();
        }

        public async Task<IEnumerable<CityViewModel>> GetByCoord(string latlng)
        {
            
            var response = await _client.GetAsync($"cities?limit=5&offset=0&location={WebUtility.UrlEncode(latlng)}&radius=500");

            response.EnsureSuccessStatusCode();

            var responseStream = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseStream["data"].ToObject<IEnumerable<CityViewModel>>();
        }

        public async Task<int> Put(CityViewModel model)
        {
            await _context.Locations.AddAsync(_mapper.Map<Location>(model));
            return _context.SaveChanges();
        }
    }
}
