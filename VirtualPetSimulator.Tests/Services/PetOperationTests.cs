using Moq;
using NUnit.Framework;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Services;

public class PetOperationTests
{
    [TestCase(-1)]
    public async Task RunOperation_WhenGivenNegative_ShouldThrowException()
    {

    }
    //s
    //[TestCase(1)]
    //[TestCase(4000)]
    //[TestCase(99999)]
    //public async Task RunOperation_WhenCalled_ShouldDelayBySpecifiedTime()
    //{
    //    var timeServiceMock = new Mock<ITimeService>();

    //    timeServiceMock.Setup(mock => mock.RunOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
    //}
}
