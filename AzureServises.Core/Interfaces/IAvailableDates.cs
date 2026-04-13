using AzureApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServises.Core.Interfaces
{
    public interface IAvailableDates
    {
        Task<IEnumerable<BookingDatesViewModel>> bookingDatesViewModels(int vilaid);
    }
}
