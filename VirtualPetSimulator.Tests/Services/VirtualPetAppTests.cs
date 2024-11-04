using Moq;
using NUnit.Framework;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Configuration;
using VirtualPetSimulator.Factories.Interfaces;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Services;

class VirtualPetAppTests
{
    private VirtualPetApp _app;
    private Mock<IPet> _testPet;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;
    private Mock<IPetActionFactory> _actionFactoryMock;
    private Mock<ISoundAction> _soundBehaviourMock;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();
        _soundBehaviourMock = new Mock<ISoundAction>();
        _actionFactoryMock = new Mock<IPetActionFactory>();

        _app = new VirtualPetApp(new KeyStrokeMappings(),
        _userCommunicationMock.Object, _timeServiceMock.Object, _actionFactoryMock.Object);
    }


    [Test]
    public void ChoosePetType_WhenUserChoosesAValidType_ReturnsTheType()
    {

        _userCommunicationMock.Setup(x => x.GetUserChoice()).Returns('C');
        var expected = PetType.Cat;

        var petType = _app.ChoosePetType();

        Assert.That(petType, Is.EqualTo(expected));
    }

    [Test]
    public void ChoosePetType_WhenUserChoosesAnInvalidType_RePromptsUntilValid()
    {
        _userCommunicationMock.SetupSequence(x => x.GetUserChoice())
            .Returns('B')
            .Returns('E')
            .Returns('C');

        var expected = PetType.Cat;

        var petType = _app.ChoosePetType();

        Assert.That(petType, Is.EqualTo(expected));
        _userCommunicationMock.Verify(x => x.GetUserChoice(), Times.Exactly(3));
    }


    //[Test]
    //public async void Run_()
    //{

    //}

}