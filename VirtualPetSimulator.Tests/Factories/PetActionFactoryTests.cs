using Moq;
using NUnit.Framework;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Tests.Factories;

public class PetActionFactoryTests
{
    private PetActionFactory _actionFactory;
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;
    private Mock<ITimer> _timerMock;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _timerMock = new Mock<ITimer>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();
        _actionFactory = new PetActionFactory(_validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);
    }

    [Test]
    public void CreatePetAction_WhenGivenAValidAction_ShouldReturnCorrectAction()
    {
        PetAction actionType = PetAction.Play;

        var petAction = _actionFactory.CreatePetAction(_testPet.Object, actionType, _timerMock.Object);

        Assert.That(petAction, Is.TypeOf<PlayAction>());
    }
}
