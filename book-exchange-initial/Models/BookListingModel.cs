using book_exchange_initial.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book_exchange_initial.Models
{
    public class BookListingModel
    {
        public string BookId
        {
            get;
            set;
        }

        public string BookTitle
        {
            get;
            set;
        }

        public string BookDescription
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public string ImageThumbnailUrl
        {
            get;
            set;
        }

        public string ImageFullUrl
        {
            get;
            set;
        }

        public static BookListingModel FromListing(BookListing listing)
        {
            var viewModel = new BookListingModel();
            viewModel.BookDescription = listing.Description;
            viewModel.BookId = listing.Id;
            viewModel.BookTitle = listing.Title;
            viewModel.Price = Math.Round(listing.Price, 2);

            return viewModel;
        }
    }
}
