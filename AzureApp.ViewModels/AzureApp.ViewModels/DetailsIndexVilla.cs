using Azure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class DetailsIndexVilla
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVila { get; set; }


        [Required]
        public string VilaName { get; set; } = null!;

        //[Required]
        //public int Stars { get; set; }


        [Required]
        public int NumberofRooms { get; set; }


        public string? ImageUrl { get; set; }

        [Required]
        public int CountAdults { get; set; }

        [Required]
        public int CountChildren { get; set; }

        [Required]
        public int Bedrooms { get; set; } = 1;

        [Required]
        public int Bathrooms { get; set; } = 1;


        [Required]
        public string VilaInfo { get; set; } = null!;

        [Required]
        public string GuestId { get; set; } = null!;

        [Required]
        public string ManagerId { get; set; } = null!;


        public bool IsManager { get; set; }


        [Required]
        public string TownName { get; set; }

        [Required]
        public string TypePlace { get; set; }


        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Parking { get; set; } = "";

        public bool IsSaved { get; set; }
    }
}
