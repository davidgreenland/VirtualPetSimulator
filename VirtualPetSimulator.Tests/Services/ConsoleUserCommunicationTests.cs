using NUnit.Framework;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Tests.Services;

public class ConsoleUserCommunicationTests
{
    private ConsoleUserCommunication _userCommunication;
    private Mock<ITimeService> _timeServiceMock;

    [SetUp]
    public void SetUp()
    {
        _timeServiceMock = new Mock<ITimeService>();
        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);

        _userCommunication = new ConsoleUserCommunication(_timeServiceMock.Object);
    }

    [Test]
    public void RunOperation_WhenCalled_CallsWaitForOperationWithCorrectArguments()
    {
        var repetitions = 2;
        var message = "Hello";
        var expectedMilliseconds = repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS;

        var result = _userCommunication.RunOperation(repetitions, message);

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(i => i == expectedMilliseconds)));
    }
}
