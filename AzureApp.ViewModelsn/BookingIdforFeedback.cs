using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class BookingIdforFeedback
    {

        [Key]
        public int IdBooking { get; set; }

        [Required]
        public string StringIdBooking { get; set; } = null!;
    }
}
