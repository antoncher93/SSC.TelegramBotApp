using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BotDbContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}