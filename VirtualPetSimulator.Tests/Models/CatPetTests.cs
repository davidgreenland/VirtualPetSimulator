using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private CatPet _pet;

    [SetUp]
    public void SetUp()
    {
        _pet = new CatPet("Simon", AttributeValue.MEDIUM, AttributeValue.MEDIUM);
    }

    [Test]
    public void Eat_WhenNotHungry_DoesNotEat()
    {
        var pet = new CatPet("Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);

        Assert.That(pet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [Test]
    public void Eat_WhenHasHunger_DecrementsHunger()
    {
        _pet.Eat();

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MEDIUM - 1));
    }

    [Test]
    public void Eat_CanDecrementHungerByMoreThan1()
    {
        var whiskas = 4;

        _pet.Eat(whiskas);

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MEDIUM - whiskas));
    }

    [TestCase()]
    public void Eat_WillNotLowerHungerPastZero()
    {
        var catFood = 3;

        _pet.Eat(catFood);
        _pet.Eat(catFood);
        _pet.Eat(catFood);
        _pet.Eat(catFood);

        Assert.That(_pet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [Test]
    public void Eat_WhenEnergyNotMax_ShouldIncreaseEnergyByOne()
    {
        var catFood = 3;

        _pet.Eat(catFood);

        Assert.That(_pet.Energy, Is.EqualTo(AttributeValue.MEDIUM + 1));
    }

    [Test]
    public void Eat_ShouldNotRaiseEnergyAboveMax()
    {
        var fullEnergyPet = new CatPet("Joseph", AttributeValue.MAX, AttributeValue.MEDIUM);
        var bigMeal = 9;

        fullEnergyPet.Eat(bigMeal);

        Assert.That(fullEnergyPet.Energy, Is.EqualTo(AttributeValue.MAX));
    }

    [Test]
    public void Eat_IfHungerMinWhenFed_EnergyDoesNotIncrease()
    {
        var notHungryPet = new CatPet("Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);
        var littleMeal = 1;

        notHungryPet.Eat(littleMeal);

        Assert.That(notHungryPet.Energy, Is.EqualTo(AttributeValue.MEDIUM));
    }
}
