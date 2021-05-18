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
        public const string ACCEPT_AGREEMENT_CALLBACK = "accept_agreement_calback";


        private static TelegramBotClient _client;

        private static UpdateHandler _rootUpdateHandler;
        private static MessageHandler _rootMessageHandler;

        public static void HandleUpdate(TelegramBotClient client, Update update)
        {
            try
            {
                if (update.Type == UpdateType.Message && update.Message != null)
                    _rootMessageHandler?.Handle(client, update.Message);
                else _rootUpdateHandler?.Handle(client, update);
            }
            catch
            {

            }
            
        }

        public static void HandleUpdateAsync(TelegramBotClient client, Update update)
        {
            Task.Factory.StartNew(() => HandleUpdate(client, update));
        }

        public static async Task<TelegramBotClient> Get()
        {
            if(_client is null)
            {
                _client = new TelegramBotClient(AppSettings.Key);

                _ConfigureHandlers();

                var hook = string.Format(AppSettings.Url, "api/message/update");
                await _client.SetWebhookAsync(hook, allowedUpdates: 
                    new[] {
                        UpdateType.Message,
                        UpdateType.ChatMember,
                        UpdateType.InlineQuery,
                        UpdateType.CallbackQuery,
                        UpdateType.ChannelPost, 
                        UpdateType.ChosenInlineResult, 
                        UpdateType.EditedChannelPost, 
                        UpdateType.EditedMessage,
                        UpdateType.Poll,
                        UpdateType.ShippingQuery,
                        UpdateType.MyChatMember,
                        UpdateType.PreCheckoutQuery,
                        UpdateType.ShippingQuery,
                        UpdateType.Unknown
                    });
            }

            return _client;
        }

        private static void _ConfigureHandlers()
        {
            _rootMessageHandler = new WelcomeNewChatMemberMessageHandler();
            _rootMessageHandler
                .SetNext(new AdminMessageHandler())
                .SetNext(new TestMessageHandler())
                .SetNext(new WarnMessageHandler())
                .SetNext(new BanUserMessageHandler())
                .SetNext(new UnbanMemberMessageHandler())
                .SetNext(new UpdateChatAgreementMessageHandler());

            _rootUpdateHandler = new AgreementAcceptUpdateHandler();
            _rootUpdateHandler.SetNext(new MemberUnbannedUpdateHandler());
        }
    }
}