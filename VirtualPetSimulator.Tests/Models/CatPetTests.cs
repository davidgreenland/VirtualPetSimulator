using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private CatPet _defaultCatPet;
    private const int SINGLE_INCREMENT = 1;

    [SetUp]
    public void SetUp()
    {
        _defaultCatPet = new CatPet("Simon");
    }

    [Test]
    public void ChangeEnergy_WhenEnergyMAX_DoesNotChange()
    {
        var startingEnergy = AttributeValue.MAX;
        var pet = new CatPet("Joseph", startingEnergy);

        pet.ChangeEnergy(SINGLE_INCREMENT);

        Assert.That(pet.Energy, Is.EqualTo(startingEnergy));
    }

    [Test]
    public void ChangeEnergy_WhenHasEnergy_IncrementsEnergy()
    {
        _defaultCatPet.ChangeEnergy(SINGLE_INCREMENT);

        Assert.That(_defaultCatPet.Energy, Is.EqualTo(AttributeValue.DEFAULT + SINGLE_INCREMENT));
    }

    [TestCase(2, 5, 7)]
    [TestCase(5, 5, 10)]
    [TestCase(8, 1, 9)]
    [TestCase(9, 1, 10)]
    [TestCase(10, -1, 9)]
    [TestCase(7, -3, 4)]
    [TestCase(9, -6, 3)]
    [TestCase(9, -9, 0)]
    public void ChangeEnergy_WhenCalledWithValue_CanChangeByValue(int startingEnergy, int change, int expected)
    {
        var pet = new CatPet("Kitty", startingEnergy);

        pet.ChangeEnergy(change);

        Assert.That(pet.Energy, Is.EqualTo(expected));
    }

    [TestCase(4, -6)]
    [TestCase(10, -12)]
    [TestCase(1, -6)]
    [TestCase(3, -5)]
    public void ChangeEnergy_WhenNegativeChangeAmountMoreThanEnergy_EnergyDoesNotBecomeNegative(int startingEnergy, int change)
    {
        var pet = new CatPet("Kitty", startingEnergy);

        pet.ChangeEnergy(change);

        Assert.That(pet.Energy, Is.EqualTo(AttributeValue.MIN));
    }

    [TestCase(4, 8)]
    [TestCase(9, 4)]
    [TestCase(1, 13)]
    [TestCase(3, 8)]
    public void ChangeEnergy_WhenPositveChangeAmountMoreThanDeficit_EnergyDoesNotExceedMax(int startingEnergy, int change)
    {
        var pet = new CatPet("Kitty", startingEnergy);

        pet.ChangeEnergy(change);

        Assert.That(pet.Energy, Is.EqualTo(AttributeValue.MAX));
    }

    [Test]
    public void ChangeHunger_WhenHungerMax_DoesNotChange()
    {
        var startingHunger = AttributeValue.MAX;
        var pet = new CatPet("Joseph", AttributeValue.MEDIUM, startingHunger);

        pet.ChangeHunger(SINGLE_INCREMENT);

        Assert.That(pet.Hunger, Is.EqualTo(AttributeValue.MAX));
    }

    [Test]
    public void ChangeHunger_WhenHasHunger_IncrementsHunger()
    {
        _defaultCatPet.ChangeHunger(SINGLE_INCREMENT);

        Assert.That(_defaultCatPet.Hunger, Is.EqualTo(AttributeValue.DEFAULT + SINGLE_INCREMENT));
    }

    [TestCase(2, 5, 7)]
    [TestCase(5, 5, 10)]
    [TestCase(8, 1, 9)]
    [TestCase(9, 1, 10)]
    [TestCase(10, -1, 9)]
    [TestCase(7, -3, 4)]
    [TestCase(9, -6, 3)]
    [TestCase(9, -9, 0)]
    public void ChangeHunger_WhenCalledWithValue_CanChangeByValue(int startingHunger, int change, int expected)
    {
        var pet = new CatPet("Kitty", AttributeValue.MEDIUM, startingHunger);

        pet.ChangeHunger(change);

        Assert.That(pet.Hunger, Is.EqualTo(expected));
    }

    [TestCase(4, -6)]
    [TestCase(10, -12)]
    [TestCase(1, -6)]
    [TestCase(3, -5)]
    public void ChangeHunger_WhenNegativeChangeAmountMoreThanHunger_HungerDoesNotBecomeNegative(int startingHunger, int change)
    {
        var pet = new CatPet("Kitty", AttributeValue.MEDIUM, startingHunger);

        pet.ChangeHunger(change);

        Assert.That(pet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [TestCase(4, 8)]
    [TestCase(9, 4)]
    [TestCase(1, 13)]
    [TestCase(3, 8)]
    public void ChangeHunger_WhenPositveChangeAmountMoreThanDeficit_HungerDoesNotExceedMax(int startingHunger, int change)
    {
        var pet = new CatPet("Kitty", AttributeValue.MEDIUM, startingHunger);

        pet.ChangeHunger(change);

        Assert.That(pet.Hunger, Is.EqualTo(AttributeValue.MAX));
    }

    [Test]
    public void ChangeHappiness_WhenHappinessMAX_DoesNotChange()
    {
        var startingHappiness = AttributeValue.MAX;
        var pet = new CatPet("Joseph", AttributeValue.DEFAULT, AttributeValue.DEFAULT, startingHappiness);

        pet.ChangeHappiness(SINGLE_INCREMENT);

        Assert.That(pet.Happiness, Is.EqualTo(startingHappiness));
    }

    [Test]
    public void ChangeHappiness_WhenHasHappiness_IncrementsHappiness()
    {
        _defaultCatPet.ChangeHappiness(SINGLE_INCREMENT);

        Assert.That(_defaultCatPet.Happiness, Is.EqualTo(AttributeValue.DEFAULT + SINGLE_INCREMENT));
    }

    [TestCase(2, 5, 7)]
    [TestCase(5, 5, 10)]
    [TestCase(8, 1, 9)]
    [TestCase(9, 1, 10)]
    [TestCase(10, -1, 9)]
    [TestCase(7, -3, 4)]
    [TestCase(9, -6, 3)]
    [TestCase(9, -9, 0)]
    public void ChangeHappiness_WhenCalledWithValue_CanChangeByValue(int startingHappiness, int change, int expected)
    {
        var pet = new CatPet("Kits", AttributeValue.DEFAULT, AttributeValue.DEFAULT, startingHappiness);

        pet.ChangeHappiness(change);

        Assert.That(pet.Happiness, Is.EqualTo(expected));
    }

    [TestCase(4, -6)]
    [TestCase(10, -12)]
    [TestCase(1, -6)]
    [TestCase(3, -5)]
    public void ChangeHappiness_WhenNegativeChangeAmountMoreThanHappiness_HappinessDoesNotBecomeNegative(int startingHappiness, int change)
    {
        var pet = new CatPet("Kits", AttributeValue.DEFAULT, AttributeValue.DEFAULT, startingHappiness);

        pet.ChangeHappiness(change);

        Assert.That(pet.Happiness, Is.EqualTo(AttributeValue.MIN));
    }

    [TestCase(4, 8)]
    [TestCase(9, 4)]
    [TestCase(1, 13)]
    [TestCase(3, 8)]
    public void ChangeHappiness_WhenPositveChangeAmountMoreThanDeficit_HappinessDoesNotExceedMax(int startingHappiness, int change)
    {
        var pet = new CatPet("CatFish", AttributeValue.DEFAULT, AttributeValue.DEFAULT, startingHappiness);

        pet.ChangeHappiness(change);

        Assert.That(pet.Happiness, Is.EqualTo(AttributeValue.MAX));
    }
}
