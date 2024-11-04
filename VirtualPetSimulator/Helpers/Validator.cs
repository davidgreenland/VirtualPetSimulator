using VirtualPetSimulator.Helpers.Interfaces;

namespace VirtualPetSimulator.Helpers;

public class Validator : IValidator
{
    public bool IsNonNegative(int input, string paramName)
    {
        if (input < 0)
        {
            Console.WriteLine((paramName, $"{paramName} must be greater than zero"));
            return false;
        }

        return true;
    }
}
