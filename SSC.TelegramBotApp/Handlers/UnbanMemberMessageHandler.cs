using SSC.TelegramBotApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class UnbanMemberMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Text != null && msg.Text.IndexOf("!unban") >= 0)
            {
                if (msg.Entities != null)
                {
                    foreach(var entity in msg.Entities)
                    {
                        var user = entity.User;
                        if (user != null)
                            user.Unban(client, msg.Chat.Id);
                    }
                }
                else
                    client.SendTextMessageAsync(msg.Chat.Id, "Для разблокировки необходимо указать пользователей!", replyToMessageId: msg.MessageId);

            }
            else base.Handle(client, msg);
        }
    }
}