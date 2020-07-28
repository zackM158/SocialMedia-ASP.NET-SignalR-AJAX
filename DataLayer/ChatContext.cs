using Entities;
using System.Data.Common;
using System.Data.Entity;

namespace DataLayer
{
    public class ChatContext : DbContext
    {
        public ChatContext() : base("Fakebook")
        {

        }

        public ChatContext(DbConnection connection)
            : base(connection, true)
        {
            Database.CreateIfNotExists();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
