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
            return "This is my first f*cking telegram bot!";
        }

        public string About()
        {
            return "SSC Bot Version 1.10";
        }

        
    }
}