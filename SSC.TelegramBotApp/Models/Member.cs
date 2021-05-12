using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Models
{
    public class Member
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public int Warns { get; set; }
    }
}