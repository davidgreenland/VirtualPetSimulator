using Moq;
using NUnit.Framework;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
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

        _app = new VirtualPetApp(new Dictionary<char, PetAction>
            {
                { 'S', PetAction.Sleep},
                { 'E', PetAction.Eat },
                { 'P', PetAction.Play }
            },
        new Dictionary<char, PetType> {
                { 'C', PetType.Cat },
        },
        _userCommunicationMock.Object, _timeServiceMock.Object, _actionFactoryMock.Object);
    }


    [Test]
    public void ChoosePetType_WhenUserChoosesAValidType_ReturnsTheType()
    {

        _userCommunicationMock.Setup(x => x.GetUserChoice(It.IsAny<string>())).Returns('C');
        var expected = PetType.Cat;

        var petType = _app.ChoosePetType();

        Assert.That(petType, Is.EqualTo(expected));
    }

    [Test]
    public void ChoosePetType_WhenUserChoosesAnInvalidType_RePromptsUntilValid()
    {
        _userCommunicationMock.SetupSequence(x => x.GetUserChoice(It.IsAny<string>()))
            .Returns('B')
            .Returns('E')
            .Returns('C');

        var expected = PetType.Cat;

        var petType = _app.ChoosePetType();

        Assert.That(petType, Is.EqualTo(expected));
        _userCommunicationMock.Verify(x => x.GetUserChoice(It.IsAny<string>()), Times.Exactly(3));
    }

    [Test]
    public void SetPet_WhenGivenAValidPet_SetsThePet()
    {
        var petName = "Catters";

        _app.SetPet(new CatPet(petName, _soundBehaviourMock.Object));

        Assert.That(_app.pet, Is.Not.Null);
        Assert.That(_app.pet.Name, Is.EqualTo(petName));
    }

    //[Test]
    //public async void Run_()
    //{

    //}

}