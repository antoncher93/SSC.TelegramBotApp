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
    public class WelcomeNewChatMemberHandler : BaseHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Type == Telegram.Bot.Types.Enums.MessageType.ChatMembersAdded)
            {
                var mentions = "";
                bool first = true;
                foreach(var member in msg.NewChatMembers)
                {
                    mentions += "[" + member.FirstName + "](tg://user?id=" + member.Id + ")";
                    if (!first)
                        mentions += ", ";
                    else first = false;

                    var permissions = new ChatPermissions()
                    {
                        CanAddWebPagePreviews = false,
                        CanChangeInfo = false,
                        CanInviteUsers = false,
                        CanPinMessages = false,
                        CanSendMediaMessages = false,
                        CanSendMessages = false,
                        CanSendOtherMessages = false,
                        CanSendPolls = false
                    };
                    var date = DateTime.Now + TimeSpan.FromDays(1);
                    client.RestrictChatMemberAsync(msg.Chat.Id, member.Id, permissions, date);
                }

                var button = new InlineKeyboardButton();
                button.Text = "Принять";
                button.CallbackData = "accept_agreement_calback";

                var keyboard = new InlineKeyboardMarkup(button);

                client.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
                {
                    if (ev.CallbackQuery.Data.Equals("accept_agreement_calback"))
                    {
                        var member = ev.CallbackQuery.From;
                        await client.PromoteChatMemberAsync(msg.Chat.Id, member.Id, canPostMessages: true);
                    }
                };

                string agreement = WebConfigurationManager.AppSettings.Get("MemberAgreement");
                client.SendTextMessageAsync(msg.Chat.Id, $"{mentions}\n{agreement}", 
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, 
                    replyMarkup: keyboard);
            }

            base.Handle(client, msg);
        }
    }
}