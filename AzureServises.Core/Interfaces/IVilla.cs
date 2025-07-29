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

        Task<IEnumerable<FavoriteVilaIndexViewModel>> GetFavoritePlaces(string? Userid);

        Task<bool> FavoritePlaces(string Userid, int favoritehotelid);
        Task<bool> RemoveFavorite(string Userid, int? id);

        Task<DeleteReservationIndexViewModel> GetForDeleteReservation(int? id, string? Userid);
        Task<bool> DeleteReservation(string Userid, int? id);

        Task<EditBooking> GetForEditReservation(int? id, string? Userid);

        Task<bool> EditReservation(string UserId, EditBooking editbooking);

    }
}
