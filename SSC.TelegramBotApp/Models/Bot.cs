using SSC.TelegramBotApp.Commands;
using SSC.TelegramBotApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Models
{
    public static class Bot
    {
        private static TelegramBotClient _client;

        private static List<Command> _commands = new List<Command>();

        private static BaseHandler _rootHandler;

        public static void HandleMessage(TelegramBotClient client, Message msg)
        {
            _rootHandler?.Handle(client, msg);
        }

        public static IReadOnlyList<Command> CommandList => _commands.AsReadOnly();

        public static async Task<TelegramBotClient> Get()
        {
            if(_client is null)
            {
                _client = new TelegramBotClient(AppSettings.Key);

                _ConfigureHandlers();

                //_commands.Add(new HelloCommand());
                //_commands.Add(new HowDoYouCommand());
                //_commands.Add(new KickUserFromChatComman());
                //_commands.Add(new WarnUserCommand());

                var hook = string.Format(AppSettings.Url, "api/message/update");
                await _client.SetWebhookAsync(hook);
            }

            return _client;
        }

        private static void _ConfigureHandlers()
        {
            _rootHandler = new TestHandler();
            _rootHandler.SetNext(new WarnMessageHandler());
        }
    }
}