using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.VerfiModels
{
    public class User
    {
        [Display(Name = "名称"), Required(ErrorMessage = "{0}不能为空"), MaxLength(10, ErrorMessage = "长度不超过10个")]
        public string UserName { get; set; }

        [Display(Name = "年龄")]
        [Required(ErrorMessage = "{0}不能为空")]
        public int? UserAge { get; set; }

        [Display(Name = "身份证")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "{0}长度是{1}")]
        public string IDCard { get; set; }

        [Display(Name = "邮箱地址")]
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "Url")]
        [Required(ErrorMessage = "{0}不能为空")]
        [ValidatorUrl(ErrorMessage = "这个Url不正确")]//自定义验证
        public string Url { get; set; }

        [Display(Name = "出生日期")]
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Date)]
        public DateTime? BirtheTime { get; set; }

        [Display(Name = "图片")]
        [Required(ErrorMessage = "{0}不能为空")]
        public List<IFormFile> Photo { get; set; }

        public RoleInfo roleInfo { get; set; }

        [Display(Name = "性别")]
        public Gender gender { get; set; }
    }

    public enum Gender
    {
        男 = 0,
        女 = 1
    }

}
