
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Actions.SoundBehaviours;
using System.ComponentModel;

namespace VirtualPetSimulator.Factories;

public class PetFactory
{
    public IPet CreatePet(PetType petType, string name)
    {
        switch (petType)
        {
            case PetType.Cat:
                return new CatPet(name, new Meow());
            default:
                throw new InvalidEnumArgumentException($"Pet Type {petType} is not implemented yet");
        }
    }
}
