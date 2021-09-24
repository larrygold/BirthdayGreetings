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
        public void Should_FireDBAndEmailMethods_When_Processing2Birthdays()
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

        [Test]
        public void Should_FireDBAndEmailMethods_When_DifferentBirthDates()
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
                BirthDate = DateTime.Parse("1994/08/21"),
                Email = "mary.ann@foobar.com"
            };

            mockRepo.Setup(x => x.GetListOfFriends())
                .Returns(new List<Friend>()
                {
                    john,
                    mary
                });

            new Birthday(mockRepo.Object, mockSenderService.Object).Process();

            mockRepo.Verify(x => x.GetListOfFriends(), Times.Once);
            mockSenderService.Verify(x => x.Send(It.IsAny<Friend>()), Times.Once);
            mockSenderService.Verify(x => x.Send(john), Times.Once);
        }

        [Test]
        public void Should_ParseTestDB_When_GetAllIsCalled()
        {
            var mockTextFile = new Mock<ITextFile>();
            mockTextFile.Setup(x => x.GetRawText())
                .Returns("Doe, John, 2021/09/24, john.doe@foobar.com\nAnn, Mary, 1994/08/21, mary.ann@foobar.com");

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
                BirthDate = DateTime.Parse("1994/08/21"),
                Email = "mary.ann@foobar.com"
            };

            var actual = new TextRepository(mockTextFile.Object).GetListOfFriends();
            var expected = new List<Friend>() {john, mary};
            CollectionAssert.AreEqual(expected, actual);

        }
    }
}