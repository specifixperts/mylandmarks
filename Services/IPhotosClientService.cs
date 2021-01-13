using MyLandmarks.Api.ViewModels.Flickr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MyLandmarks.Api.ViewModels.Photo;
namespace MyLandmarks.Api.Services
{
    public interface IPhotosClientService
    {
        //<Summary>
        //Place code that will combine the photo response to create a link to retrieve the photo
        //</Summary>
        string AnalyseResponse(ApiResponse response);
        //<Summary>
        //Place code that retrieve full details of a photo
        //</Summary>
        Task<DetailViewModel> GetPhotoDetails(string id);
    }
}
