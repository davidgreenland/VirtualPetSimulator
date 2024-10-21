using NUnit.Framework;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Helpers.Interfaces;
using Moq;

namespace VirtualPetSimulator.Tests.Services;

public class SleepActionTests
{
    private Mock<IPet> _testPet;
    private Mock<IValidator> _validatorMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private SleepAction _sleepAction;
    private const int DEFAULT_SLEEP_VALUE = 1;

    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();
        _validatorMock = new Mock<IValidator>();
        _userCommunicationMock = new Mock<IUserCommunication>();

        _testPet.Setup(x => x.Energy).Returns(AttributeValue.DEFAULT);
        _userCommunicationMock.Setup(mock => mock.RunOperation(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.CompletedTask);
    }

    [Test]
    public async Task Execute_WhenNotTired_DoesNotSleep()
    {
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.MAX);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(0));
    }

    [Test]
    public async Task Execute_WhenNotTired_DoesNotCallChangeEnergy()
    {
        _testPet.Setup(x => x.Energy).Returns(AttributeValue.MAX);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);

        await _sleepAction.Execute();

        _testPet.Verify(x => x.ChangeEnergy(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Execute_WhenEnergyNotMax_IncreasesEnergy()
    {
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);
        _testPet.SetupSequence(x => x.Energy)
            .Returns(AttributeValue.MAX - 1)
            .Returns(AttributeValue.MAX);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.GreaterThan(0));
        _testPet.Verify(x => x.ChangeEnergy(It.IsAny<int>()));
    }

    [Test]
    public async Task Execute_WhenPetSleeps_SleepsUntilEnergyMax()
    {
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object);
        _testPet.SetupSequence(x => x.Energy)
            .Returns(AttributeValue.DEFAULT)
            .Returns(7)
            .Returns(8)
            .Returns(9)
            .Returns(AttributeValue.MAX);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(AttributeValue.MAX - AttributeValue.DEFAULT));
    }

    [TestCase(2, 3)]
    [TestCase(2, 8)]
    [TestCase(5, 5)]
    [TestCase(7, 2)]
    [TestCase(9, 1)]
    public async Task Execute_WhenSleepSpecifiedLessOrEqualToEnergyDeficit_SleepsForSpecifiedAmount(int energy, int sleepValue)
    {
        _testPet.Setup(x => x.Energy).Returns(energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, sleepValue);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(sleepValue));
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
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, sleepValue);

        var amountSlept = _sleepAction.Execute();

        Assert.That(await amountSlept, Is.EqualTo(expected));
    }

    [TestCase(9, 3, 1)]
    [TestCase(5, 8, 5)]
    [TestCase(3, 10, 7)]
    [TestCase(0, 14, 10)]
    public async Task Execute_WhenSleeps_UserCommsCalledCorrectNumberOfTimes(int energy, int sleepValue, int expected)
    {
        energy--;
        _testPet.Setup(x => x.Energy)
            .Returns(() => ++energy);
        _sleepAction = new SleepAction(_testPet.Object, _validatorMock.Object, _userCommunicationMock.Object, sleepValue);

        await _sleepAction.Execute();

        _userCommunicationMock.Verify(x => x.RunOperation(It.Is<int>(x => x == DEFAULT_SLEEP_VALUE), It.IsAny<string>()), Times.Exactly(expected));
    }
}
