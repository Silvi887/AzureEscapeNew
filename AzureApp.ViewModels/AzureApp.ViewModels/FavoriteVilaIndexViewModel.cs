using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.ViewModels
{
    public class FavoriteVilaIndexViewModel
    {
        [Key]
        public int IdVila { get; set; }

        [Required]
        public string VilaName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        [Required]
        public string TownName { get; set; } = null!;
    }
}
