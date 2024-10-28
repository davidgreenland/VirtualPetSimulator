using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunicationService : IUserCommunication
{
    private readonly ITimeService _timeService;
    private readonly IAsciiArtService _asciiArtService;
    private const int HEADER_SPACER = 15;
    private readonly string _applicationOptions;
    public string DisplayMessage { get; private set; }

    public ConsoleUserCommunicationService(ITimeService timeService, IAsciiArtService asciiArtService)
    {
        _timeService = timeService;
        _asciiArtService = asciiArtService;
        _applicationOptions = GetApplicationOptions();
        DisplayMessage =  _applicationOptions;
    }

    public void RenderScreen(IPet pet)
    {
        ClearScreen();
        Console.Write($"Energy: {new string('#', pet.Energy)}{new string(' ', HEADER_SPACER - pet.Energy)}");
        Console.Write($"Hunger: {new string('#', pet.Hunger)}{new string(' ', HEADER_SPACER - pet.Hunger)}");
        Console.Write($"Happiness: {new string('#', pet.Happiness)}\n\n");
        Console.WriteLine($"{_asciiArtService.GetAsciiForAction((pet.CurrentAction))}\n");
        Console.WriteLine(DisplayMessage);
    }

    public void SetDisplayMessageToOptions()
    {
        DisplayMessage = _applicationOptions;
    }

    public void SetDisplayMessage(string message)
    {
        DisplayMessage = message;
    }

    private string GetApplicationOptions()
    {
        // todo: deal with null in pet actions dictionary
        string options = "";
        foreach (var action in Enum.GetNames(typeof(PetAction)))
        {
            if (action != "Sit")
            {
                var actionKey = action[0];
                options += $"[{actionKey}]{action.Substring(1)}\n";
            }
        }

        return options;
    }
    public char GetUserChoice(string prompt)
    {
        Console.Write(prompt);
        var input = char.ToUpper(Console.ReadKey().KeyChar);
        Console.WriteLine(Environment.NewLine);
        return input;
    }

    public async Task ShowProgress(Task task)
    {
        int interval = 100;

        while (!task.IsCompleted)
        {
            await _timeService.WaitForOperation(interval);

            Console.Write(".");
        }

        Console.WriteLine();
    }

    public void ClearScreen() 
    {
        // tests were failing without this conditional
        if (!Console.IsOutputRedirected) 
        {
            Console.Clear();
        } 
    }

    public void WaitForUser() => Console.ReadKey();

    public void ShowMessage(string message) => Console.WriteLine(message);
}