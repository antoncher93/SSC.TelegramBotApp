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
    public class SetAgreementCommand : AdminCommand
    {
        public override string Name => "/agreement";

        public override void Execute(TelegramBotClient client, Message msg)
        {
            if (msg.ReplyToMessage is null)
                return;

            var member = client.GetChatMemberAsync(msg.Chat.Id, msg.From.Id).Result;
            if(member.Status == ChatMemberStatus.Administrator || member.Status == ChatMemberStatus.Creator)
            {
                var agreement = BotDbContext.Get().Agreements.FirstOrDefault(a => a.ChatId.Equals(msg.Chat.Id));
                if(agreement is null)
                {
                    agreement = new Agreement()
                    {
                        Text = msg.ReplyToMessage.Text,
                        ChatId = msg.Chat.Id
                    };

                    BotDbContext.Get().Agreements.Add(agreement);
                    BotDbContext.Get().Entry(agreement).State = System.Data.Entity.EntityState.Added;
                    BotDbContext.Get().SaveChangesAsync();
                }
                else
                {
                    agreement.Text = msg.Text;
                    BotDbContext.Get().Entry(agreement).State = System.Data.Entity.EntityState.Added;
                    BotDbContext.Get().SaveChangesAsync();
                }

                client.SendTextMessageAsync(msg.Chat.Id, "Правила чата были изменены.", replyToMessageId: msg.MessageId);
            }
        }
    }
}