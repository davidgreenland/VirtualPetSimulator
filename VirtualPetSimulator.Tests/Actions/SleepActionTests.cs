using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;
using Moq;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Tests.Actions;

public class SleepActionTests
{
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private Mock<ITimeService> _timeServiceMock;
    private SleepAction _sleepAction;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _timeServiceMock = new Mock<ITimeService>();

        _testPet.Setup(x => x.Energy).Returns(AttributeValue.DEFAULT);
        _timeServiceMock.Setup(mock => mock.WaitForOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
        _validatorMock.Setup(x => x.Validate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
    }

    [Test]
    public async Task Execute_WhenNotTired_DoesNotSleep()
    {
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.MAX);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(0));
    }

    [Test]
    public async Task Execute_WhenNotTired_DoesNotCallChangeEnergy()
    {
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.MAX);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);

        await _sleepAction.Execute();

        _testPet.Verify(x => x.ChangeEnergy(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Execute_WhenEnergyNotMax_CallsChangeEnergyCorrectTimes()
    {
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);
        _testPet.SetupSequence(x => x.Energy)
            .Returns(AttributeValue.MAX - 2)
            .Returns(AttributeValue.MAX - 1)
            .Returns(AttributeValue.MAX);
        var expected = 2;

        var amountSlept = await _sleepAction.Execute();

        _testPet.Verify(x => x.ChangeEnergy(It.Is<int>(val => val == 1)), Times.Exactly(expected));
    }

    [Test]
    public async Task Execute_WhenPetSleeps_SleepsUntilEnergyMax()
    {
        var startingEnergy = 6;
        var expected = 4;
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object);
        _testPet.SetupSequence(x => x.Energy)
            .Returns(startingEnergy)
            .Returns(7)
            .Returns(8)
            .Returns(9)
            .Returns(AttributeValue.MAX);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(expected));
    }

    [TestCase(2, 3)]
    [TestCase(2, 8)]
    [TestCase(5, 5)]
    [TestCase(7, 2)]
    [TestCase(9, 1)]
    public async Task Execute_WhenSleepSpecifiedLessOrEqualToEnergyDeficit_SleepsForSpecifiedAmount(int energy, int sleepValue)
    {
        _testPet.Setup(x => x.Energy).Returns(energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, sleepValue);
        var expected = sleepValue;

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(expected));
    }

    [TestCase(9, 3, 1)]
    [TestCase(5, 8, 5)]
    [TestCase(3, 10, 7)]
    [TestCase(0, 14, 10)]
    public async Task Execute_WhenSleepSpecifiedHigherThanEnergyDeficit_SleepsForExpectedAmount(int energy, int sleepValue, int expected)
    {
        energy--; // so first Energy computes
        _testPet.Setup(x => x.Energy)
            .Returns(() => ++energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, sleepValue);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(expected));
    }

    [TestCase(9, 3, 1)]
    [TestCase(5, 8, 5)]
    [TestCase(3, 10, 7)]
    [TestCase(0, 14, 10)]
    public async Task Execute_WhenSleeps_CallsValidatorWithValue(int energy, int sleepValue, int expected)
    {
        energy--;
        _testPet.Setup(x => x.Energy)
            .Returns(() => ++energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, sleepValue);

        await _sleepAction.Execute();

        _validatorMock.Verify(x => x.Validate(It.Is<int>(val => val == sleepValue), It.IsAny<string>()), Times.Once());
    }

    [TestCase(9, 3, 1)]
    [TestCase(5, 8, 5)]
    [TestCase(3, 10, 7)]
    [TestCase(0, 14, 10)]
    [TestCase(4, -14, 0)]
    [TestCase(3, -1, 0)]
    public async Task Execute_WhenSleeps_UserCommsCalledCorrectNumberOfTimes(int energy, int sleepValue, int expected)
    {
        energy--;
        _testPet.Setup(x => x.Energy).Returns(() => ++energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, _timeServiceMock.Object, sleepValue);

        await _sleepAction.Execute();

        _timeServiceMock.Verify(x => x.WaitForOperation(It.Is<int>(x => x == AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS), It.IsAny<CancellationToken>()), Times.Exactly(expected));
    }
}
