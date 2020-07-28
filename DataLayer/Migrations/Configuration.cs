namespace DataLayer.Migrations
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataLayer.ChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataLayer.ChatContext context)
        {
            //USERS

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 1,
                FirstName = "Zack",
                LastName = "Mitchell",
                EmailAddress = "ZM@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_1.jpg",
                FriendIds = "2,3,4,5"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 2,
                FirstName = "Liam",
                LastName = "Gallagher",
                EmailAddress = "LG@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_2.jpg",
                FriendIds = "1,3,4,5"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 3,
                FirstName = "Noel",
                LastName = "Gallagher",
                EmailAddress = "NG@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_3.jpg",
                FriendIds = "1,2,4,5"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 4,
                FirstName = "Karl",
                LastName = "Pilkington",
                EmailAddress = "KP@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_4.jpeg",
                FriendIds = "1,2,3,5"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 5,
                FirstName = "Mark",
                LastName = "Zuckaberg",
                EmailAddress = "MZ@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_5.jpg",
                FriendIds = "1,2,3,4"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 6,
                FirstName = "Norris",
                LastName = "Cole",
                EmailAddress = "NC@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_6.jpg",
                FriendIds = "7"
            });

            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 7,
                FirstName = "Gail",
                LastName = "Platt",
                EmailAddress = "GP@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_7.jpg",
                FriendIds = "6"
            });


            context.Users.AddOrUpdate(u => u.UserId, new User()
            {
                UserId = 8,
                FirstName = "Tony",
                LastName = "Stark",
                EmailAddress = "TS@gmail.com",
                Password = "1bc1a361f17092bc7af4b2f82bf9194ea9ee2ca49eb2e53e39f555bc1eeaed74",
                Salt = "salt",
                DateJoined = DateTime.Now,
                ImageUrl = "User_ID_8.jpg"
            });

            //STATUSES
            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 1,
                UserId = 2,
                SentAt = DateTime.Now,
                Text = "My first status."
            });

            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 2,
                UserId = 2,
                SentAt = DateTime.Now,
                Text = "Noel Sucks, as you were LG x",
                Likes = 3,
                LikedIds = "3,4,5"
            });

            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 3,
                UserId = 3,
                SentAt = DateTime.Now,
                Text = "My first status."
            });

            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 4,
                UserId = 4,
                SentAt = DateTime.Now,
                Text = "My first status."
            });

            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 5,
                UserId = 4,
                SentAt = DateTime.Now,
                Text = "A slug is always on its own. It's a lonely insect.",
                Likes = 2,
                LikedIds = "6,8"
            });

            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 6,
                UserId = 4,
                SentAt = DateTime.Now,
                Text = "Man moths?",
                Likes = 2,
                LikedIds = "7,8"
            });


            context.Statuses.AddOrUpdate(s => s.StatusId, new Status()
            {
                StatusId = 7,
                UserId = 4,
                SentAt = DateTime.Now,
                Text = "You never see an old man having a Twix.",
                Likes = 5,
                LikedIds = "2,5,6,7,8"
            });

        }
    }
}
