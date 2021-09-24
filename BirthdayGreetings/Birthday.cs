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

    public interface ISenderService
    {
        public void Send(Friend friend);
    }

    public class Friend
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal DateTime BirthDate { get; set; }
        internal string Email { get; set; }

    }
}
