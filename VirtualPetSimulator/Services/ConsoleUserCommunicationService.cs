﻿using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;
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
        RenderAttributes(pet);
        RenderImage(pet);
        ShowMessage(DisplayMessage);
    }

    public string ReadInput(string prompt)
    {
        string? input;

        do
        {
            Console.Write(prompt);
            input = Console.ReadLine();
        }
        while (string.IsNullOrEmpty(input));

        return input;
    }

    public void RenderAttributes(IPet pet)
    {
        int cursorXPosition = Console.CursorLeft;
        int cursorYPosition = Console.CursorTop;

        Console.SetCursorPosition(0, 0);
        Console.Write("Energy: ");
        PrintColor($"{new string('#', pet.Energy)}{new string(' ', HEADER_SPACER - pet.Energy)}", ConsoleColor.Red);
        Console.Write("Hunger: ");
        PrintColor($"{new string('#', pet.Hunger)}{new string(' ', HEADER_SPACER - pet.Hunger)}", ConsoleColor.Red);
        Console.Write("Happiness: ");
        PrintColor($"{new string('#', pet.Happiness)}", ConsoleColor.Red);
        Console.WriteLine("\n");
        Console.SetCursorPosition(Math.Max(cursorXPosition, Console.CursorLeft), Math.Max(cursorYPosition, Console.CursorTop));
    }

    public void RenderImage(IPet pet)
    {
        if (pet.CurrentAction == PetAction.Sit)
        {
            PrintColor($"{_asciiArtService.GetAsciiForMood(pet.CurrentMood)}\n", ConsoleColor.DarkGreen);
        }
        else
        {
            PrintColor($"{_asciiArtService.GetAsciiForAction((pet.CurrentAction))}\n", ConsoleColor.DarkGreen);
        }
    }

    private void PrintColor(string characters, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(characters);
        Console.ResetColor();
    }

    public async void DisplaySound(IPet pet)
    {
        int cursorXPosition = Console.CursorLeft;
        int cursorYPosition = Console.CursorTop;
        int soundCursorX = 20;
        int soundCursorY = 4;

        if (pet.CurrentAction == PetAction.Sit)
        {
            Console.SetCursorPosition(soundCursorX, soundCursorY);
            Console.Write(pet.PerformSound());
            Console.SetCursorPosition(cursorXPosition, cursorYPosition);
        }

        await _timeService.WaitForOperation(3000, new CancellationTokenSource().Token);

        if (pet.CurrentAction == PetAction.Sit)
        {
            Console.SetCursorPosition(soundCursorX, soundCursorY);
            Console.Write(new string(' ', pet.PerformSound().Length));
            Console.SetCursorPosition(cursorXPosition, cursorYPosition);
        }
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
            if (action == "Exit")
            {
                options += "E[x]it";
                continue;
            }
            if (action != "Sit")
            {
                var actionKey = action[0];
                options += $"[{actionKey}]{action.Substring(1)}\n";
            }

        }

        return options;
    }

    public string GetPetChoices()
    {
        string petChoices = "";
        foreach (var pet in Enum.GetNames(typeof(PetType)))
        {
            var actionKey = pet[0];
            petChoices += $"[{actionKey}]{pet.Substring(1)}\n";

        }

        return petChoices;
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

            if (!task.IsCompleted)
            {
                Console.Write(".");
            }
        }
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

    public void ListenForKeyStroke(CancellationTokenSource tokenSource, Task operation)
    {
        Console.WriteLine("Press spacebar to wake them");
        ConsoleKeyInfo consoleKey;
        do
        {
            consoleKey = Console.ReadKey();
            if (consoleKey.Key == ConsoleKey.Spacebar)
            {
                tokenSource.Cancel();
            }
        } while ((consoleKey.Key != ConsoleKey.Spacebar && !operation.IsCompleted));
    }
}