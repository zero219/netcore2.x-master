using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels
{
    public class ManageClaimsViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ClaimId { get; set; }

        public List<string> AvailableClaims { get; set; }
    }
}
