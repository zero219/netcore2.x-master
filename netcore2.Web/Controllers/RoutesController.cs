using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace netcore2.Web.Controllers
{
    //自定义路由(做api适用)
    [Route("api/[controller]/[action]")]
    public class RoutesController : Controller
    {
        public IActionResult Index()
        {
            return Json("Hello Word!!!!");
        }
        public IActionResult Me()
        {
            return Json("Me");
        }
    }
}