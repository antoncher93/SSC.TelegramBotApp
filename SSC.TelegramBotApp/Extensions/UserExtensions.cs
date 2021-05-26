using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Extensions
{
    public static class UserExtensions
    {
        public static string GetMension(this User member)
        {
            return "[" + member.FirstName + "](tg://user?id=" + member.Id + ")";
        }
    }
}