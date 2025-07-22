using Azure.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class Booking
    {

        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBooking { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int AdultsCount { get; set; }

        [Required]
        public int ChildrenCount { get; set; }

        //[Required]
        //public string BookedRoomsids { get; set; }


        [Required]
        public string GuestId { get; set; } = null!;

        [ForeignKey(nameof(GuestId))]
        public virtual IdentityUser Guest { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.GuestMaxLenght)]
        [MinLength(ValidationConstants.GuestMinLenght)]
        public string FirstName { get; set; } = null!;


        [Required]
        [MaxLength(ValidationConstants.GuestMaxLenght)]
        [MinLength(ValidationConstants.GuestMinLenght)]
        public string LastName { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }


        public string? Address { get; set; }

        public string? Email { get; set; }


        [Required]
        public string NumberOfPhone { get; set; } = null!;

        //[Required]
        //public int RoomId { get; set; }

        //[ForeignKey(nameof(RoomId))]
        //public virtual Room Room { get; set; } = null!;

        [Required]
        public int VillaId { get; set; }

        [ForeignKey(nameof(VillaId))]
        public virtual VillaPenthhouse VillaPenthhouse { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
    }
}
