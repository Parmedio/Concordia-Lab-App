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

        [Fact]
        public void CheckUrlForAddAComment()
        {

            string url = "cards/64761756c338bac930497a59/actions/comments?text=Could you test also for Temperature = 50?&key=9ba27d32be683843dd1ffb346ae07641&token=ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193";
            _sut.AddACommentOnACard("64761756c338bac930497a59", "Could you test also for Temperature = 50?", "ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193").Should().Be(url);
        }

        [Fact]
        public void CheckUrlForUpdateAnExperiment()
        {
            string url = "cards/64761756c338bac930497a59?idList=64760975fbea80d6ef329081&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.UpdateAnExperiment("64761756c338bac930497a59", "64760975fbea80d6ef329081").Should().Be(url);
        }
    }
}