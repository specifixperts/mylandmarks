using MyLandmarks.Api.ViewModels.FourSquare;
using ApiResponse = MyLandmarks.Api.ViewModels.Photo.ApiResponse;
using DetailViewModel = MyLandmarks.Api.ViewModels.Photo.DetailViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MyLandmarks.Api.Models;

namespace MyLandmarks.Api.Services
{
    public class FourSquareClientService : IPhotosClientService
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;
        private readonly ILogger<FourSquareClientService> _logger;
        public FourSquareClientService(HttpClient client, AppDbContext context, ILogger<FourSquareClientService> logger)
        {
            _client = client;
            _logger = logger;
            _context = context;
        }

        public string AnalyseResponse(ApiResponse response)
        {
            var photoSourceModel = (ViewModels.FourSquare.IndexViewModel)response;
            return $"{photoSourceModel.Prefix}{photoSourceModel.Width}x{photoSourceModel.Height}{photoSourceModel.Suffix}";
        }

        //<Summary>
        // Returns the list of venues provided a [name] of a location. 
        //</Summary>
        //public async Task<IEnumerable<string>> Get(string name)
        //{

        //    //Get Venues
        //    //var fsResponse = await _client.GetAsync($"search?client_id=2LLL0I4XU2IINZTRUZHEBKOJGMMY4D32BDLTW5IXV1AHUBNG&client_secret=UADFNMIALJXGXVUZST4WVNMFWTB4HVY1EJGE3VKOYPJ34SUI&v=20180323&near={name}&intent=browse&radius=10000&categoryId=4bf58dd8d48988d12d941735&llAcc=50000");
        //    var fsResponse = await _client.GetAsync($"search?client_id=2LLL0I4XU2IINZTRUZHEBKOJGMMY4D32BDLTW5IXV1AHUBNG&client_secret=UADFNMIALJXGXVUZST4WVNMFWTB4HVY1EJGE3VKOYPJ34SUI&v=20180323&near={name}&intent=browse&radius=10000&categoryId=4bf58dd8d48988d12d941735&llAcc=50000");
        //    fsResponse.EnsureSuccessStatusCode();
        //    var fsResponseStream = JObject.Parse(await fsResponse.Content.ReadAsStringAsync());
        //    var fsResult = fsResponseStream["response"]["venues"].ToObject<IEnumerable<VenueViewModel>>();
        //    IEnumerable<string> fsPhotos = new List<string>();

        //    //Get Photos
        //    foreach (var item in fsResult)
        //    {
        //        fsPhotos = fsPhotos.Concat(await GetPhotos(item));
        //    }

        //    return fsPhotos;
        //}

        //<Summary>
        // Returns the list of venues provided a coordinates [latitude,longitude] of a location. 
        //</Summary>
        public async Task<string> Get(string lat, string lng, Guid locationid)
        {

            //Get Venues
            var fsResponse = await _client.GetAsync($"search?client_id=2LLL0I4XU2IINZTRUZHEBKOJGMMY4D32BDLTW5IXV1AHUBNG&client_secret=UADFNMIALJXGXVUZST4WVNMFWTB4HVY1EJGE3VKOYPJ34SUI&v=20180323&ll={lat},{lng}&intent=browse&radius=10000&categoryId=4bf58dd8d48988d12d941735");
            if (!fsResponse.IsSuccessStatusCode)
            {
                _logger.LogError(fsResponse.ReasonPhrase);
                return fsResponse.ReasonPhrase;
            }
            var fsResponseStream = JObject.Parse(await fsResponse.Content.ReadAsStringAsync());
            var fsResult = fsResponseStream["response"]["venues"].ToObject<IEnumerable<VenueViewModel>>();
            IList<string> fsPhotos = new List<string>();

            //Get Photos
            foreach (var item in fsResult)
            {

                fsPhotos.Add(await GetPhotos(item, locationid));
            }

            return fsPhotos.ToString();
        }

        public Task<DetailViewModel> GetPhotoDetails(string id)
        {
            throw new NotImplementedException();
        }

        //<Summary>
        // Returns the list of photos for specified [Venue]. 
        //</Summary>
        public async Task<string> GetPhotos(VenueViewModel venue, Guid locationid)
        {
            var fsPhotoResponse = await _client.GetAsync($"{venue.Id}/photos?client_id=2LLL0I4XU2IINZTRUZHEBKOJGMMY4D32BDLTW5IXV1AHUBNG&client_secret=UADFNMIALJXGXVUZST4WVNMFWTB4HVY1EJGE3VKOYPJ34SUI&v=20180323&group=venue&limit=10");
            if (!fsPhotoResponse.IsSuccessStatusCode)
            {
                _logger.LogError(fsPhotoResponse.ReasonPhrase);
                return fsPhotoResponse.ReasonPhrase;
            }
            var fsResponseStream = JObject.Parse(await fsPhotoResponse.Content.ReadAsStringAsync());
            var photosInfo = fsResponseStream["response"]["photos"]["items"].ToObject<IEnumerable<ViewModels.FourSquare.IndexViewModel>>();
            foreach (var item in photosInfo)
            {

                var check = await _context.Photos.Where(p => p.Url == AnalyseResponse(item)).SingleOrDefaultAsync();
                if (check == null)
                {
                    var newImge = new Photo
                    {
                        DateCreated = DateTime.Now,
                        Deleted = false,
                        Source = "FourSquare",
                        Url = AnalyseResponse(item),
                        SourceId = item.Id,
                        Height = item.Height,
                        Width = item.Width
                    };
                    newImge.DateCreated = new DateTime(1970, 1, 1).AddSeconds(Convert.ToInt32(item.CreatedAt));

                    var savedImg = await _context.Photos.AddAsync(newImge);
                    await _context.PhotoLocations.AddAsync(new PhotoLocation { DateCreated = DateTime.Now, Deleted = false, LocationId = locationid, PhotoId = savedImg.CurrentValues.GetValue<Guid>("Id") });
                }
                else
                {
                    var saved = await _context.PhotoLocations.Where(p => p.LocationId == locationid && p.PhotoId == check.Id).SingleOrDefaultAsync();
                    if (saved == null)
                    {
                        await _context.PhotoLocations.AddAsync(new PhotoLocation { DateCreated = DateTime.Now, Deleted = false, LocationId = locationid, PhotoId = check.Id });
                    }

                }
                try
                {
                    if (_context.ChangeTracker.HasChanges())
                    {
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Error occured inserting photos from FourSquare : {ex.Message}");
                }

            }

            return "success";
        }

        public int SavePhoto(ApiResponse photo)
        {
            throw new NotImplementedException();
        }
    }
}
