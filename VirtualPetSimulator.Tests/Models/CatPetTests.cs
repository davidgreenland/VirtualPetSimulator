using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;
using Moq;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private Mock<ITimeService> _timeServiceMock;
    private CatPet _testCatPet;

    [SetUp]
    public void SetUp()
    {
        _timeServiceMock = new Mock<ITimeService>();

        _timeServiceMock.Setup(mock => mock.Delay(It.IsAny<int>())).Returns(Task.CompletedTask);
        _testCatPet = new CatPet(_timeServiceMock.Object, "Simon");
    }

    [Test]
    public async Task Eat_WhenNotHungry_DoesNotEat()
    {
        var notHungryPet = new CatPet(_timeServiceMock.Object, "Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);

        await notHungryPet.Eat();

        Assert.That(notHungryPet.Hunger, Is.EqualTo(AttributeValue.MIN));
        _timeServiceMock.Verify(x => x.Delay(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Eat_WhenHasHunger_DecrementsHunger()
    {
        await _testCatPet.Eat();

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.DEFAULT - 1));
    }

    [Test]
    public async Task Eat_CanDecrementHungerByMoreThan1()
    {
        var whiskas = 4;

        await _testCatPet.Eat(whiskas);

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.DEFAULT - whiskas));
    }

    [Test]
    public async Task Eat_WillNotLowerHungerPastMinimum()
    {
        var catFood = 3;

        await Task.WhenAll(_testCatPet.Eat(catFood), _testCatPet.Eat(catFood), _testCatPet.Eat(catFood), _testCatPet.Eat(catFood));

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [Test]
    public async Task Sleep_WhenNotTired_DoesNotSleep()
    {
        var maxEnergyCat = new CatPet(_timeServiceMock.Object, "Joseph", energy: AttributeValue.MAX, AttributeValue.MIN);

        await maxEnergyCat.Sleep();

        Assert.That(maxEnergyCat.Energy, Is.EqualTo(AttributeValue.MAX));
        _timeServiceMock.Verify(x => x.Delay(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Sleep_WhenEnergyNotMax_IncrementsEnergy()
    {
        await _testCatPet.Sleep();

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.DEFAULT + 1));
    }

    [Test]
    public async Task Sleep_CanIncrementEnergyByMoreThan1()
    {
        var sleepValue = 3;

        await _testCatPet.Sleep(sleepValue);

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.DEFAULT + sleepValue));
    }

    [Test]
    public async Task Sleep_WillNotRaiseEnergyBeyondMax()
    {
        var sleepValue = 5;

        await Task.WhenAll(_testCatPet.Sleep(sleepValue), _testCatPet.Sleep(sleepValue));

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.MAX));
    }
}
