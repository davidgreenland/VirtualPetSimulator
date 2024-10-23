using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    Task RunOperation(int repetitions, string message, string image);
    void ShowMessage(string message);
}
