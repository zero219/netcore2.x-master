using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Data
{
    public static class ClaimTypes
    {
        public static List<string> AllClaimTypeList { get; set; } = new List<string>
        {
            "About",
            "Contact",
            "用户管理",
            "角色管理",

        };
    }
}
