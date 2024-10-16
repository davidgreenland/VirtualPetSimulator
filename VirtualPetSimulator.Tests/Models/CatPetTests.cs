using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;
using Moq;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private Mock<ITimeService> _timeService;
    private CatPet _pet;

    [SetUp]
    public void SetUp()
    {
        _timeService = new Mock<ITimeService>();

        _timeService.Setup(mock => mock.Delay(It.IsAny<int>())).Returns(Task.CompletedTask);
        _pet = new CatPet(_timeService.Object, "Simon", AttributeValue.MEDIUM, AttributeValue.MEDIUM);
    }

    [Test]
    public async Task Eat_WhenNotHungry_DoesNotEat()
    {
        var pet = new CatPet(_timeService.Object, "Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);

        await _pet.Eat();

        Assert.That(pet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [Test]
    public async Task Eat_WhenHasHunger_DecrementsHunger()
    {
        await _pet.Eat();

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MEDIUM - 1));
    }

    [Test]
    public async Task Eat_CanDecrementHungerByMoreThan1()
    {
        var whiskas = 4;

        await _pet.Eat(whiskas);

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MEDIUM - whiskas));
    }

    [Test]
    public async Task Eat_WillNotLowerHungerPastZero()
    {
        var catFood = 3;

        await Task.WhenAll(_pet.Eat(catFood), _pet.Eat(catFood), _pet.Eat(catFood), _pet.Eat(catFood));

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [Test]
    public void Sleep_WhenNotTired_DoesNotSleep()
    {
        var pet = new CatPet(_timeService.Object, "Joseph", AttributeValue.MAX, AttributeValue.MIN);

        _pet.Sleep();

        Assert.That(pet.Energy, Is.EqualTo(AttributeValue.MAX));
    }

    [Test]
    public void Sleep_WhenEnergyNotMax_IncrementsEnergy()
    {
        _pet.Sleep();

        Assert.That(_pet.Energy, Is.EqualTo(AttributeValue.MEDIUM + 1));
    }

    [Test]
    public void Sleep_CanIncrementEnergyByMoreThan1()
    {
        var sleepValue = 3;

        _pet.Sleep(sleepValue);

        Assert.That(_pet.Energy, Is.EqualTo(AttributeValue.MEDIUM + sleepValue));
    }
}
