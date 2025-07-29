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
    public class UserVilla
    {
        [Required]
        public int VillaId { get; set; }
        [ForeignKey(nameof(VillaId))]
        public VillaPenthhouse Villa { get; set; } = null!;


        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }= null!;
    }
}
