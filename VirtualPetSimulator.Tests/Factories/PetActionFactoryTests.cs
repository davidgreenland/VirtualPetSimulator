using Moq;
using NUnit.Framework;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Factories;

public class PetActionFactoryTests
{
    private PetActionFactory _actionFactory;
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();
        _actionFactory = new PetActionFactory(_validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);
    }

    [Test]
    public void CreatePetAction_WhenGivenAValidAction_ShouldReturnCorrectAction()
    {
        PetAction actionType = PetAction.Play;

        var petAction = _actionFactory.CreatePetAction(_testPet.Object, actionType);

        Assert.That(petAction, Is.TypeOf<PlayAction>());
    }
}
