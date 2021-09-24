using System;
using System.Collections.Generic;

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
            _repo.GetListOfFriends();
            _sender.Send(new List<string>());
        }
    }

    public interface IRepository
    {
        public List<Friend> GetListOfFriends();
    }

    public interface ISenderService
    {
        public void Send(List<string> emails);
    }

    public class Friend
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal DateTime BirthDate { get; set; }
        internal string Email { get; set; }

    }
}
