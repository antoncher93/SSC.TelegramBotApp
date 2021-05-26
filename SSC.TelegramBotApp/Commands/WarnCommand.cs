using SSC.TelegramBotApp.Extensions;
using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SSC.TelegramBotApp.Commands
{
    public class WarnCommand : AdminCommand
    {
        public override string Name => "/warn";
        private readonly Action<TelegramBotClient, Chat, User, int, DateTime> _ban;

        public WarnCommand(Action<TelegramBotClient, Chat, User,int, DateTime> ban)
        {
            _ban = ban;
        }


        public override void Execute(TelegramBotClient client, Message msg)
        {
            int replyMsgId = msg.MessageId;
            User user = null;

            if (msg.ReplyToMessage != null)
            {
                user = msg.ReplyToMessage.From;
                replyMsgId = msg.ReplyToMessage.MessageId;
            }
            else if (msg.Entities.Any(e => e.Type == MessageEntityType.Mention))
            {
                for(int i = 0; i<msg.Entities.Length; i++)
                {
                    var entity = msg.Entities[i];
                    if (entity.Type != MessageEntityType.Mention)
                        continue;

                    user = entity.User;
                    if (user is null)
                    {
                        var username = msg.EntityValues.ElementAt(i).Replace("@", "");
                        var userInfo = BotDbContext.Get().UserInfoes.FirstOrDefault(u => u.Username.Equals(username));
                        if (userInfo != null)
                        {
                            var chatMember = client.GetChatMemberAsync(msg.Chat.Id, userInfo.TelegramId).Result;
                            user = chatMember.User;
                        }
                    }
                }
            }

            if (user != null)
                _WarnUser(client, msg, user, replyMsgId);
        }

        private void _WarnUser(TelegramBotClient client, Message msg, User user, int replyMessageId)
        {
            var member = BotDbContext.Get().GetMember(msg.Chat.Id, user.Id);
            member.Warns++;
            if (member.Warns >= 3)
            {
                _ban?.Invoke(client, msg.Chat, user, replyMessageId, DateTime.UtcNow + TimeSpan.FromDays(3));
            }
            else
            {
                var text = $"{user.GetMension()} предупреждение №{member.Warns}!\n" +
                    $"Бан при получении 3-х предупреждений!";
                client.SendTextMessageAsync(msg.Chat.Id, text, Telegram.Bot.Types.Enums.ParseMode.Markdown, replyToMessageId: replyMessageId);
                BotDbContext.Get().Entry(member).State = System.Data.Entity.EntityState.Modified;
                BotDbContext.Get().SaveChanges();
            }
        }
    }
}