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
            else if(msg != null && msg.Text != null && msg.Text.IndexOf("/welcome")>=0)
            {
                if (msg.Entities is null)
                {
                    var mentions = "";

                    for (int i = 1; i < msg.Entities.Length; i++)
                    {
                        if (msg.Entities[i].Type != Telegram.Bot.Types.Enums.MessageEntityType.Mention)
                            continue;

                        var user = msg.Entities[i].User;
                        if(user == null)
                        {
                            var username = msg.EntityValues.ElementAt(i).Replace("@", "");
                            var userInfo = BotDbContext.Get().UserInfoes.FirstOrDefault(u => u.Username.Equals(username));
                            if (userInfo != null)
                            {
                                var chatMember = client.GetChatMemberAsync(msg.Chat.Id, userInfo.TelegramId).Result;
                                user = chatMember.User;
                            }
                            else continue;
                        }

                        if (i> 0)
                            mentions += ", ";
                        mentions += "[" + user.FirstName + "](tg://user?id=" + user.Id + ")";

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
                else if(msg.ReplyToMessage != null)
                {
                    var user = msg.ReplyToMessage.From;

                    user.BanInChat(client, msg.Chat.Id);

                    BotDbContext.Get().GetUserInfo(user);

                    var button = new InlineKeyboardButton();
                    button.Text = "Принять";
                    button.CallbackData = Bot.ACCEPT_AGREEMENT_CALLBACK;

                    var keyboard = new InlineKeyboardMarkup(button);
                    string agreement = WebConfigurationManager.AppSettings.Get("MemberAgreement");
                    client.SendTextMessageAsync(msg.Chat.Id, $"{user.GetMension()}\n{agreement}",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyMarkup: keyboard);
                }
                else
                {
                    client.SendTextMessageAsync(msg.Chat.Id, "Для выполнения команды необходимо указать пользователей чата.", replyToMessageId: msg.MessageId);
                    return;
                }
            }
            else
                base.Handle(client, msg);
        }
    }
}