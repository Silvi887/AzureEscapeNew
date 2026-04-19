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

        [Required(ErrorMessage ="You have to fill Villa Name")]
        [MaxLength(ValidationConstants.VillaMaxLenght)]
        [MinLength(ValidationConstants.VillaMinLenght)]
        public string NameVilla { get; set; } = null!;

        [Required]

        public string NamePlace { get; set; } = null!;

        [Required (ErrorMessage ="The info of villa is required to be filled!")]
        [MaxLength(ValidationConstants.DescriptionMaxLenght)]
        [MinLength(ValidationConstants.DescriptionMinLenght)]
        public string VillaInfo { get; set; } = null!;

        [Required(ErrorMessage = "You have to fill Address of Villa!")]
        [MaxLength(ValidationConstants.VillaAdressMaxLenght)]
        [MinLength(ValidationConstants.VillaAdressMinLenght)]
        public string VillaAddress { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public int CountRooms { get; set; }

        [Range(1,20,ErrorMessage = "You have to fill count of Adults bigger than 0!")]
        public int CountAdults { get; set; }

        [Required]
        public int CountChildren { get; set; }

        [Range(1, 20, ErrorMessage = "You have to fill count of Bedrooms !")]
        public int Bedrooms { get; set; } = 1;

        [Range(1, 20, ErrorMessage = "You have to fill count of Bathrooms!")]
        public int Bathrooms { get; set; } = 1;

        [Required(ErrorMessage = "You have to fill Area of Villa in range 3-80m3")]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Area { get; set; } = "";

        [Required(ErrorMessage ="Please fill Parking info!")]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Parking { get; set; } = "";


        public string LocationName { get; set; } = "";

        [Range(1,int.MaxValue,ErrorMessage ="Please select a town!")]
        public int IdTown { get; set; }

        [Range(1,int.MaxValue,ErrorMessage = "Please select Type of Place!")]
        public int IdTypePlace { get; set; }
        public IEnumerable<TownIndexViewModel> AllTownsModels { get; set; } = null!;

        public IEnumerable<TypePlaceIndexViewModel> AllTypePlaces { get; set; } = null!;
    }
}
