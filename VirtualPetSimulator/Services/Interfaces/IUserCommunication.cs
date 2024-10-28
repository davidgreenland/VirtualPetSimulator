using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    public void SetDisplayMessageToOptions();

    public void SetDisplayMessage(string message);
    void RenderScreen(IPet pet);
    void RenderAttributes(IPet pet);
    char GetUserChoice(string prompt);
    Task ShowProgress(Task task);
    void ClearScreen();
    void WaitForUser();
    void ShowMessage(string message);
}