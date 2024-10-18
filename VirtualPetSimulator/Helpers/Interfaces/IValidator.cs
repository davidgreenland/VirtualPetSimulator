namespace VirtualPetSimulator.Helpers.Interfaces;

public interface IValidator
{
    bool IsNonNegative(int input, string paramName);
}