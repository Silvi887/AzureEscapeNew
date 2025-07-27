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
    public class TownService : ITownService
    {


        private readonly AzureAddDbContext Dbcontext;
      //  private readonly UserManager<IdentityUser> UserManager;

        public TownService(AzureAddDbContext pDbcontext)
        {
            this.Dbcontext = pDbcontext;
          //  this.UserManager = usermanager;
        }
        public async Task<IEnumerable<TownIndexViewModel>> TownViewDataAsync()
        {
            IEnumerable<TownIndexViewModel> allTowns = await Dbcontext.Locations
                .AsNoTracking()
                .Select(t => new TownIndexViewModel()
                {
                    IdTown = t.IdLocation,
                    NameTown = t.NameLocation
                }
                ).ToListAsync();

           return allTowns;
        }

        public async Task<IEnumerable<TypePlaceIndexViewModel>> TypePlaceViewDataAsync()
        {

            IEnumerable<TypePlaceIndexViewModel> allPlaces= await Dbcontext.TypePlaces
                .AsNoTracking()
                .Select(t => new TypePlaceIndexViewModel()
                {
                    IdTypePlace = t.IdTypePlace,
                    NameTypePlace = t.NamePlace
                }
                ).ToListAsync();

            return allPlaces;
        }
    }
}
