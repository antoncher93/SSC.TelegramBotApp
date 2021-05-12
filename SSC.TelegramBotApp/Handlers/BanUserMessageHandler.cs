using SSC.TelegramBotApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class BanUserMessageHandler : MessageHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg != null && msg.Text != null && msg.Text.IndexOf("!ban", StringComparison.OrdinalIgnoreCase)>=0)
            {
                if(msg.Entities != null)
                {
                    foreach(var entity in msg.Entities)
                    {
                        entity.User.BanInChat(client, msg.Chat.Id, msg.MessageId);
                        //entity.User.BanInChat(client, msg.Chat.Id, msg.MessageId, DateTime.UtcNow + TimeSpan.FromSeconds(35));
                    }
                }
                else if(msg.ReplyToMessage != null)
                {
                    msg.ReplyToMessage.From
                        .BanInChat(client, msg.Chat.Id, msg.MessageId);
                }
            }
            else base.Handle(client, msg);
        }
    }
}