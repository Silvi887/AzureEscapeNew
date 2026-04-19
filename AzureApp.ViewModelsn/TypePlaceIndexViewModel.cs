using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class TypePlaceIndexViewModel
    {
        [Key]
        public int IdTypePlace { get; set; }

        [Required]
        public string NameTypePlace { get; set; } = null!;
    }
}
