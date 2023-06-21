using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;

using ConcordiaAppTestLayer.BusinessLogicTests.MockData;

using FluentAssertions;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer.BusinessLogicTests;

public class UploaderTests
{
    private readonly IUploader _uploader;
    private readonly IApiSender _sender;
    private readonly IExperimentRepository _experimentRepository;
    private readonly ICommentRepository _commentRepository;

    public UploaderTests()
    {
        _sender = Mock.Of<IApiSender>();
        _experimentRepository = Mock.Of<IExperimentRepository>();
        _commentRepository = Mock.Of<ICommentRepository>();
        _uploader = new Uploader(_sender, _experimentRepository, _commentRepository);
    }

    [Fact]
    public void UploadShouldWork()
    {
        var data = new DataSyncerMockData2();
        Mock.Get(_experimentRepository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { data.LocalExperiment1InToDo });
        Mock.Get(_sender).Setup(p => p.UpdateAnExperiment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        Mock.Get(_sender).Setup(p => p.AddAComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("notUsefulString");
        _uploader.Invoking(p => p.Upload()).Should().NotThrowAsync();
        _uploader.Upload().Result.Message.Should().NotBeEmpty();
    }
}
