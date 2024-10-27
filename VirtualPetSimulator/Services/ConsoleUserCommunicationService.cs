using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services
{
    public class ConsoleUserCommunicationService : IUserCommunication
    {
        private readonly IPet _pet;
        private readonly ITimeService _timeService;
        private const int HEADER_SPACER = 15;
        private CancellationTokenSource _cts = new CancellationTokenSource();

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
            return _timeService.WaitForOperation(delay);
        }

        public void RenderScreen()
        {
            ClearScreen();
            UpdateEnergyDisplay();
            Console.Write($"Hunger: {new string('#', _pet.Hunger)}{new string(' ', HEADER_SPACER - _pet.Hunger)}");
            Console.Write($"Happiness: {new string('#', _pet.Happiness)}\n\n");
            Console.WriteLine($"{_pet.GetAsciiArt()}\n");
            Console.WriteLine(ActivityMessage);
        }

        public char GetUserChoice(string prompt)
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => StartEnergyUpdater(_cts.Token));

            Console.Write(prompt);
            var input = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine(Environment.NewLine);

            _cts.Cancel(); // Stop the background updater
            return input;
        }

        private void UpdateEnergyDisplay()
        {
            _pet.ChangeEnergy(-1); // Example energy decrease for demonstration

            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;
            Console.SetCursorPosition(0, 0);
            Console.Write($"Energy: {new string('#', _pet.Energy)}{new string(' ', HEADER_SPACER - _pet.Energy)}");
            
            // Restore the original cursor position
            Console.SetCursorPosition(currentLeft, currentTop);
        }

        private async Task StartEnergyUpdater(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                UpdateEnergyDisplay();
                await Task.Delay(1000); // Update every second
            }
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
            if (!Console.IsOutputRedirected) 
            {
                Console.Clear();
            }
        }

        public void WaitForUser() => Console.ReadKey();

        public void ShowMessage(string message) => Console.WriteLine(message);
    }
}