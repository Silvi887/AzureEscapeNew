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
        [Range(1,20,ErrorMessage ="Adults count must be at least one!")]
        public int AdultsCount { get; set; } = 0;


        [Required]
        [Range(0,20,ErrorMessage ="Children count cannot be negative!")]
        public int ChildrenCount { get; set; } = 0;


        [Required(ErrorMessage ="You have to fill your First Name!")]
        public string GuestFirstName { get; set; } = "";


        [Required(ErrorMessage = "You have to fill your Last Name!")]
        public string LastNameG { get; set; } = "";

        [Required(ErrorMessage = "You have to fill your Date of Birth !")]
        public string DateofBirth { get; set; } = null!;


         [StringLength(200)]
        public string? GuestAddress { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? GuestPhoneNumber { get; set; }



        [Required(ErrorMessage = "Email is required.")]
        public string GuestEmail { get; set; } = "";



        //[Required]
        public string VillaId { get; set; }
        public bool IsGuest { get; set; }//user is autor of recipe

       
        public string VilaName { get; set; } = "";

        public decimal pricepernight { get;set; }


        //public IEnumerable<RoomViewModel> roomdrp = null!;
    }
}
