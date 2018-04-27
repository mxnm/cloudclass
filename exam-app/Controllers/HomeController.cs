using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using exam_app.Models;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace exam_app.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString;

        public HomeController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetValue<string>("ConnectionStrings:StorageConnectionString");
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageModel();
            model.ImageSource = "ff";
            var account = CloudStorageAccount.Parse(this.connectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference("exam.gif");
            await blob.FetchAttributesAsync();
            byte[] imageBytes = new byte[blob.Properties.Length];
            await blob.DownloadToByteArrayAsync(imageBytes, 0);

            model.ImageSource = "data:image/gif;base64," + Convert.ToBase64String(imageBytes);

            return View(model);
        }
    }
}
