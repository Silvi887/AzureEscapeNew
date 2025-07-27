using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureApp.ViewModels;

namespace AzureServises.Core.Interfaces
{
    public interface IVilla
    {

        Task<IEnumerable<VilaIndexViewModel>> GetAllVillasAsync(string? UserId);
        Task<bool> AddVilaModel(string Userid, AddVillaIndexViewModel vilamodel);

        Task<bool> AddBookingModel(string Userid, AddReservationViewModel reservationmodel);

        Task<IEnumerable<AllReservationsViewModel>> GetAllReservations(string? Userid);
    }
}
