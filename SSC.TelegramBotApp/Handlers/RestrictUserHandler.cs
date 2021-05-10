using SSC.TelegramBotApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class RestrictUserHandler : BaseHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Text != null && msg.Text.IndexOf("!restrict", StringComparison.OrdinalIgnoreCase)>=0)
            {
                if(msg.Entities != null)
                {
                    foreach(var entity in msg.Entities)
                    {
                        var permissions = new ChatPermissions()
                        {
                            //CanAddWebPagePreviews = true,
                            //CanChangeInfo = true,
                            //CanInviteUsers = true,
                            //CanPinMessages = true,
                            CanSendMediaMessages = false,
                            CanSendMessages = false,
                            CanSendOtherMessages = false
                            //CanSendPolls = true
                        };

                        var untilDate = DateTime.UtcNow.AddDays(3);

                        client.RestrictChatMemberAsync(msg.Chat.Id, entity.User.Id, permissions, untilDate);

                        //client.PromoteChatMemberAsync(msg.Chat.Id, entity.User.Id);

                        client.SendTextMessageAsync(msg.Chat.Id, $"{entity.User.GetMension()} понижен в правах.", 
                            replyToMessageId: msg.MessageId, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

                    }
                }
            }
            else base.Handle(client, msg);
        }
    }
}