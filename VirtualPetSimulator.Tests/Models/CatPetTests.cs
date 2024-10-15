using NUnit.Framework;
using VirtualPetSimulator.Models;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    [Test]
    public void Eat_WhenNotHungry_DoesNotEat()
    {
        var hasFullEnergy = 10;
        var isNotHungry = 0;
        var pet = new CatPet("Simon", hasFullEnergy, isNotHungry);

        pet.Eat();

        Assert.That(pet.Hunger, Is.EqualTo(isNotHungry));
    }

    [Test]
    public void Eat_WhenHasHunger_DecrementsHunger()
    {
        var hasFullEnergy = 10;
        var mediumHunger = 5;
        var pet = new CatPet("Simon", hasFullEnergy, mediumHunger);

        pet.Eat();

        Assert.That(pet.Hunger, Is.EqualTo(mediumHunger - 1));
    }
}
