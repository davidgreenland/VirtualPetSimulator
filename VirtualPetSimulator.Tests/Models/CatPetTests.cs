using NUnit.Framework;
using VirtualPetSimulator.Models;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    [Test]
    public void Eat_WhenNotHungry_DoesNotEat()
    {
        var hasFullEnergy = 10;
        var isFull = 0;
        var pet = new CatPet("Simon", hasFullEnergy, isFull);

        pet.Eat();

        Assert.That(pet.Hunger, Is.EqualTo(isFull));
    }
}
