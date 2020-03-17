using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.IocModels
{
    public class WelcomeService : IWelcomeService
    {
        public string GetMessage()
        {
            return "Hello Word";
        }
    }
}
