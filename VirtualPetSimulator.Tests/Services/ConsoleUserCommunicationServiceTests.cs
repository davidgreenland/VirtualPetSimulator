using NUnit.Framework;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Tests.Services;

public class ConsoleUserCommunicationServiceTests
{
    private ConsoleUserCommunicationService _userCommunication;
    private Mock<ITimeService> _timeServiceMock;

    [SetUp]
    public void SetUp()
    {
        _timeServiceMock = new Mock<ITimeService>();
        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);

        _userCommunication = new ConsoleUserCommunicationService(_timeServiceMock.Object);
    }

    [Test]
    public async Task RunOperation_WhenCalled_CallsWaitForOperationWithCorrectArguments()
    {
        var repetitions = 2;
        var message = "Hello";
        var image = "CatPic";
        var expectedMilliseconds = repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS;

        var result = _userCommunication.RunOperation(repetitions, message, image);

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(i => i == expectedMilliseconds)));
    }
}
