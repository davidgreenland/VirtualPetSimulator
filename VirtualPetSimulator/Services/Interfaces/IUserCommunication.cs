using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    string ActivityMessage { get; set; }
    void RenderScreen(IPet pet);
    char GetUserChoice(string prompt);
    Task ShowProgress(Task task);
    void ClearScreen();
    void WaitForUser();
    void ShowMessage(string message);
}
