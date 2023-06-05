using BusinessLogic.APIConsumers.UriCreators;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Moq;

namespace ConcordiaAppTestLayer
{
    public class UrlFactoryTests
    {
        private readonly IUriCreatorFactory _sut;

        public UrlFactoryTests()
        {
            var configuration = Mock.Of<IConfiguration>();
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloAuthorization").GetSection("Key").Value).Returns("9ba27d32be683843dd1ffb346ae07641");
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloAuthorization").GetSection("Token").Value).Returns("ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD");
#if DEBUG
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloTestEnvironment").GetSection("idBoard").Value).Returns("64760975fbea80d6ef329079");
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloTestEnvironment").GetSection("List").GetSection("idToDo").Value).Returns("64760975fbea80d6ef329080");
#else
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloIDsDevelopment").GetSection("idBoard").Value).Returns("64760804e47275c707e05d31");
            Mock.Get(configuration).Setup(x => x.GetSection("TrelloIDsDevelopment").GetSection("List").GetSection("idToDo").Value).Returns("64760804e47275c707e05d38");
#endif
            _sut = new UriCreatorFactory(configuration);
        }

        [Fact]
        public void CheckUrlForGetAllComments()
        {


#if DEBUG
            string url = "boards/64760975fbea80d6ef329079/actions?filter=commentCard&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCommentsOnABoard().Should().Be(url);
#else
            string releaseUrl = "boards/64760804e47275c707e05d31/actions?filter=commentCard&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCommentsOnABoard().Should().Be(releaseUrl);
#endif
        }

        [Fact]
        public void CheckUrlForAllCardsInToDoList()
        {


#if DEBUG
            string url = "lists/64760975fbea80d6ef329080/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCardsOnToDoList().Should().Be(url);
#else
            string releaseUrl = "lists/64760804e47275c707e05d38/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCardsOnToDoList().Should().Be(releaseUrl);
#endif
        }
    }
}