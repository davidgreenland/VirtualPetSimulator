using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Helpers.Interfaces;
using Moq;
using VirtualPetSimulator.Actions;

namespace VirtualPetSimulator.Tests.Actions;

public class PlayActionTests
{
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private PlayAction _playAction;
    private const int DEFAULT_PLAY_VALUE = 1;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();

        _testPet.Setup(pet => pet.Name).Returns("Simon");
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.DEFAULT);
        _userCommunicationMock.Setup(x => x.RunOperation(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    [Test]
    public async Task Execute_WhenCalled_IncrementsHappiness()
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(DEFAULT_PLAY_VALUE));
        _userCommunicationMock.Verify(x => x.RunOperation(It.Is<int>(val => val == DEFAULT_PLAY_VALUE), It.IsAny<string>()), Times.Once);
    }

    [TestCase(4)]
    [TestCase(5)]
    [TestCase(10)]
    public async Task Execute_WhenGivenAmount_CanPlayMoreThanDefault(int playLengthRequest)
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, playLengthRequest);
        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(playLengthRequest));
    }

    [Test]
    public async Task Execute_WhenHappinessAtOrBelowThreshold_PetDoesNotPlay()
    {
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.HAPPINESS_PLAY_THRESHOLD - 1);
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);
        var expected = 0;

        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(expected));
    }

    [TestCase(18)]
    [TestCase(-4)]
    [TestCase(10898)]
    public async Task Execute_WhenCalled_CallsValidatorWithValue(int playAmountRequest)
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, playAmountRequest);

        await _playAction.Execute();

        _validatorMock.Verify(x => x.IsNonNegative(It.Is<int>(val => val == playAmountRequest), It.IsAny<string>()), Times.Once());
    }
}
