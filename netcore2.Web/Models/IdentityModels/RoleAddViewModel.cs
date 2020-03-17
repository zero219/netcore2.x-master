using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage ="{0}不能为空")]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }
    }
}
