using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Services.Interfaces
{
    public interface IAsciiArtService
    {
        IDictionary<PetAction, string> _actionImages { get; }

        string GetAsciiForAction(PetAction action);
    }
}