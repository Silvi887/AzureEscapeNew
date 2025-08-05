
using Azure.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class VillaPenthhouse
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVilla { get; set; }


        [Required]
        [MaxLength(ValidationConstants.VillaMaxLenght)]
        [MinLength(ValidationConstants.VillaMinLenght)]
        public string NameVilla { get; set; } = null!;

        [Required]
        public int IdPlace { get; set; }

        [ForeignKey(nameof(IdPlace))]
        public virtual TypePlace TypePlace { get; set; } = null!;

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
        public string Area { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string Parking { get; set; } =null!;

        [Required]
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; } = null!;


        [Required]
        public string IDManager { get; set; } = null!;

        [ForeignKey(nameof(IDManager))]
        public virtual IdentityUser Manager { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Booking> AllBookings { get; set; } = new HashSet<Booking>();

        public virtual ICollection<UserVilla> UserVillas { get; set; } = new HashSet<UserVilla>();

        public virtual ICollection<FeedBack> Feedbacks { get; set; } = new HashSet<FeedBack>();
    }
}
