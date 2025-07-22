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
    public class VillaService:IVilla
    {

        private readonly AzureAddDbContext Dbcontext;
        private readonly UserManager<IdentityUser> userManager;

        public VillaService(AzureAddDbContext pDbcontext, UserManager<IdentityUser> usermanager)
        {
            this.Dbcontext = pDbcontext;
            this.userManager = usermanager;
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
