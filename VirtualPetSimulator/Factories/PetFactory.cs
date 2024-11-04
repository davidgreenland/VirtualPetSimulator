using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Actions;
using System.ComponentModel;
using VirtualPetSimulator.Models.Enums;

namespace VirtualPetSimulator.Factories;

public class PetFactory
{
    public IPet CreatePet(PetType petType, string name)
    {
        switch (petType)
        {
            case PetType.Cat:
                return new CatPet(name, new MeowAction());
            case PetType.Bear:
                return new BearPet(name, new MeowAction());
            default:
                throw new InvalidEnumArgumentException($"Pet Type {petType} is not implemented yet");
        }
    }
}
