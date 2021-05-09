using SSC.TelegramBotApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public class WarnUserCommand : Command
    {
        public override string Name => "Warn!";

        public override void Execute(TelegramBotClient client, Message msg)
        {
            throw new NotImplementedException();
        }

        public override Task ExecuteAsync(TelegramBotClient client, Message msg)
        {
            if (msg.Entities != null && msg.Entities.Length > 0)
            {
                var warn = 0;
                var entity = msg.Entities[0];
                var userId = entity.User.Id;
                try
                {
                    var botDB = BotDbContext.Get();
                    var user = botDB.UserInfoes.FirstOrDefault(u => u.TelegramId.Equals(userId));
                    if (user is null)
                    {
                        user = new Models.UserInfo()
                        {
                            TelegramId = entity.User.Id,
                            FirstName = entity.User.FirstName,
                            LastName = entity.User.LastName,
                            IsBanned = true,
                            Warns = 0
                        };

                        botDB.UserInfoes.Add(user);
                        botDB.Entry(user).State = System.Data.Entity.EntityState.Added;
                        botDB.SaveChangesAsync().Wait();
                    }

                    user.Warns++;
                    warn = user.Warns;
                    botDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    botDB.SaveChangesAsync().Wait();
                }
                catch (Exception e)
                {
                    client.SendTextMessageAsync(msg.Chat.Id, "Error: " + e.Message, replyToMessageId: msg.MessageId);
                }


                return client.SendTextMessageAsync(msg.Chat.Id, "@" + entity.User.Username + " Предупреждение номер " + warn);
            }
            return client.SendTextMessageAsync(msg.Chat.Id, "Для выполнения команды необходимо указать пользователя чата", replyToMessageId: msg.MessageId);
        }
    }
}