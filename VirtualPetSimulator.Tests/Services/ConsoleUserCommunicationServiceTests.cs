using NUnit.Framework;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Tests.Services;

public class ConsoleUserCommunicationServiceTests
{
    private ConsoleUserCommunicationService _userCommunication;
    private Mock<IPet> _testPet;
    private Mock<IAsciiArtService> _artServiceMock;
    private Mock<ITimeService> _timeServiceMock;

    [SetUp]
    public void SetUp()
    {
        _userCommunication = new ConsoleUserCommunicationService(_timeServiceMock.Object, _artServiceMock.Object);

        _artServiceMock = new Mock<IAsciiArtService>();
        _timeServiceMock = new Mock<ITimeService>();

        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.DEFAULT);
        _testPet.Setup(x => x.Hunger).Returns(AttributeValue.DEFAULT);
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.DEFAULT);
    }
}
