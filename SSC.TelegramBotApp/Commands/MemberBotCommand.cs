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
    /// <summary>
    /// Обобщенный класс команды управления участником чата
    /// </summary>
    public class MemberBotCommand : AdminCommand
    {
        private readonly string _name;
        private readonly Action<TelegramBotClient, Chat, User, int> _execute;

        public MemberBotCommand(string name, Action<TelegramBotClient, Chat, User, int> execute)
        {
            _name = name;
            _execute = execute;
        }

        public override string Name => _name;

        public override void Execute(TelegramBotClient client, Message msg)
        {
            if (msg.Entities.Any(e => e.Type == MessageEntityType.Mention))
            {
                for (int i = 0; i < msg.Entities.Length; i++)
                {
                    var entity = msg.Entities[i];
                    if (entity.Type != MessageEntityType.Mention)
                        continue;

                    var user = entity.User;
                    if (user != null)
                    {
                        _execute?.Invoke(client, msg.Chat, user, msg.MessageId);
                    }
                    else
                    {
                        var username = msg.EntityValues.ElementAt(i).Replace("@", "");
                        var userInfo = BotDbContext.Get().UserInfoes.FirstOrDefault(u => u.Username.Equals(username));
                        if (userInfo != null)
                        {
                            var chatMember = client.GetChatMemberAsync(msg.Chat.Id, userInfo.TelegramId).Result;
                            user = chatMember.User;
                            _execute?.Invoke(client, msg.Chat, user, msg.MessageId);
                        }
                    }
                }
            }
            else if (msg.ReplyToMessage != null)
            {
                var user = msg.ReplyToMessage.From;
                _execute?.Invoke(client, msg.Chat, user, msg.ReplyToMessage.MessageId);
            }
            else
            {
                client.SendTextMessageAsync(msg.Chat.Id, "Для выполнения команды необходимо указать пользователя " +
                    "или командой ответить на сообщение пользователя", replyToMessageId: msg.MessageId);
            }
        }
    }
}