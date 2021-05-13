using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SSC.TelegramBotApp.Handlers
{
    public class WelcomeNewChatMemberMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg != null && msg.Type == Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded)
            {
                var mentions = "";
                bool first = true;
                foreach (var user in msg.NewChatMembers)
                {
                    mentions += "[" + user.FirstName + "](tg://user?id=" + user.Id + ")";
                    if (!first)
                        mentions += ", ";
                    else first = false;

                    user.BanInChat(client, msg.Chat.Id);

                    BotDbContext.Get().GetUserInfo(user); // добавить информацию о юзере в базу
                }

                var button = new InlineKeyboardButton();
                button.Text = "Принять";
                button.CallbackData = Bot.ACCEPT_AGREEMENT_CALLBACK;

                var keyboard = new InlineKeyboardMarkup(button);
                string agreement = WebConfigurationManager.AppSettings.Get("MemberAgreement");
                client.SendTextMessageAsync(msg.Chat.Id, $"{mentions}\n{agreement}", 
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, 
                    replyMarkup: keyboard);
            }

            base.Handle(client, msg);
        }
    }
}