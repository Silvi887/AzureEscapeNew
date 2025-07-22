using Azure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdd.DataModels
{
    public class TypePlace
    {
        [Key]
        public int IdTypePlace { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PlaceMaxLenght)]
        [MinLength(ValidationConstants.PlaceMinLenght)]
        public string NamePlace { get; set; } = null!;

        public virtual ICollection<VillaPenthhouse> VillasPenthhouses { get; set; } = new HashSet<VillaPenthhouse>();
    }
}
