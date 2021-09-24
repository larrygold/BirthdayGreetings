using System;
using System.Collections.Generic;
using System.Linq;

namespace BirthdayGreetings
{
    public class Birthday
    {
        private readonly IRepository _repo;
        private readonly ISenderService _sender;
        public Birthday(IRepository repo, ISenderService sender)
        {
            _repo = repo;
            _sender = sender;
        }
        public void Process()
        {
            var birthdayPeople = _repo.GetListOfFriends();
            foreach (var birthdayPerson in birthdayPeople)
            {
                if (
                    (birthdayPerson.BirthDate.Day == DateTime.Today.Day)
                    &&
                    (birthdayPerson.BirthDate.Month == DateTime.Today.Month)
                )
                {
                    _sender.Send(birthdayPerson);
                }
            }
        }
    }

    public interface IRepository
    {
        public List<Friend> GetListOfFriends();
    }

    public class TextRepository : IRepository
    {
        private ITextFile _textFile;
        public TextRepository(ITextFile textFile)
        {
            _textFile = textFile;
        }
        public List<Friend> GetListOfFriends()
        {
            var friendsObjects = new List<Friend>();
            var friends = _textFile.GetRawText().Split("\n");
            foreach (var friend in friends)
            {
                var aFriendData = friend.Split(", ");
                var aFriend = new Friend()
                {
                    FirstName = aFriendData[1],
                    LastName = aFriendData[0],
                    BirthDate = DateTime.Parse(aFriendData[2]),
                    Email = aFriendData[3]
                };
                friendsObjects.Add(aFriend);
            }
            return friendsObjects;
        }
    }

    public interface ISenderService
    {
        public void Send(Friend friend);
    }

    public interface ITextFile
    {
        public string GetRawText();
    }

    public class Friend
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal DateTime BirthDate { get; set; }
        internal string Email { get; set; }
        public override bool Equals(object otherFriend)
        {
            var otherFriendAsFriend = (Friend) otherFriend;
            return ((FirstName == otherFriendAsFriend.FirstName) && (LastName == otherFriendAsFriend.LastName));
        }
    }
}
