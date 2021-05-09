﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSC.TelegramBotApp.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public int TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Warns { get; set; }
        public bool IsBanned { get; set; }
    }
}