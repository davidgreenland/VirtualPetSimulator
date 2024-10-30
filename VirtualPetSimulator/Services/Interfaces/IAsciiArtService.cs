using VirtualPetSimulator.Actions.Enums;

namespace VirtualPetSimulator.Services.Interfaces
{
    public interface IAsciiArtService
    {
        IDictionary<PetAction, string> _actionImages { get; }

        string GetAsciiForAction(PetAction action);
    }
}