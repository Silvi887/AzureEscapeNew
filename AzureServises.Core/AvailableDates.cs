using AzureAdd.Data;
using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServises.Core
{
    public class AvailableDates : IAvailableDates
    {


        private readonly AzureAddDbContext Dbcontext;
        private readonly UserManager<IdentityUser> userManager;

        public AvailableDates(AzureAddDbContext villaDbcontext, UserManager<IdentityUser> usermanager)
        {
            this.Dbcontext = villaDbcontext;
            this.userManager = usermanager;
        }
        public async Task<IEnumerable<BookingDatesViewModel>> bookingDatesViewModels(int vilaid)
        {

            var bookingDates = await Dbcontext.Bookings
                     .Where(b => b.VillaId == vilaid)
                     .Select(v => new BookingDatesViewModel()
                     {
                         StartDate = v.StartDate.ToString("yyyy-MM-dd"),
                         EndDate = v.EndDate.ToString("yyyy-MM-dd")
                     })
                     .Distinct()
                     .ToListAsync();

            return bookingDates;


        }
    }
}
