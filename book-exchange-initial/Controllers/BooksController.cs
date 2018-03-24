using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using book_exchange_initial.Models;
using Microsoft.Extensions.Options;
using book_exchange_initial.Services;
using Microsoft.AspNetCore.Hosting;
using book_exchange_initial.Entities;
using System.IO;

namespace book_exchange_initial.Controllers
{
    public class BooksController : Controller
    {
        private string imagesFolder;
        private string listingsFolder;
        private IOptions<AppSettings> appSettings;

        public BooksController(IOptions<AppSettings> appSettings, IHostingEnvironment hostingEnvironment)
        {
            this.appSettings = appSettings;
            this.imagesFolder = Path.Combine(hostingEnvironment.WebRootPath, appSettings.Value.ImagesFolder);
            this.listingsFolder = Path.Combine(hostingEnvironment.WebRootPath, appSettings.Value.ListingsFolder);
        }

        public ActionResult Index()
        {
            var bookService = new BookListingService(this.listingsFolder, this.imagesFolder);
            var listings = bookService.GetBookListings();
            var books = this.GetBookListingModels(listings);

            return View(books);
        }

        private IEnumerable<BookListingModel> GetBookListingModels(IEnumerable<BookListing> listings)
        {
            var books = new List<BookListingModel>();

            foreach (var listing in listings.OrderByDescending(l => l.PublishedOn))
            {
                // Create model for page
                var viewModel = BookListingModel.FromListing(listing);

                // Set image URLs
                if (listing.ImageId != null)
                {
                    viewModel.ImageThumbnailUrl = appSettings.Value.ImagesFolder + listing.ImageId + "_thumb.jpg";
                    viewModel.ImageFullUrl = appSettings.Value.ImagesFolder + listing.ImageId + ".jpg";
                }

                books.Add(viewModel);
            }

            return books;
        }

        [HttpGet]
        public ActionResult Submit()
        {
            return View();
        }

        [ActionName("Submit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitPost(BookListingSubmissionModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var bookService = new BookListingService(this.listingsFolder, this.imagesFolder);
            var newListing = BookListing.New(model.BookTitle, model.BookDescription, model.Price);

            if (model.BookImage != null)
            {
                var imageService = new ImageProcessingService(this.imagesFolder);
                imageService.ProcessImage(model.BookImage.OpenReadStream(), newListing);
            }

            bookService.SaveNewListing(newListing);

            return this.RedirectToAction("Index", "Books");
        }
    }
}
