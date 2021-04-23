using SSC.TelegramBotApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;

namespace SSC.TelegramBotApp.Models
{
    public static class Bot
    {
        private static TelegramBotClient _client;

        private static List<Command> _commands = new List<Command>();

        public static IReadOnlyList<Command> CommandList => _commands.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            if(_client is null)
            {
                _client = new TelegramBotClient(AppSettings.Key);
                _commands.Add(new HelloCommand());
                _commands.Add(new HowDoYouCommand());

                var hook = string.Format(AppSettings.Url, "api/message/update");

                await _client.SetWebhookAsync(hook);

                //var offset = 0;

                //while(true)
                //{
                //    var updates = await _client.GetUpdatesAsync(offset);

                //    foreach(var update in updates)
                //    {
                //        var message = update.Message;
                //        foreach(var command in CommandList)
                //        {
                //            if (command.Contains(message.Text))
                //            {
                //                command.Execute(_client, message);
                //                break;
                //            }
                //        }
                        
                //    }
                //}
            }

            return _client;
        }
    }
}