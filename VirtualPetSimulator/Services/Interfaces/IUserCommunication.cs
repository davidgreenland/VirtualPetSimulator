using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    public void SetDisplayMessageToOptions();
    public void SetDisplayMessage(string message);
    void RenderScreen(IPet pet);
    void RenderAttributes(IPet pet);
    void RenderImage(IPet pet);
    void DisplaySound(IPet pet);
    string ReadInput(string input);
    char GetUserChoice(string prompt);
    string GetPetChoices();
    Task ShowProgress(Task task);
    void ClearScreen();
    void WaitForUser();
    void ShowMessage(string message);
    void ListenForKeyStroke(CancellationTokenSource tokenSource, Task operation);
}