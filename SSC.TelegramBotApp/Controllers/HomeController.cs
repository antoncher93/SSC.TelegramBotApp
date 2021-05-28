using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSC.TelegramBotApp.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "This is Admin Telegram Bot (Betta)!";
        }

        public string About()
        {
            var status = "Unknown";
#if DEBUG
            status = "Debug";
#else
            status = "release";
#endif
            return $"SSC Bot Version 1.17. Status: {status}";
        }

        
    }
}