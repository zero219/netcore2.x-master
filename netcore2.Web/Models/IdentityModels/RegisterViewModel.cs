using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IdentityModels
{
    public class RegisterViewModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [Compare("PassWord")]
        public string ConfirmPassWord { get; set; }

    }
}
