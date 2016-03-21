using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LoveMeHandMake2.Models
{
    public class LoveMeHandMakeContext : DbContext
    {
        public LoveMeHandMakeContext() : base("name=LoveMeHandMakeContext") { }

        public DbSet<Member> Members { get; set; }

        public DbSet<NonMember> NonMembers { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<StoreCanSellCategory> StoreCanSellCategory { get; set; }

        public DbSet<TradeOrder> TradeOrder { get; set; }

        public DbSet<TradePurchaseProduct> TradePurchaseProduct { get; set; }

        public DbSet<NonMemberTradeList> NonMemverTradeList { get; set; }

        public DbSet<DepositHistory> DepositHistory { get; set; }

        public DbSet<DepositRewardRule> DepositRewardRule { get; set; }

        public DbSet<SysParameter> SysParameter { get; set; }

        // Remove 's' from DB table's name. 
        // Example: Members -> Member
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}