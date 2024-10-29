using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Helpers.Interfaces;
using Moq;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Tests.Actions;

public class PlayActionTests
{
    private PlayAction _playAction;
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;

    private const int DEFAULT_PLAY_VALUE = 1;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        _testPet.Setup(pet => pet.Name).Returns("Simon");
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.DEFAULT);
        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
        _validatorMock.Setup(x => x.Validate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
        //_validatorMock.Setup(x => x.IsNonNegative(It.Is<int>(val => val < 0), It.IsAny<string>())).Returns(false);
    }

    [Test]
    public async Task Execute_WhenCalled_IncrementsHappiness()
    {

        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(DEFAULT_PLAY_VALUE));
    }

    [TestCase(2)]
    [TestCase(4)]
    public async Task Execute_WhenFeedAmountProvided_CallsUserCommsWithCorrectValue(int playValue)
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, playValue); 
        var expected = playValue * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;

        await _playAction.Execute();

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(val => val == expected)), Times.Once);
    }

    [TestCase(4)]
    [TestCase(5)]
    [TestCase(10)]
    public async Task Execute_WhenGivenAmount_CanPlayMoreThanDefault(int playLengthRequest)
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, playLengthRequest);

        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(playLengthRequest));
    }

    [Test]
    public async Task Execute_WhenHappinessAtOrBelowThreshold_PetDoesNotPlay()
    {
        _testPet.Setup(x => x.Happiness).Returns(AttributeValue.HAPPINESS_PLAY_THRESHOLD - 1);
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);
        var expected = 0;

        var playLength = _playAction.Execute();

        Assert.That(await playLength, Is.EqualTo(expected));
    }

    [TestCase(18)]
    [TestCase(-4)]
    [TestCase(10898)]
    public async Task Execute_WhenCalled_CallsValidatorWithValue(int playAmountRequest)
    {
        _playAction = new PlayAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, playAmountRequest);

        await _playAction.Execute();

        _validatorMock.Verify(x => x.Validate(It.Is<int>(val => val == playAmountRequest), It.IsAny<string>()), Times.Once());
    }
}
