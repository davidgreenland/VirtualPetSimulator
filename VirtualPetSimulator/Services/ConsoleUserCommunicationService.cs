using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunicationService : IUserCommunication
{
    private readonly IPet _pet;
    private readonly ITimeService _timeService;
    private const int HEADER_SPACER = 15;
    public string ActivityMessage { get; set; } = string.Empty;

    public ConsoleUserCommunicationService(IPet pet, ITimeService timeService)
    {
        _pet = pet;
        _timeService = timeService;
    }

    public Task RunOperation(int repetitions, string message, string image)
    {
        ActivityMessage = message;
        var delay = repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS;
        var operation = _timeService.WaitForOperation(delay);

        return operation;
    }

    public void RenderScreen()
    {
        ClearScreen();
        Console.Write($"Energy: {new string('#', _pet.Energy)}{new string(' ', HEADER_SPACER - _pet.Energy)}");
        Console.Write($"Hunger: {new string('#', _pet.Hunger)}{new string(' ', HEADER_SPACER - _pet.Hunger)}");
        Console.Write($"Happiness: {new string('#', _pet.Happiness)}\n\n");
        Console.WriteLine($"{_pet.GetAsciiArt()}\n");
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
        RenderScreen();

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