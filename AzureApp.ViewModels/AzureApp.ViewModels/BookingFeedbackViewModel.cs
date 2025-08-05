using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class BookingFeedbackViewModel
    {
        public int IdBooking { get; set; }

        public int VillaId { get; set; }
        public string? VillaName { get; set; } = "";
        public string ClientName { get; set; }
        public string FeedbackMessage { get; set; }
        public int Rating { get; set; } // Optional: 1–5 stars

        public IEnumerable<BookingIdforFeedback>? idbookingsforuser { get; set; }
    }
}
