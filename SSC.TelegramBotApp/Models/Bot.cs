using SSC.TelegramBotApp.Commands;
using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SSC.TelegramBotApp.Models
{
    public static class Bot
    {
        public const string ACCEPT_AGREEMENT_CALLBACK = "accept_agreement_calback";


        private static TelegramBotClient _client;

        private static UpdateHandler _rootUpdateHandler;
        private static MessageHandler _rootMessageHandler;
        private static List<Commands.AdminCommand> _commandList = new List<Commands.AdminCommand>();
        public static IReadOnlyCollection<SSC.TelegramBotApp.Commands.AdminCommand> CommandList => _commandList.AsReadOnly();

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

        public static void HandleUpdate2(TelegramBotClient client, Update update)
        {
            try
            {
                if (update.Type == UpdateType.Message && update.Message != null && update.Message.Type == MessageType.Text)
                {
                    var msg = update.Message;
                    if(msg.Entities.Any(e => e.Type == MessageEntityType.BotCommand))
                    {
                        for (int i = 0; i < update.Message.Entities.Length; i++)
                        {
                            var entity = update.Message.Entities[i];
                            if (entity.Type == MessageEntityType.BotCommand)
                            {
                                var commandName = update.Message.EntityValues.ElementAt(i);
                                var command = CommandList.FirstOrDefault(c => commandName.Contains(c.Name));
                                command?.Execute(client, update.Message);
                            }
                        }
                    }
                }
                else if(update.Type == UpdateType.Message && update.Message != null)
                {
                    _rootMessageHandler?.Handle(client, update.Message);
                }
                else _rootUpdateHandler?.Handle(client, update);
            }
            catch(Exception exc)
            {

            }

        }

        public static void HandleUpdateAsync(TelegramBotClient client, Update update)
        {
            Task.Factory.StartNew(() => HandleUpdate2(client, update));
        }

        public static async Task<TelegramBotClient> Get()
        {
            if(_client is null)
            {
                _client = new TelegramBotClient(AppSettings.Key);

                _ConfigureHandlers();
                _ConfigureBotCommands();

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
            _rootMessageHandler = new ChatMemberAddedMessageHandler(_BanInChat);

            _rootUpdateHandler = new AgreementAcceptUpdateHandler(_Unban);
            _rootUpdateHandler.SetNext(new MemberUnbannedUpdateHandler());
        }

        private static void _ConfigureBotCommands()
        {
            _commandList.Add(new TestCommand());
            _commandList.Add(new SetAgreementCommand());
            _commandList.Add(new WarnCommand(_BanInChat));
            _commandList.Add(new BanCommand(_BanInChat));
            _commandList.Add(new UnbanCommand((client, chat, user, msgId) => _Unban(client, chat, user)));
            _commandList.Add(new MemberBotCommand("/welcome", (client, chat, user, msgId) => _WelcomeChatMember(client, chat, user)));
        }


        private static void _WelcomeChatMember(TelegramBotClient client, Chat chat, User user)
        {
            _BanInChat(user, client, chat.Id);

            BotDbContext.Get().GetUserInfo(user); // добавить информацию о юзере в базу

            var button = new InlineKeyboardButton();
            button.Text = "Принимаю";
            button.CallbackData = Bot.ACCEPT_AGREEMENT_CALLBACK;

            var keyboard = new InlineKeyboardMarkup(button);
            string agreement = BotDbContext.Get().Agreements.FirstOrDefault(a => a.ChatId.Equals(chat.Id))?.Text; //WebConfigurationManager.AppSettings.Get("MemberAgreement");
            agreement = agreement ?? "Я не в курсе о правилах чата, так что просто нажми кнопку.";
            client.SendTextMessageAsync(chat.Id, $"{user.GetMension()}\n{agreement}",
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }

        private static void _BanInChat(this User user, TelegramBotClient client, long chatId, int replyMessageId = 0,
            DateTime untilDate = default)
        {
            var permissions = new ChatPermissions()
            {
                CanSendMediaMessages = false,
                CanSendMessages = false,
                CanSendOtherMessages = false
            };

            client.RestrictChatMemberAsync(chatId, user.Id, permissions, untilDate);

            if (replyMessageId > 0)
                client.SendTextMessageAsync(chatId, $"{user.GetMension()} будет забанен!",
                    replyToMessageId: replyMessageId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

            var member = BotDbContext.Get().GetMember(chatId, user.Id);
            member.Warns = 0;
            BotDbContext.Get().Entry(member).State = System.Data.Entity.EntityState.Modified;
            BotDbContext.Get().SaveChanges();
        }

        private static void _Unban(TelegramBotClient client, Chat chat, User user)
        {
            var permissions = new ChatPermissions()
            {
                CanAddWebPagePreviews = true,
                CanChangeInfo = true,
                CanInviteUsers = true,
                CanPinMessages = true,
                CanSendMediaMessages = true,
                CanSendMessages = true,
                CanSendOtherMessages = true,
                CanSendPolls = true
            };

            client.RestrictChatMemberAsync(chat.Id, user.Id, permissions);
        }

        private static void _BanInChat(TelegramBotClient client, Chat chat, User user, int replyMessageId = 0,
            DateTime untilDate = default)
        {
            var permissions = new ChatPermissions()
            {
                CanSendMediaMessages = false,
                CanSendMessages = false,
                CanSendOtherMessages = false
            };

            client.RestrictChatMemberAsync(chat.Id, user.Id, permissions, untilDate);

            if (replyMessageId > 0)
                client.SendTextMessageAsync(chat.Id, $"{user.GetMension()} будет забанен!",
                    replyToMessageId: replyMessageId, parseMode: ParseMode.Markdown);

            var member = BotDbContext.Get().GetMember(chat.Id, user.Id);
            member.Warns = 0;
            BotDbContext.Get().Entry(member).State = System.Data.Entity.EntityState.Modified;
            BotDbContext.Get().SaveChanges();
        }
    }
}