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

        public static void HandleUpdate(TelegramBotClient client, Update update)
        {
            if(update.Message != null)
                _rootHandler?.Handle(client, update.Message);
        }

        public static async Task<TelegramBotClient> Get()
        {
            if(_client is null)
            {
                _client = new TelegramBotClient(AppSettings.Key);

                _ConfigureHandlers();

                var hook = string.Format(AppSettings.Url, "api/message/update");
                await _client.SetWebhookAsync(hook);
            }

            return _client;
        }

        private static void _ConfigureHandlers()
        {
            _rootHandler = new TestHandler();
            _rootHandler.SetNext(new WarnMessageHandler())
                .SetNext(new WarnUserHandler())
                .SetNext(new WelcomeNewChatMemberHandler())
                .SetNext(new UpdateMemberAgreementHandler())
                .SetNext(new RestrictUserHandler());
        }
    }
}