using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Models
{
    public class Agreement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public long ChatId { get; set; }
    }
}