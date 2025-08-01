using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class AddReservationViewModel
    {
        //public int IdReservation { get; set; }


        [Required]
        public string StartDate { get; set; } = null!;

        [Required]
        public string EndDate { get; set; } = null!;

        [Required]
        public int AdultsCount { get; set; } = 0;


        [Required]
        public int ChildrenCount { get; set; } = 0;


        [Required]
        public string GuestFirstName { get; set; } = "";


        [Required]
        public string LastNameG { get; set; } = "";

        [Required]
        public string DateofBirth { get; set; } = null!;

        public string? GuestAddress { get; set; }

        public string? GuestPhoneNumber { get; set; }



        [Required]
        public string GuestEmail { get; set; } = "";



        [Required]
        public string VillaId { get; set; }
        public bool IsGuest { get; set; }//user is autor of recipe

        [Required]
        public string VilaName { get; set; } = "";


        //public IEnumerable<RoomViewModel> roomdrp = null!;
    }
}
