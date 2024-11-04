using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;

namespace VirtualPetSimulator.Services.Interfaces
{
    public interface IAsciiArtService
    {
        string GetAsciiForAction(PetAction action);
        string GetAsciiForMood(PetMood petMood);
    }
}