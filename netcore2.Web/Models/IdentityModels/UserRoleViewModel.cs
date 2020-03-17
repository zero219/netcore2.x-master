using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            Users = new List<ApplicationUser>();
        }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public List<ApplicationUser> Users { get; set; }

    }
}
