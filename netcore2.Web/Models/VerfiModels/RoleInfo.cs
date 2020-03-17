using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.VerfiModels
{
    /// <summary>
    /// 自定义验证
    /// </summary>
    public class RoleInfo
    {
        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "{0}不能为空")]
        //CheckRoleExist：action名称，Home控制器名称
        [Remote("CheckRoleExist", "Home", ErrorMessage = "角色已存在")]
        public string RoleName { get; set; }
    }
}
