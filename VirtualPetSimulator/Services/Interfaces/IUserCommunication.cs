namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    Task RunOperation(int repetitions, string message);
    void ShowMessage(string message);
}
