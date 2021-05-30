using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using netcore2.Web.Models;
using netcore2.Web.Models.CacheModels;
using netcore2.Web.Models.IocModels;
using netcore2.Web.Models.VerfiModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace netcore2.Web.Controllers
{

    public class HomeController : Controller
    {
        private IWelcomeService _welcomeService;
        /// <summary>
        /// xss
        /// </summary>
        private readonly HtmlEncoder _htmlEncoder;
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 缓存
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IWelcomeService welcomeService, HtmlEncoder htmlEncoder, ILogger<HomeController> logger, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            this._welcomeService = welcomeService;
            this._htmlEncoder = htmlEncoder;
            this._logger = logger;
            this._memoryCache = memoryCache;
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrators")]//角色
        [Authorize(Policy = "仅限About")]
        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "Administrators")]//角色
        [Authorize(Policy = "仅限About")]
        [HttpPost]
        [ValidateAntiForgeryToken]//单个防止CSRF
        [IgnoreAntiforgeryToken]//不防止CSRF
        public IActionResult About(User user)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = string.Empty;
                //多张或一张图片处理
                if (user.Photo != null && user.Photo.Count > 0)
                {
                    foreach (var photo in user.Photo)
                    {
                        string uploadFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadFilePath, uniqueFileName);
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }

                }

            }
            return View(user);
        }


        [Authorize(Policy = "仅限管理员")]
        [Authorize(Policy = "仅限联系人")]
        [Authorize(Policy = "自定义的权限")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "联系人页。";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/HttpStausCode/{stausCode}")]
        public IActionResult HttpStausCode(int stausCode)
        {
            var stausCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (stausCode)
            {
                case 404:
                    ViewBag.Message = "抱歉，您访问的页面不存在";
                    //ViewBag.Path = stausCodeResult.OriginalPath;
                    //ViewBag.QueryString = stausCodeResult.OriginalQueryString;
                    //ViewBag.PathBase = stausCodeResult.OriginalPathBase;
                    break;
            }
            //绝对路径“~/Views/Shared/NotFound.cshtml”
            //相对路径“../../Views/Shared/NotFound”(..代表上面有几个文件夹)
            return View("~/Views/Shared/NotFound.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //ViewBag.ExcetionPath = exception.Path;//异常路径
            //ViewBag.ExcetionMessage = exception.Error.Message;//异常内容
            //ViewBag.ExcetionStackTrace = exception.Error.StackTrace;//异常堆栈
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 读取appsettings.json中数据
        /// </summary>
        /// <returns></returns>
        public IActionResult Config()
        {
            #region 读取appsettings.json中数据
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //获取根目录
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //构建configurationBuilder对象
            var configuration = configurationBuilder.Build();
            //获取字符串
            var conectString = configuration.GetConnectionString("DefaultConnection");

            var AllowedHosts = configuration["AllowedHosts"];
            //如果有json中数组的话就添加0索引
            var Default = configuration["Logging:Console:LogLevel:Default"];
            #endregion
            return Content(Default);
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <returns></returns>
        public IActionResult Ioc()
        {
            var welcome = _welcomeService.GetMessage();
            return Json(welcome);
        }

        /// <summary>
        /// XSS
        /// </summary>
        /// <returns></returns>

        public IActionResult Xss()
        {
            string str = _htmlEncoder.Encode("<script>我是弹窗</script>");
            return Json(str);
        }

        /// <summary>
        /// 自定义验证
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckRoleExist(RoleInfo roleInfo)
        {
            if (roleInfo.RoleName == "Admin")
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <returns></returns>
        public IActionResult LoggerData()
        {
            //NLog日志级别
            _logger.LogTrace("Trace(跟踪)");
            _logger.LogDebug("Debug(调试)");
            _logger.LogInformation("Information(信息)");
            _logger.LogWarning("Waring(警告)");
            _logger.LogError("Error(错误)");
            _logger.LogCritical("Critical(严重)");
            return Json("");
        }
        /// <summary>
        /// 缓存(Razor中使用缓存<cache>缓存的内容</cache>)
        /// </summary>
        /// <returns></returns>
        public IActionResult CacheData()
        {
            if (!_memoryCache.TryGetValue(CacheEntryConstants.AlbumsOfToday, out int num))
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    num += 1;
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                //.SetAbsoluteExpiration(TimeSpan.FromSeconds(30))//绝对过期时间
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));//相对过期时间
                _memoryCache.Set(CacheEntryConstants.AlbumsOfToday, num, cacheEntryOptions);
            }
            return Json(num);
        }
    }
}
