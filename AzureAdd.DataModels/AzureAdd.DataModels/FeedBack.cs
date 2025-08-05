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
    public class FeedBack
    {

        [Key]
        public int IdFeedBack { get; set; } 

        [Required]
        public int BookingId { get; set; } //have collection in Booking

        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;



        [Required]
        public int VillaId { get; set; }   //have collection in Vila

        [ForeignKey(nameof(VillaId))]
        public virtual VillaPenthhouse Villa { get; set; } = null!;


        [Required]
        public string GuestId { get; set; } = null!;

       
        [ForeignKey(nameof(GuestId))]     // have collection in Guest
        public virtual IdentityUser Guest { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLenght)]
        [MinLength(ValidationConstants.DescriptionMinLenght)]
        public string FeedbackMessage { get; set; } = null!;
        public int Rating { get; set; } // Optional: 1–5 stars
    }
}
