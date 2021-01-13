using MyLandmarks.Api.ViewModels.Flickr;
using MyLandmarks.Api.Models;
using ApiResponse = MyLandmarks.Api.ViewModels.Photo.ApiResponse;
using DetailViewModel = MyLandmarks.Api.ViewModels.Photo.DetailViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyLandmarks.Api.Services
{
    public class FlickrClientService : IPhotosClientService
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;
        private readonly ILogger<FlickrClientService> _logger;

        public FlickrClientService(HttpClient client, AppDbContext context, ILogger<FlickrClientService> logger)
        {
            _client = client;
            _context = context;
            _logger = logger;
        }

        public string AnalyseResponse(ApiResponse response)
        {
            var photoSourceModel = (IndexViewModel)response;
            return $"https://live.staticflickr.com/{photoSourceModel.Server}/{photoSourceModel.Id}_{photoSourceModel.Secret}.jpg";
        }


        public async Task<string> Get(string lat, string lng, Guid locationid)
        {
            //Get photos from Flickr

            var flickrResponse = await _client.GetAsync($"?api_key=57413f30e136cb63363b4257274eddd8&method=flickr.photos.search&media=photos&tags=landmark&geo_context=2&format=json&nojsoncallback=2&lat={lat}&lon={lng}");
            if (!flickrResponse.IsSuccessStatusCode)
            {
                _logger.LogError(flickrResponse.ReasonPhrase);
                return flickrResponse.ReasonPhrase;
            }
            var flickrResponseStream = JObject.Parse(await flickrResponse.Content.ReadAsStringAsync());
            var flickrResult = flickrResponseStream["photos"]["photo"].ToObject<IEnumerable<IndexViewModel>>();

            foreach (var item in flickrResult)
            {
                var check = await _context.Photos.Where(p => p.Url == AnalyseResponse(item)).SingleOrDefaultAsync();

                if (check == null)
                {

                    var newImge = new Photo { DateCreated = DateTime.Now, Deleted = false, Source = "Flickr", Url = AnalyseResponse(item) };
                    var imgDetails = await GetPhotoDetails(item.Id);
                    newImge.Height = imgDetails.Height;
                    newImge.Width = imgDetails.Width;
                    newImge.DateTaken = imgDetails.DateTaken;
                    newImge.SourceId = item.Id;
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
                    _logger.LogError($"Error occured inserting photos from Flickr : {ex.Message}");
                }

            }

            return "Success";
        }

        public async Task<DetailViewModel> GetPhotoDetails(string photoId)
        {
            var response = await _client.GetAsync($"?method=flickr.photos.getInfo&api_key=57413f30e136cb63363b4257274eddd8&photo_id={photoId}&format=json&nojsoncallback=1");
            response.EnsureSuccessStatusCode();
            var responseStream = JObject.Parse(await response.Content.ReadAsStringAsync());

            DetailViewModel detailView = new DetailViewModel { SourceId = photoId };
            
            detailView.DateTaken = DateTime.Parse(responseStream["photo"]["dates"]["taken"].ToString());
            _logger.LogInformation($"Flickr: images date taken obtained");

            var responseSizes = await _client.GetAsync($"?method=flickr.photos.getSizes&api_key=57413f30e136cb63363b4257274eddd8&photo_id={photoId}&format=json&nojsoncallback=1");
            responseSizes.EnsureSuccessStatusCode();
            var responseStreamSizes = JObject.Parse(await responseSizes.Content.ReadAsStringAsync());
            IEnumerable<ImageSize> sizes = responseStreamSizes["sizes"]["size"].ToObject<IEnumerable<ImageSize>>();
            try
            {
                detailView.Height = sizes.Where(d => d.Label == "Large" || d.Label == "Original").First().Height;
                detailView.Width = sizes.Where(d => d.Label == "Large" || d.Label == "Original").First().Width;
                _logger.LogInformation($"Flickr: images sizes obtained");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Photo Details Error: {ex.Message}");
            }
            

            return detailView;
        }

        public int SavePhoto(ApiResponse photo)
        {
            throw new NotImplementedException();
        }

    }

    public class ImageSize
    {
        public string Label { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
