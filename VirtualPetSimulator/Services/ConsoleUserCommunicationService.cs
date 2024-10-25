using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunicationService : IUserCommunication
{
    private readonly ITimeService _timeService;
    private readonly IAsciiArtService _asciiArtService;
    private const int HEADER_SPACER = 15;
    public string ActivityMessage { get; set; } = string.Empty;

    public ConsoleUserCommunicationService(ITimeService timeService, IAsciiArtService AsciiArtService)
    {
        _timeService = timeService;
        _asciiArtService = AsciiArtService;
    }

    public void RenderScreen(IPet pet)
    {
        ClearScreen();
        Console.Write($"Energy: {new string('#', pet.Energy)}{new string(' ', HEADER_SPACER - pet.Energy)}");
        Console.Write($"Hunger: {new string('#', pet.Hunger)}{new string(' ', HEADER_SPACER - pet.Hunger)}");
        Console.Write($"Happiness: {new string('#', pet.Happiness)}\n\n");
        Console.WriteLine($"{_asciiArtService.GetAsciiForAction((pet.CurrentAction))}\n");
        Console.WriteLine(ActivityMessage);
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