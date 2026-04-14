using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class ApplicationUser :IdentityUser
    {
        public virtual Manager? Manager { get; set; }

        public virtual ICollection<UserVilla> UserVillas { get; set; }// Favorite 
            = new HashSet<UserVilla>();

        public virtual ICollection<VillaPenthhouse> VillaPenths { get; set; } //AllVillas
            = new HashSet<VillaPenthhouse>();

        public virtual ICollection<Booking> AllBookings { get; set; }// AllBooking
            = new HashSet<Booking>();

        public virtual ICollection<FeedBack> AllFeedbackss { get; set; }// Allfeedback
         = new HashSet<FeedBack>();
    }
}
