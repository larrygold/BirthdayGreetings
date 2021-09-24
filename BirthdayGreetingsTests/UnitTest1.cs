using System;
using System.Collections.Generic;
using NUnit.Framework;
using BirthdayGreetings;
using Moq;

namespace BirthdayGreetingsTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_FireMethodInEmailClass_When_ProcessingBirthdays()
        {
            var mockRepo = new Mock<IRepository>();
            var mockSenderService = new Mock<ISenderService>();

            var john = new Friend()
            {
                LastName = "Doe",
                FirstName = "John",
                BirthDate = DateTime.Parse("2021/09/24"),
                Email = "john.doe@foobar.com"
            };

            var mary = new Friend()
            {
                LastName = "Ann",
                FirstName = "Mary",
                BirthDate = DateTime.Parse("2021/09/24"),
                Email = "mary.ann@foobar.com"
            };

            mockRepo.Setup(x => x.GetListOfFriends())
                .Returns(new List<Friend>()
                {
                    john ,
                    mary
                });

            new Birthday(mockRepo.Object, mockSenderService.Object).Process();

            mockRepo.Verify(x => x.GetListOfFriends(), Times.Once);
            mockSenderService.Verify(x => x.Send(john), Times.Once);
            mockSenderService.Verify(x => x.Send(mary), Times.Once);
        }
    }
}