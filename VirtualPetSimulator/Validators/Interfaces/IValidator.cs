namespace VirtualPetSimulator.Validators.Interfaces;

public interface IValidator
{
    bool Validate(int input, string paramName);
}