using SSC.TelegramBotApp.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Commands
{
    public class KickUserFromChatComman : Command
    {
        public override string Name =>"2.4 не едет";

        public override void Execute(TelegramBotClient client, Message msg)
        {
            var text = msg.Text;
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;

            client.SendTextMessageAsync(chatId, "You will be kicked from this chat!", replyToMessageId: msgId);
        }

        public override Task ExecuteAsync(TelegramBotClient client, Message msg)
        {
            var chatId = msg.Chat.Id;
            var msgId = msg.MessageId;
            client.SendTextMessageAsync(chatId, "Вы были удалены из чата!", replyToMessageId: msgId).Wait();
            return client.KickChatMemberAsync(chatId, msg.From.Id);
        }
    }
}