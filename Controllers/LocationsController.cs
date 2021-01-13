using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLandmarks.Api.Attributes;
using MyLandmarks.Api.Services;
using MyLandmarks.Api.ViewModels.Location;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MyLandmarks.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MyLandmarks.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiKeyAuthorize]
    public class LocationsController : Controller
    {
        private readonly GeoCitiesClientService _geocitiesClientService;
        private readonly RestCountriesClientService _restCountriesClientService;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private const string filename = "search.json";
        private readonly AppDbContext _context;

        public LocationsController(GeoCitiesClientService locationClientService,
                                   RestCountriesClientService restCountriesClientService,
                                   IWebHostEnvironment env,
                                   IMapper mapper,
                                   AppDbContext context)
        {
            _geocitiesClientService = locationClientService;
            _restCountriesClientService = restCountriesClientService;
            _env = env;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var locations = await _context.Locations.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<IndexViewModel>>(locations));
        }

        [HttpGet("Countries/{name}")]
        public async Task<IActionResult> Countries(string name)
        {
            var response = await _geocitiesClientService.Get(name);
            if (response.Count() > 0)
            {
                var folder = CreateFolder();


                string json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<Location>>(response));
                //write string to file
                System.IO.File.WriteAllText(Path.Combine(folder, filename), json);

                return Ok(JsonConvert.DeserializeObject<IEnumerable<IndexViewModel>>(json));
            }

            return StatusCode(404);
        }

        [HttpGet("Cities/{name}")]
        public async Task<IActionResult> Cities(string name)
        {
            var response = await _geocitiesClientService.Get(name);
            if (response.Count() > 0)
            {
                var folder = CreateFolder();

                string json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<Location>>(response));
                //write string to file
                System.IO.File.WriteAllText(Path.Combine(folder, filename), json);

                return Ok(JsonConvert.DeserializeObject<IEnumerable<IndexViewModel>>(json));
            }

            return StatusCode(404);

        }

        [HttpGet("All/{name}")]
        public async Task<IActionResult> All(string name)
        {

            var folder = CreateFolder();

            var response = await _geocitiesClientService.Get(name);
            var countriesResponse = await _restCountriesClientService.Get(name);

            IEnumerable<Location> searchResults = new List<Location>();
            //combine search results from Flickr and FourSquare
            if (response.Count() > 0) { searchResults = searchResults.Concat(_mapper.Map<IEnumerable<Location>>(response)); }
            if (countriesResponse.Count() > 0) { searchResults = searchResults.Concat(_mapper.Map<IEnumerable<Location>>(countriesResponse)); }

            string json = JsonConvert.SerializeObject(searchResults);
            System.IO.File.WriteAllText(Path.Combine(folder, filename), json);

            return Ok(JsonConvert.DeserializeObject<IEnumerable<IndexViewModel>>(json));
        }

        [HttpGet("Coord")]
        public async Task<IActionResult> Coord(string latlng)
        {

            var folder = CreateFolder();

            var response = await _geocitiesClientService.GetByCoord(latlng);

            string json = JsonConvert.SerializeObject(_mapper.Map<IEnumerable<Location>>(response));
            System.IO.File.WriteAllText(Path.Combine(folder, filename), json);

            return Ok(JsonConvert.DeserializeObject<IEnumerable<IndexViewModel>>(json));
        }

        private string CreateFolder()
        {
            var user = HttpContext.User.Claims;
            var userid = HttpContext.User.Identity.Name;
            if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "Data", userid)))
            {
                Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Data", userid));
            }

            return Path.Combine(_env.ContentRootPath, "Data", userid);
        }
    }
}