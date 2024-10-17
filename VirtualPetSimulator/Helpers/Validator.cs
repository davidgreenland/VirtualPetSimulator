using VirtualPetSimulator.Helpers.Interfaces;

namespace VirtualPetSimulator.Helpers;

public class Validator : IValidator
{
    public void ValidateNonNegative(int input, string paramName)
    {
        if (input < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be greater than zero");
        }
    }
}
