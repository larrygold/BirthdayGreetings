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

            mockRepo.Setup(x => x.GetListOfFriends())
                .Returns(new List<Friend>()
                {
                    new Friend()
                    {
                        LastName = "Doe", FirstName = "John",
                        BirthDate = DateTime.Parse("1982/10/08"), Email = "john.doe@foobar.com"
                    },
                    new Friend()
                    {
                        LastName = "Ann", FirstName = "Mary",
                        BirthDate = DateTime.Parse("1975/09/11"), Email = "mary.ann@foobar.com"
                    }
                });

            new Birthday(mockRepo.Object, mockSenderService.Object).Process();

            mockRepo.Verify(x => x.GetListOfFriends(), Times.Once);
            mockSenderService.Verify(x => x.Send(It.IsAny<List<string>>()), Times.Once);

        }
    }
}