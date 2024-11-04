using System.ComponentModel;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Services.AsciiArt;

namespace VirtualPetSimulator.Factories
{
    public class AsciiArtServiceFactory
    {
        public IAsciiArtService GetService(PetType petType) 
        {
            switch (petType)
            {
                case PetType.Cat:
                    return new CatAsciiArtService();
                case PetType.Bear:
                    return new BearAsciiArtService();
                default:
                    throw new InvalidEnumArgumentException($"PetType {petType} is not implemented yet");
            }
        }
    }
}
