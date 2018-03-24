using book_exchange_initial.Entities;
using book_exchange_initial.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace book_exchange_initial.Services
{
    public class BookListingService
    {
        private string listingFolder;
        private string imagesFolder;

        public BookListingService(string listingFolder, string imagesFolder)
        {
            this.listingFolder = listingFolder;
            this.imagesFolder = imagesFolder;
        }

        public IEnumerable<BookListing> GetBookListings()
        {
            var listings = new List<BookListing>();
            Directory.CreateDirectory(this.listingFolder);
            var allListringFiles = Directory.GetFiles(this.listingFolder, "*.json");

            foreach (var listingFile in allListringFiles)
            {
                var jsonText = File.ReadAllText(listingFile);
                var listing = JsonConvert.DeserializeObject<BookListing>(jsonText);

                listings.Add(listing);
            }

            return listings;
        }

        public void SaveNewListing(BookListing newListing)
        {
            var filePath = this.listingFolder + newListing.Id + ".json";
            Directory.CreateDirectory(this.listingFolder);

            var json = JsonConvert.SerializeObject(newListing);
            File.WriteAllText(filePath, json);
        }
    }
}
