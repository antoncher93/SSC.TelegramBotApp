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
                foreach(var member in msg.NewChatMembers)
                {
                    mentions += "[" + member.FirstName + "](tg://user?id=" + member.Id + ")";
                    if (!first)
                        mentions += ", ";
                    else first = false;

                    member.BanInChat(client, msg.Chat.Id);
                }

                var button = new InlineKeyboardButton();
                button.Text = "Принять";
                button.CallbackData = Bot.ACCEPT_AGREEMENT_CALLBACK;

                var keyboard = new InlineKeyboardMarkup(button);

                //client.OnCallbackQuery += (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                //{
                //    if (ev.CallbackQuery.Data.Equals("accept_agreement_calback"))
                //    {
                //        var user = ev.CallbackQuery.From;
                //        user.Unban(client, msg.Chat.Id);
                //    }
                //};

                string agreement = WebConfigurationManager.AppSettings.Get("MemberAgreement");
                client.SendTextMessageAsync(msg.Chat.Id, $"{mentions}\n{agreement}", 
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, 
                    replyMarkup: keyboard);
            }

            base.Handle(client, msg);
        }
    }
}