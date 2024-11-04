using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services.Interfaces;

public interface IUserCommunication
{
    void SetArtService(IAsciiArtService asciiArtService);
    void SetDisplayMessageToOptions();
    void SetDisplayMessage(string message);
    void RenderScreen(IPet pet);
    void RenderAttributes(IPet pet);
    void RenderImage(IPet pet);
    void DisplaySoundAsync(IPet pet);
    string ReadInput(string input);
    char GetUserChoice();
    string GetOptions(Type options);
    Task ShowProgressAsync(Task task);
    void ClearScreen();
    void WaitForUser();
    void ShowMessage(string message);
    void ListenForKeyStroke(CancellationTokenSource tokenSource, Task operation);
}