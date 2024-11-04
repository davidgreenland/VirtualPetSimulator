using NUnit.Framework;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Tests.Services;

public class ConsoleUserCommunicationServiceTests
{
    private Mock<IPet> _testPet;
    private ConsoleUserCommunicationService _userCommunication;
    private Mock<ITimeService> _timeServiceMock;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _timeServiceMock = new Mock<ITimeService>();

        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.DEFAULT);
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.DEFAULT);
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.DEFAULT);

        _userCommunication = new ConsoleUserCommunicationService(_testPet.Object, _timeServiceMock.Object);
    }

    [Test]
    public async Task RunOperation_WhenCalled_CallsWaitForOperationWithCorrectArguments()
    {
        var repetitions = 2;
        var message = "Hello";
        var image = "CatPic";
        var expectedMilliseconds = repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS;

        await _userCommunication.RunOperation(repetitions, message, image);

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(i => i == expectedMilliseconds)));
    }

    [Test] 
    public async Task RunOperation_WhenCalled_Displays()
    {

    }
}
