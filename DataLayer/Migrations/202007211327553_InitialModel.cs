namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        FriendRequestId = c.Int(nullable: false, identity: true),
                        Senderid = c.Int(nullable: false),
                        Recieverid = c.Int(nullable: false),
                        Seen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FriendRequestId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecieverId = c.Int(nullable: false),
                        SenderName = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        SentAt = c.DateTime(nullable: false),
                        Seen = c.Boolean(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        SentAt = c.DateTime(nullable: false),
                        Likes = c.Int(nullable: false),
                        LikedIds = c.String(),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        EmailAddress = c.String(nullable: false, maxLength: 250),
                        Password = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        ImageUrl = c.String(),
                        FriendIds = c.String(),
                        DateJoined = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.EmailAddress, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "User_UserId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "EmailAddress" });
            DropIndex("dbo.Messages", new[] { "User_UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Status");
            DropTable("dbo.Messages");
            DropTable("dbo.FriendRequests");
        }
    }
}
