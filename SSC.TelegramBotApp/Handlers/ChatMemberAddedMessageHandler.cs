using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SSC.TelegramBotApp.Handlers
{
    public class ChatMemberAddedMessageHandler : MessageHandler
    {
        private readonly Action<TelegramBotClient, Chat, User, int, DateTime> _ban;

        public ChatMemberAddedMessageHandler(Action<TelegramBotClient, Chat, User, int, DateTime> ban)
        {
            _ban = ban;
        }

        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Type == Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded)
            {
                var mentions = "";
                bool first = true;
                foreach (var user in msg.NewChatMembers)
                {
                    mentions += "[" + user.FirstName + "](tg://user?id=" + user.Id + ")";
                    if (!first)
                        mentions += ", ";
                    else first = false;

                    _ban?.Invoke(client, msg.Chat, user, 0, default);

                    BotDbContext.Get().GetUserInfo(user); // добавить информацию о юзере в базу
                }

                var button = new InlineKeyboardButton();
                button.Text = "Принять";
                button.CallbackData = Bot.ACCEPT_AGREEMENT_CALLBACK;

                var keyboard = new InlineKeyboardMarkup(button);
                string agreement = BotDbContext.Get().Agreements.FirstOrDefault(a => a.ChatId.Equals(msg.Chat.Id))?.Text;   //WebConfigurationManager.AppSettings.Get("MemberAgreement");
                agreement = agreement ?? "Я не в курсе о правилах чата, так что путь пользователь просто нажмет кнопку.";
                
                client.SendTextMessageAsync(msg.Chat.Id, $"{mentions}\n{agreement}",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyMarkup: keyboard);
            }
            else
                base.Handle(client, msg);
        }
    }
}