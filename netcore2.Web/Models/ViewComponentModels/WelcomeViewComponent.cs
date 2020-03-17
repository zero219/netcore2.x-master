using Microsoft.AspNetCore.Mvc;
using netcore2.Web.Models.IocModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.ViewComponentModels
{
    public class WelcomeViewComponent : ViewComponent
    {
        private readonly IWelcomeService _welcomeService;
        public WelcomeViewComponent(IWelcomeService welcomeService)
        {
            this._welcomeService = welcomeService;
        }
        public IViewComponentResult Invoke(string str)
        {
            var welcome = _welcomeService.GetMessage() + ":" + str;
            return View("Default", welcome);
        }
    }
}
