using Azure.Constants;
using AzureAdd.Data;
using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServises.Core
{
    public class VillaService:IVilla
    {

        private readonly AzureAddDbContext Dbcontext;
        private readonly UserManager<IdentityUser> userManager;

        public VillaService(AzureAddDbContext pDbcontext, UserManager<IdentityUser> usermanager)
        {
            this.Dbcontext = pDbcontext;
            this.userManager = usermanager;
        }

        public async Task<bool> AddBookingModel(string Userid, AddReservationViewModel reservationmodel)
        {
            try
            {


                bool operationResult = false;
                IdentityUser? user1 = await this.userManager.FindByIdAsync(Userid);

                //string? UserId = this.GetUserId();
                //int idroom = int.Parse(reservationmodel.RoomId);
                //var Room = this.Dbcontext.Rooms.FindAsync(idroom);

                if (user1 != null)
                {
                    Booking reservation1 = new Booking()
                    {
                        StartDate = DateTime.ParseExact(reservationmodel.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None),

                        EndDate = DateTime.ParseExact(reservationmodel.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        AdultsCount = reservationmodel.AdultsCount,

                        ChildrenCount = reservationmodel.ChildrenCount,
                        GuestId = Userid,

                        //Guest { get; set; } = null!;

                       // RoomId = int.Parse(reservationmodel.RoomId),
                        VillaId = int.Parse(reservationmodel.VillaId),
                        FirstName = reservationmodel.GuestFirstName,
                        LastName = reservationmodel.LastNameG,
                        DateOfBirth = DateTime.ParseExact(reservationmodel.DateofBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Address = reservationmodel.GuestAddress,
                        Email = reservationmodel.GuestEmail,
                        NumberOfPhone = reservationmodel.GuestPhoneNumber

                    };

                    //Guest guest = new Guest()
                    //{
                    //    IdGuest= Userid,
                    //    FirstName = reservationmodel.GuestFirstName,
                    //    LastName = reservationmodel.LastNameG,
                    //    DateOfBirth = DateTime.ParseExact(reservationmodel.DateofBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    //    Address = reservationmodel.GuestAddress,
                    //    Email = reservationmodel.GuestEmail,
                    //    NumberOfPhone = reservationmodel.GuestPhoneNumber


                    //};

                    //UserReservation reservation = new UserReservation()
                    //{
                    //    Reservation= reservation1,
                    //    User=guest
                    //}


                    await this.Dbcontext.Bookings.AddAsync(reservation1);
                    // await this.Dbcontext.Guests.AddAsync(guest);

                    await this.Dbcontext.SaveChangesAsync();
                    operationResult = true;

                }
                ;




                return operationResult;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> AddVilaModel(string Userid, AddVillaIndexViewModel vilamodel)
        {


            try
            {
                bool operationResult = false;
                IdentityUser? user1 = await this.userManager.FindByIdAsync(Userid);


                if (user1 != null)
                {
                    VillaPenthhouse villa = new VillaPenthhouse()
                    {

                        NameVilla = vilamodel.NameVilla,

                        IdPlace = vilamodel.IdTypePlace,

                        VillaInfo = vilamodel.VillaInfo,

                        VillaAddress = vilamodel.VillaAddress,

                        ImageUrl = vilamodel.ImageUrl,
                        CountRooms = vilamodel.CountRooms,

                        CountAdults = vilamodel.CountAdults,

                        CountChildren = vilamodel.CountChildren,


                        Bedrooms = vilamodel.Bedrooms,

                        Bathrooms = vilamodel.Bathrooms,

                        Area = vilamodel.Area,

                        Parking = vilamodel.Parking,

                        LocationId = vilamodel.IdTown,


                        IDManager = user1.Id


                    };
                    Dbcontext.VillasPenthhouses.Add(villa);
                    Dbcontext.SaveChanges();

                    operationResult = true;

                }

                return operationResult;
            }
              catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
           // throw new NotImplementedException();
        }

     

        public async Task<IEnumerable<AllReservationsViewModel>> GetAllReservations(string? Userid)
        {
            IdentityUser? user = await userManager.FindByIdAsync(Userid);
            var reservations = await Dbcontext.Bookings
                                .Include(r => r.VillaPenthhouse)
                                .AsNoTracking()
                                .Where(r => r.IsDeleted == false)   /* r.GuestId == Userid &&*/
                                .Select(r => new AllReservationsViewModel()
                                {
                                    IdBooking = r.IdBooking,
                                    VilaName = r.VillaPenthhouse.NameVilla,
                                    StartDate = r.StartDate.ToString(ValidationConstants.DateFormat),
                                    EndDate = r.EndDate.ToString(ValidationConstants.DateFormat),
                                    IsUserGuest = user != null ? r.GuestId == user.Id : false,
                                }).ToListAsync();



            return reservations;
        }

        public async Task<IEnumerable<VilaIndexViewModel>> GetAllVillasAsync(string? UserId)
        {

            var Allhotels = await Dbcontext.VillasPenthhouses
                .Include(v=> v.Location)
                .Include(v => v.TypePlace)
                .Include(v=> v.Feedbacks)
                .Where(v => v.IsDeleted == false)
                .Select(h => new VilaIndexViewModel()
                {
                    IdVilla = h.IdVilla,
                    NameVilla = h.NameVilla,
                    LocationName = h.Location.NameLocation,
                    NamePlace = h.TypePlace.NamePlace,
                    VillaAddress = h.VillaAddress,
                    ImageUrl = h.ImageUrl,
                    CountAdults = h.CountAdults,
                    CountChildren = h.CountChildren,
                    Bedrooms = h.Bedrooms,
                    Bathrooms = h.Bathrooms,
                    Area = h.Area,
                    Parking = h.Parking,
                    Raiting= h.Feedbacks.Sum(v => v.Rating)
                }).ToListAsync();

            return Allhotels;
        }

        public async Task<IEnumerable<FavoriteVilaIndexViewModel>> GetFavoritePlaces(string? Userid)
        {
            bool issuccessfavorite = false;
            IdentityUser? curentuser = await userManager.FindByIdAsync(Userid);
            IEnumerable<FavoriteVilaIndexViewModel>? favoritehotel = null;


            if (curentuser != null)
            {
                favoritehotel = await Dbcontext.UserVilla
                    .Where(f => f.UserId.ToLower() == curentuser.Id.ToLower() && f.Villa.IsDeleted == false)
                    .Include(f => f.Villa.Location)
                     .Select(h => new FavoriteVilaIndexViewModel()
                     {
                         IdVila = h.VillaId,
                         TownName = h.Villa.Location.NameLocation,
                         ImageUrl = h.Villa.ImageUrl,
                         VilaName = h.Villa.NameVilla,
                     }).ToArrayAsync();

                issuccessfavorite = true;
            }

            return favoritehotel;
        }

        public async Task<bool> FavoritePlaces(string Userid, int favoritehotelid)
        {
            bool issuccessfavorite = false;
            IdentityUser? curentuser = await userManager.FindByIdAsync(Userid);

            VillaPenthhouse? currentPlace1 = await Dbcontext.VillasPenthhouses.FindAsync(favoritehotelid);

            if (curentuser != null && currentPlace1 != null) /* && currentPlace.IDManager != curentuser.Id */
            {
                UserVilla CurrentPlace = await Dbcontext.UserVilla.SingleOrDefaultAsync(h => h.VillaId == currentPlace1.IdVilla && h.UserId == curentuser.Id);


                if (CurrentPlace == null)
                {

                    CurrentPlace = new UserVilla()
                    {
                        UserId = curentuser.Id,
                        VillaId = favoritehotelid
                        // HotelID = idhotel,
                        //UserId = curentuser.Id
                    };
                    await Dbcontext.UserVilla.AddAsync(CurrentPlace);
                }

                await this.Dbcontext.SaveChangesAsync();

                issuccessfavorite = true;
            }



            return issuccessfavorite;
        }

        public async Task<bool> RemoveFavorite(string Userid, int? id)
        {
            bool isdeleted = false;
            IdentityUser? curentuser = await userManager.FindByIdAsync(Userid);
            VillaPenthhouse? hoteltoremove = await Dbcontext.VillasPenthhouses.SingleOrDefaultAsync(h => h.IdVilla == id);
            if (curentuser != null && hoteltoremove != null)
            {
                hoteltoremove.IsDeleted = true;


                await this.Dbcontext.SaveChangesAsync();

                isdeleted = true;
            }



            return isdeleted;
        }

        public async Task<DeleteReservationIndexViewModel> GetForDeleteReservation(int? id, string? Userid)
        {

            IdentityUser? currentUser = await userManager.FindByIdAsync(Userid);
            DeleteReservationIndexViewModel? reservation1 = null;

            if (currentUser != null)
            {

                Booking? curentreservation = await Dbcontext.Bookings.Include(r => r.VillaPenthhouse)

                    .FirstOrDefaultAsync(r => r.IdBooking == id);

                reservation1 = new DeleteReservationIndexViewModel()
                {


                    StartDate = curentreservation.StartDate.ToString("yyyy-MM-dd"),

                    EndDate = curentreservation.StartDate.ToString("yyyy-MM-dd"),
                    IdBooking = curentreservation.IdBooking,
                    GuestFirstName = curentreservation.FirstName + " " + curentreservation.LastName,
                    HotelName = curentreservation.VillaPenthhouse.NameVilla

                    //AdultsCount = curentreservation.AdultsCount,

                    //ChildrenCount = curentreservation.ChildrenCount,

                    //RoomId = curentreservation.RoomId.ToString(),
                    //HotelId = curentreservation.HotelId.ToString(),
                    //HotelName = curentreservation.Hotel.HotelName,
                    //GuestFirstName = curentreservation.FirstName,
                    //LastNameG = curentreservation.LastName,
                    //DateofBirth = curentreservation.DateOfBirth.ToString("yyyy-MM-dd"),
                    //GuestAddress = curentreservation.Address,
                    //GuestEmail = curentreservation.Email,
                    //GuestPhoneNumber = curentreservation.NumberOfPhone
                };

            }


            return reservation1;
        }

        public async Task<bool> DeleteReservation(string Userid, int? id)
        {

            bool issuccessdelete = false;

            IdentityUser? curentuser = await userManager.FindByIdAsync(Userid);

            Booking? currentReservation = await Dbcontext.Bookings.FindAsync(id);

            if (currentReservation != null && currentReservation != null)
            {


                currentReservation.IsDeleted = true;
                //Dbcontext.Reservations.Remove(currentReservation);
                Dbcontext.SaveChanges();

                issuccessdelete = true;
            }
            return issuccessdelete;
        }

        public async Task<EditBooking> GetForEditReservation(int? id, string? Userid)
        {

            IdentityUser? currentUser = await userManager.FindByIdAsync(Userid);
            EditBooking reservation1 = null;

            if (currentUser != null)
            {

                Booking? curentreservation = await Dbcontext.Bookings.Include(r => r.VillaPenthhouse)
                                              .FirstOrDefaultAsync(r => r.IdBooking == id);

                reservation1 = new EditBooking()
                {

                    IdBooking = curentreservation.IdBooking.ToString(),
                    StartDate = curentreservation.StartDate.ToString("yyyy-MM-dd"),

                    EndDate = curentreservation.EndDate.ToString("yyyy-MM-dd"),
                    AdultsCount = curentreservation.AdultsCount,

                    ChildrenCount = curentreservation.ChildrenCount,

                   // RoomId = curentreservation.RoomId.ToString(),
                    VilaId = curentreservation.VillaId.ToString(),
                    VilaName = curentreservation.VillaPenthhouse.NameVilla,
                    GuestFirstName = curentreservation.FirstName,
                    LastNameG = curentreservation.LastName,
                    DateofBirth = curentreservation.DateOfBirth.ToString("yyyy-MM-dd"),
                    GuestAddress = curentreservation.Address,
                    GuestEmail = curentreservation.Email,
                    GuestPhoneNumber = curentreservation.NumberOfPhone
                };

            }


            return reservation1;
        }

        public async Task<bool> EditReservation(string UserId, EditBooking editbooking)
        {
            IdentityUser userid = await userManager.FindByIdAsync(UserId);
            bool resultReservation = false;

            Booking? CurrentReservation = await Dbcontext.Bookings
                                    .FindAsync(int.Parse(editbooking.IdBooking));

           // Room? Room = await Dbcontext.Rooms.FindAsync(int.Parse(editreservation.RoomId));


            if (userid != null && CurrentReservation != null)
            {

                CurrentReservation.StartDate =
                    DateTime.ParseExact(editbooking.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                CurrentReservation.EndDate =
                  DateTime.ParseExact(editbooking.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                CurrentReservation.AdultsCount = editbooking.AdultsCount;
                CurrentReservation.ChildrenCount = editbooking.ChildrenCount;
                CurrentReservation.GuestId = userid.Id;
                //CurrentReservation.RoomId = Room.IdRoom;
                CurrentReservation.IdBooking = int.Parse(editbooking.IdBooking);
                CurrentReservation.FirstName = editbooking.GuestFirstName;
                CurrentReservation.LastName = editbooking.LastNameG;
                CurrentReservation.DateOfBirth = DateTime.ParseExact(editbooking.DateofBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                CurrentReservation.Address = editbooking.GuestAddress;
                CurrentReservation.Email = editbooking.GuestEmail;
                CurrentReservation.NumberOfPhone = editbooking.GuestPhoneNumber;


                this.Dbcontext.SaveChanges();
                resultReservation = true;

            }


            return resultReservation;
        }

        public async Task<DetailsIndexVilla> GetVilaDetailsAsync(int? id, string? UserId)
        {
            IdentityUser? currentUser = await  userManager.FindByIdAsync(UserId);
            DetailsIndexVilla? viladetails = null;
            VillaPenthhouse? CurrentDetailshotel =await  Dbcontext.VillasPenthhouses
                .Include(h => h.Location)
                .Include(h => h.Manager)
                .Include(h=> h.TypePlace)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.IdVilla == id.Value);


            if (CurrentDetailshotel != null)
            {
                viladetails = new DetailsIndexVilla()
                {
                    IdVila = CurrentDetailshotel.IdVilla,
                    VilaName = CurrentDetailshotel.NameVilla,
                  //  Stars = CurrentDetailshotel.Stars,
                   CountChildren= CurrentDetailshotel.CountChildren,
                    CountAdults = CurrentDetailshotel.CountAdults,
                    NumberofRooms = CurrentDetailshotel.CountRooms,
                    Bedrooms= CurrentDetailshotel.Bedrooms,
                    Bathrooms= CurrentDetailshotel.Bathrooms,
                    Parking= CurrentDetailshotel.Parking,
                    ImageUrl = CurrentDetailshotel.ImageUrl,
                    VilaInfo = CurrentDetailshotel.VillaInfo,
                    TownName = CurrentDetailshotel.Location.NameLocation,
                    TypePlace= CurrentDetailshotel.TypePlace.NamePlace,
                    ManagerId = CurrentDetailshotel.IDManager,
                    IsManager = currentUser != null ? currentUser.Id == CurrentDetailshotel.IDManager : false,

                };

            }

            return viladetails;
        }

        public  async Task<IEnumerable<VilaIndexViewModel>> GetAllVillasSearch(string? UserId, string StartDate, string EndDate)
        {

            var startdate = DateTime.ParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var enddate = DateTime.ParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var AllVillasSearch = await Dbcontext.VillasPenthhouses
                .Include(v => v.AllBookings)
                .Where(v => !v.AllBookings.Any(b => b.StartDate >= startdate && b.EndDate <= enddate))
                 .Select(h => new VilaIndexViewModel()
                 {
                     IdVilla = h.IdVilla,
                     NameVilla = h.NameVilla,
                     LocationName = h.Location.NameLocation,
                     NamePlace = h.TypePlace.NamePlace,
                     VillaAddress = h.VillaAddress,
                     ImageUrl = h.ImageUrl,
                     CountAdults = h.CountAdults,
                     CountChildren = h.CountChildren,
                     Bedrooms = h.Bedrooms,
                     Bathrooms = h.Bathrooms,
                     Area = h.Area,
                     Parking = h.Parking
                 }).Distinct().ToListAsync();

            return AllVillasSearch;


        }

        public async Task<bool> LeaveFeedBack(string Userid, BookingFeedbackViewModel leavefeedbackmodel)
        {
            bool operationResult = false;
            IdentityUser? user1 = await this.userManager.FindByIdAsync(Userid);


            if (user1 != null)
            {
                FeedBack feedback = new FeedBack()
                {

                    BookingId = leavefeedbackmodel.BookingId,

                    VillaId = leavefeedbackmodel.VillaId,

                   GuestId = user1.Id,
                   FeedbackMessage = leavefeedbackmodel.FeedbackMessage,    
                   Rating= leavefeedbackmodel.Rating

                  };

                this.Dbcontext.FeedBacks.Add(feedback);
                this.Dbcontext.SaveChanges();

                operationResult = true;
            }

            return operationResult;
        }

        public async Task<IEnumerable<BookingFeedbackViewModel>> GetAllFeedbacks(string? Userid)
        {
            var AllFeedbackfromDB = await Dbcontext.FeedBacks
                .Where(f=> f.GuestId== Userid)
                .Include(f => f.Villa)
                .Select(f => new BookingFeedbackViewModel()
                {

                    BookingId = f.BookingId,
                    VillaId = f.VillaId,
                    VillaName = f.Villa.NameVilla,
                    ClientName = f.Guest.UserName,
                    FeedbackMessage = f.FeedbackMessage,
                    Rating = f.Rating
                }).ToListAsync();

            return AllFeedbackfromDB;
        }
    }
}
