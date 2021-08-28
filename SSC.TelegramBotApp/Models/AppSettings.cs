using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Models
{
    public static class AppSettings
    {
#if DEBUG
        public static string Url { get; set; } = @"https://1afa-109-252-75-4.ngrok.io/{0}";
        //public static string Url { get; set; } = @"https://9b00004ad418.ngrok.io/{0}";
        //public static string Url { get; set; } = @"https://ssctelegrambotapp20210506235851.azurewebsites.net/{0}";
#else
        public static string Url { get; set; } = @"https://1afa-109-252-75-4.ngrok.io/{0}";
#endif
        public static string Name { get; set; } = "anton_cher93_bot";
        public static string Key { get; set; } = "1728956369:AAFDn4tD1ZFiCO1INKqO9dGqeVmhV1Ldj9g";
    }
}