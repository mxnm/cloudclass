using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using text_analysis.Models;
using System;

namespace text_analysis.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<AppSettings> appSettings;
        private string baseTextUri;

        public HomeController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
             var account = CloudStorageAccount.Parse(this.appSettings.Value.StorageConnectionString);
             this.baseTextUri = account.BlobEndpoint.ToString() + "texts/";
        }

        public async Task<IActionResult> Index()
        {
            var account = CloudStorageAccount.Parse(this.appSettings.Value.StorageConnectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("textinfo");
            await table.CreateIfNotExistsAsync();

            TableContinuationToken token = null;
            var query = new TableQuery<TextInfoTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "text"));
            var result = await table.ExecuteQuerySegmentedAsync(query, token);

            var model = result.Results
                .Select(r => new TextInfo()
                {
                    TextId = r.RowKey,
                    WordCount = r.WordCount,
                    Complexity = r.Complexity,
                    UniqueWords = r.UniqueWords,
                    LinkToText = this.baseTextUri + r.RowKey
                });

            return View(model);
        }

        [HttpGet]
        public ActionResult Submit()
        {
            return View();
        }

        [ActionName("Submit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitText(TextSubmissionModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var id = Guid.NewGuid().ToString();
            var account = CloudStorageAccount.Parse(this.appSettings.Value.StorageConnectionString);
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("texts");
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(id);
            await blob.UploadTextAsync(model.Content);

            return this.RedirectToAction("Submitted", "Home", new { id = id });
        }

        [HttpGet]
        public ActionResult Submitted(string id)
        {
            return this.View("Submitted", id);
        }
    }
}
