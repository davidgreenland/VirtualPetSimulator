using NUnit.Framework;
using System.ComponentModel;
using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;

namespace VirtualPetSimulator.Tests.Factories;

public class PetFactoryTests
{
    [Test]
    public void CreatePet_WhenGivenATypeAndName_ShouldReturnCorrectPet()
    {
        var petFactory = new PetFactory();
        PetType type = PetType.Cat;
        string name = "Henry";

        var pet = petFactory.CreatePet(type, name);

        Assert.That(pet, Is.TypeOf<CatPet>());
    }

    [Test]
    public void CreatePet_WhenGivenInvalidType_ShouldThrowException()
    {
        var petFactory = new PetFactory();
        var invalidPetTypeEnum = 99;
        PetType unhandledType = (PetType) invalidPetTypeEnum;

        Assert.Throws<InvalidEnumArgumentException>(() => petFactory.CreatePet(unhandledType, "NameForTest"));
    }
}
