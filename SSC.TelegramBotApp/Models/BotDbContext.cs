using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Telegram.Bot.Types;

namespace SSC.TelegramBotApp.Models
{
    public class BotDbContext : DbContext
    {
        private static BotDbContext _instance;
        private DbSet<UserInfo> userInfos;

        public static BotDbContext Get() => _instance ?? (_instance = new BotDbContext());

        public BotDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserInfo> UserInfoes { get => userInfos; 
            set => userInfos = value; }

        public DbSet<Member> Members { get; set; }
        public DbSet<Agreement> Agreements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BotDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        public Member GetMember(long chatId, long userId)
        {
            try
            {
                var member = Members.FirstOrDefault(m => m.ChatId.Equals(chatId) && m.UserId.Equals(userId));
                if (member is null)
                {
                    member = new Member()
                    {
                        ChatId = chatId,
                        UserId = userId,
                        Warns = 0
                    };
                    member = Members.Add(member);
                    Entry(member).State = EntityState.Added;
                    SaveChanges();
                }
                return member;
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public UserInfo GetUserInfo(User user)
        {
            try
            {
                var userInfo = UserInfoes.FirstOrDefault(ui => ui.TelegramId.Equals(user.Id));
                if (userInfo is null)
                {
                    userInfo = new UserInfo()
                    {
                        TelegramId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Username = user.Username
                    };
                    UserInfoes.Add(userInfo);
                    Entry(userInfo).State = EntityState.Added;
                    SaveChanges();
                }
                return userInfo;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}