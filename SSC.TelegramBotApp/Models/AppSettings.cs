using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Models
{
    public static class AppSettings
    {
#if DEBUG
        public static string Url { get; set; } = @"https://95fdbd215693.ngrok.io/{0}";
#else
        public static string Url { get; set; } = @"https://ssctelegrambot.azurewebsites.net/{0}";
#endif
        public static string Name { get; set; } = "anton_cher93_bot";
        public static string Key { get; set; } = "1728956369:AAFDn4tD1ZFiCO1INKqO9dGqeVmhV1Ldj9g";
    }
}