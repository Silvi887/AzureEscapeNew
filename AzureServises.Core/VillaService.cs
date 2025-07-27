using Azure.Constants;
using AzureAdd.Data;
using AzureAdd.DataModels;
using AzureApp.ViewModels;
using AzureServises.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            throw new NotImplementedException();
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

            var Allhotels = await Dbcontext.VillasPenthhouses.Where(v => v.IsDeleted == false)
                .Select(h => new VilaIndexViewModel()
                {
                    IdVilla = h.IdVilla,
                    NameVilla = h.NameVilla,
                    LocationName = h.Location.NameLocation,
                    NamePlace = h.Location.NameLocation,
                    VillaAddress = h.VillaAddress,
                    ImageUrl = h.ImageUrl,
                    CountAdults = h.CountAdults,
                    CountChildren = h.CountChildren,
                    Bedrooms = h.Bedrooms,
                    Bathrooms = h.Bathrooms,
                    Area = h.Area,
                    Parking = h.Parking
                }).ToListAsync();

            return Allhotels;
        }


        //public async Task<IEnumerable<VilaIndexViewModel>> GetAllVillasAsync(string? UserId)
        //{


        //    var Allhotels = await Dbcontext.VillasPenthhouses.Where(v => v.IsDeleted == false)
        //        .Select(h => new VilaIndexViewModel()
        //        {
        //            IdVilla=h.IdVilla,
        //            NameVilla= h.NameVilla,
        //            LocationName=h.Location.NameLocation,
        //            NamePlace= h.Location.NameLocation,
        //            VillaAddress=h.VillaAddress,
        //            ImageUrl=h.ImageUrl,
        //            CountAdults=h.CountAdults,
        //            CountChildren=h.CountChildren,
        //            Bedrooms=h.Bedrooms,
        //            Bathrooms=h.Bathrooms,
        //            Area=h.Area,
        //            Parking=h.Parking
        //        }).ToListAsync();

        //    return Allhotels;
        //}
    }
}
