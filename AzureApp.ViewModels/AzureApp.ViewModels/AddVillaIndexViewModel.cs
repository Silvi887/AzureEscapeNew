using Azure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class AddVillaIndexViewModel
    {

        [Key]

        public int IdVilla { get; set; }

        [Required]
        [MaxLength(ValidationConstants.VillaMaxLenght)]
        [MinLength(ValidationConstants.VillaMinLenght)]
        public string NameVilla { get; set; } = null!;

        [Required]

        public string NamePlace { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLenght)]
        [MinLength(ValidationConstants.DescriptionMinLenght)]
        public string VillaInfo { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.VillaAdressMaxLenght)]
        [MinLength(ValidationConstants.VillaAdressMinLenght)]
        public string VillaAddress { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int CountRooms { get; set; }

        [Required]
        public int CountAdults { get; set; }

        [Required]
        public int CountChildren { get; set; }

        [Required]
        public int Bedrooms { get; set; } = 1;

        [Required]
        public int Bathrooms { get; set; } = 1;

        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Area { get; set; } = "";

        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Parking { get; set; } = "";


        public string LocationName { get; set; } = "";

        public int IdTown { get; set; }
        public int IdTypePlace { get; set; }
        public IEnumerable<TownIndexViewModel> AllTownsModels { get; set; } = null!;

        public IEnumerable<TypePlaceIndexViewModel> AllTypePlaces { get; set; } = null!;
    }
}
