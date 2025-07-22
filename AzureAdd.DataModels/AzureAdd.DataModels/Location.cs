using Azure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class Location
    {
        [Key]
        public int IdLocation { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string NameLocation { get; set; } = null!;

        public virtual ICollection<VillaPenthhouse> VillasPenthhouses { get; set; } = new HashSet<VillaPenthhouse>();

    }
}
