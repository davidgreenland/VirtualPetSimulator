using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class StartUpAsciiArtService : IAsciiArtService
{
    public string GetAsciiForAction(PetAction action)
    {
        return "\nPet images not selected yet\n";
    }

    public string GetAsciiForMood(PetMood petMood)
    {
        return "\nPet images not selected yet\n";
    }

}
