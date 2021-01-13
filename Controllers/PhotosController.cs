using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLandmarks.Api.Attributes;
using MyLandmarks.Api.Models;
using MyLandmarks.Api.Services;
using LocationViewModel = MyLandmarks.Api.ViewModels.Location.IndexViewModel;
using PhotoLocationViewModel = MyLandmarks.Api.ViewModels.Photo.PhotoLocationViewModel;
using MyLandmarks.Api.ViewModels.Photo;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace MyLandmarks.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiKeyAuthorize]
    public class PhotosController : Controller
    {

        private readonly FourSquareClientService _fourSquareClientService;
        private readonly FlickrClientService _flickrClientService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PhotosController> _logger;
        private string userFolder { get; set; }
        public PhotosController(FourSquareClientService fourSquareClientService,
                                FlickrClientService flickrClientService,
                                GeoCitiesClientService locationsClientService,
                                AppDbContext context,
                                IWebHostEnvironment env,
                                IMapper mapper,
                                ILogger<PhotosController> logger)
        {
            _fourSquareClientService = fourSquareClientService;
            _flickrClientService = flickrClientService;
            _env = env;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{location}")]
        public async Task<IActionResult> Get(string location)
        {
            var photos = await _context.Locations.Where(p => p.Name.Contains(location)).SelectMany(x => x.Photos).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PhotoLocationViewModel>>(photos));
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(Guid locationid)
        {
            var userid = HttpContext.User.Identity.Name;
            var filePath = Path.Combine(_env.ContentRootPath, "Data", userid, "search.json");

            LocationViewModel searchloc = null;


            var oldresults = System.IO.File.ReadAllText(filePath);
            var searchedLocations = JsonConvert.DeserializeObject<IEnumerable<LocationViewModel>>(oldresults);
            searchloc = searchedLocations.Where(s => s.Id == locationid).FirstOrDefault();
            Location savedLocation = null;
            if (searchloc != null)
            {
                savedLocation = await _context.Locations.Where(l => l.Name == searchloc.Name).SingleOrDefaultAsync();
                if (savedLocation == null)
                {
                    await _context.Locations.AddAsync(_mapper.Map<Location>(searchloc));
                    try
                    {
                        _context.SaveChanges();
                        savedLocation = await _context.Locations.Where(l => l.Name == searchloc.Name).SingleOrDefaultAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError($"Error occured inserting photos from FourSquare : /n {ex.Message}");
                    }
                }
            }
            else
            {
                savedLocation = await _context.Locations.Where(l => l.Id == locationid).SingleOrDefaultAsync();
            }

            if (savedLocation == null) { return StatusCode(404); }

            if (searchloc != null)
            {
                var resFlickr = await _flickrClientService.Get(savedLocation.Latitude, savedLocation.Longitude, savedLocation.Id);
                var resFourSq = await _fourSquareClientService.Get(savedLocation.Latitude, savedLocation.Longitude, savedLocation.Id);

            }


            //retrive photos from the database
            var _photos = await _context.Locations.Where(p => p.Id == savedLocation.Id).SelectMany(x => x.Photos).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PhotoLocationViewModel>>(_photos));
        }

        [HttpGet("Photo/{id}")]
        public async Task<IActionResult> GetPhoto (Guid id)
        {
            var photo = await _context.Photos.Where(p => p.Id == id).SingleOrDefaultAsync();
            if (photo != null)
            {
                return Ok(_mapper.Map<DetailViewModel>(photo));
            }

            return StatusCode(404,"Image not found");
        }


    }

    public class Search
    {
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}