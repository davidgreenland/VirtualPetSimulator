namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    Task RunOperation(int repetitions, string message, string image);
    string ActivityMessage { get; set; }
    void RenderScreen();
    char GetUserChoice(string prompt);
    Task ShowProgress(Task task);
    void ClearScreen();
    void WaitForUser();
    void ShowMessage(string message);
}
