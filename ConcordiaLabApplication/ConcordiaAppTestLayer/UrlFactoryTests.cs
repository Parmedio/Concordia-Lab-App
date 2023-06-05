using BusinessLogic.APIConsumers.UriCreators;

using FluentAssertions;

namespace ConcordiaAppTestLayer
{
    public class UrlFactoryTests
    {
        private readonly IUriCreatorFactory _sut;

        public UrlFactoryTests(IUriCreatorFactory sut)
        {
            _sut = sut;
        }

        [Fact]
        public void CheckUrlForGetAllComments()
        {


#if DEBUG
            string url = "https://api.trello.com/1/boards/64760975fbea80d6ef329079/actions?filter=commentCard&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCommentsOnABoard().Should().Be(url);
#else
            string releaseUrl = "https://api.trello.com/1/boards/64760804e47275c707e05d31/actions?filter=commentCard&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD";
            _sut.GetAllCommentsOnABoard().Should().Be(releaseUrl);
#endif
        }
    }
}