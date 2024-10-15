using NUnit.Framework;
using VirtualPetSimulator.Models;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private CatPet _pet;
    private readonly int _fullEnergy = 10;
    private readonly int _mediumHunger = 5;

    [SetUp]
    public void SetUp()
    {
        _pet = new CatPet("Simon", _fullEnergy, _mediumHunger);
    }

    [Test]
    public void Eat_WhenNotHungry_DoesNotEat()
    {
        var isNotHungry = 0;
        var pet = new CatPet("Joseph", _fullEnergy, isNotHungry);

        Assert.That(pet.Hunger, Is.EqualTo(isNotHungry));
    }

    [Test]
    public void Eat_WhenHasHunger_DecrementsHunger()
    {
        _pet.Eat();

        Assert.That(_pet.Hunger, Is.EqualTo(_mediumHunger - 1));
    }

    [Test]
    public void Eat_CanDecrementHungerByMoreThan1()
    {
        var whiskas = 4;

        _pet.Eat(whiskas);

        Assert.That(_pet.Hunger, Is.EqualTo(_mediumHunger - whiskas));
    }

    [TestCase()]
    public void Eat_WillNotLowerHungerPastZero()
    {
        var catFood = 3;

        _pet.Eat(catFood);
        _pet.Eat(catFood);
        _pet.Eat(catFood);
        _pet.Eat(catFood);

        Assert.That(_pet.Hunger, Is.EqualTo(0));
    }
}
