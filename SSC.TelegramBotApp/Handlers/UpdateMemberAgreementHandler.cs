using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Handlers
{
    public class UpdateMemberAgreementHandler : BaseHandler
    {
        public override void Handle(TelegramBotClient client, Message msg)
        {
            if(msg.Text != null && msg.Text.IndexOf("!agreement\n", StringComparison.OrdinalIgnoreCase)>= 0)
            {
                var agreement = msg.Text.Replace("!agreement\n", "");
                Configuration objConfig = WebConfigurationManager.OpenWebConfiguration("~");
                AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
                if (objAppsettings != null)
                {
                    objAppsettings.Settings["MemberAgreement"].Value = agreement;
                    objConfig.Save();
                    client.SendTextMessageAsync(msg.Chat.Id, "Agreement updated success.", replyToMessageId: msg.MessageId);
                }

            }
            else base.Handle(client, msg);
        }
    }
}