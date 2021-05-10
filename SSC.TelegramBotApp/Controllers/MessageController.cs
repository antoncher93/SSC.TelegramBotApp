using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Controllers
{
    public class MessageController : ApiController
    {
        [Route(@"api/message/update")]
        public async Task<OkResult> Update([FromBody]Update update)
        {
            var client = await Bot.Get();
            Bot.HandleUpdate(client, update);
            return Ok();
        }

        [Route(@"api/message/test")]
        public string Test(int id)
        {
            return "Message Test id = " + id;
        }

        public string Print(int id)
        {
            return $"id = {id}";
        }
    }
}
