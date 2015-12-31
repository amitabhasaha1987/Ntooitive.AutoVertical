using PagedList;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models
{
    public class ListingViewModel
    {
        public IEnumerable<CarListing> CarListing { get; set; }
        public StaticPagedList<CarListing> CarPageListings { get; set; }
        public AdvanceSearch advSearch { get; set; }
        public int RecordCount { get; set; }

        public string ProfileImage { get; set; }
    }
}