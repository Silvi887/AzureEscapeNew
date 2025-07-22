using Azure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class Amenity
    {
        [Key] 
        public int IdAmenity { get; set; }

        [Required]
        [MaxLength(ValidationConstants.AmenityMaxLenght)]
        [MinLength(ValidationConstants.AmenityMinLenght)]
        public string NameAmenity { get; set; } = null!;
    }
}
