using AzureApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServises.Core.Interfaces
{
    public interface ITownService
    {
        Task<IEnumerable<TownIndexViewModel>> TownViewDataAsync();
        Task<IEnumerable<TypePlaceIndexViewModel>> TypePlaceViewDataAsync();
        Task<IEnumerable<BookingIdforFeedback>> TypeIdBookingFeedback(string? userid, string idbooking);
    }
}
