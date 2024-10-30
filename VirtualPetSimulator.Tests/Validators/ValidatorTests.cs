using NUnit.Framework;
using VirtualPetSimulator.Validators;

namespace VirtualPetSimulator.Tests.Validators;

public class ValidatorTests
{
    private Validator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new Validator();
    }

    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(-109)]
    [TestCase(-34567)]
    public void Validate_WhenGivenNegativeValue_IsInvalid(int negativeParam)
    {
        var paramName = "value";

        Assert.That(_validator.Validate(negativeParam, paramName), Is.EqualTo(false));
    }

    [TestCase(1)]
    [TestCase(4)]
    [TestCase(9)]
    [TestCase(490)]
    public void Validate_WhenGivenPositive_IsValid(int positiveParam)
    {
        var paramName = "value";

        Assert.That(_validator.Validate(positiveParam, paramName), Is.EqualTo(true));
    }
}
