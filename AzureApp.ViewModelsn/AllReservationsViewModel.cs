using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class AllReservationsViewModel
    {

        [Key]
        public int IdBooking { get; set; }

        [Required]
        public string VilaName { get; set; } = "";

        [Required]
        public string StartDate { get; set; } = null!;

        [Required]
        public string EndDate { get; set; } = null!;

        [Required]
        public bool IsUserGuest { get; set; }

        public bool IsManager { get; set; }
    }
}
