using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class DeleteReservationIndexViewModel
    {
        [Key]
        public int IdBooking { get; set; }

        [Required]
        public string StartDate { get; set; } = null!;

        [Required]
        public string EndDate { get; set; } = null!;

        [Required]
        public string GuestFirstName { get; set; } = "";

        public string HotelName { get; set; } = "";
    }
}
