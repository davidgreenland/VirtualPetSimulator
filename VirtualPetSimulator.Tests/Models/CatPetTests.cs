using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;
using Moq;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Helpers.Interfaces;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private Mock<ITimeService> _timeServiceMock;
    private Mock<IValidator> _validatorMock;
    private CatPet _testCatPet;
    private const int DEFAULT_INCREMENT = 1;

    [SetUp]
    public void SetUp()
    {
        _timeServiceMock = new Mock<ITimeService>();
        _validatorMock = new Mock<IValidator>();

        _timeServiceMock.Setup(mock => mock.RunOperation(It.IsAny<int>())).Returns(Task.CompletedTask);
        _testCatPet = new CatPet(_timeServiceMock.Object, _validatorMock.Object, "Simon");
    }

    [Test]
    public async Task Eat_WhenNotHungry_DoesNotEat()
    {
        var notHungryPet = new CatPet(_timeServiceMock.Object, _validatorMock.Object, "Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);

        var portionsEaten = notHungryPet.Eat();

        Assert.That(notHungryPet.Hunger, Is.EqualTo(AttributeValue.MIN));
        Assert.That(await portionsEaten, Is.EqualTo(0));
        _timeServiceMock.Verify(x => x.RunOperation(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Eat_WhenHasHunger_DecrementsHunger()
    {
        await _testCatPet.Eat();

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.DEFAULT - DEFAULT_INCREMENT));
    }

    [Test]
    public async Task Eat_WhenGivenFoodValue_CanDecrementHungerByMoreThanDefault()
    {
        var whiskas = 4;

        await _testCatPet.Eat(whiskas);

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.DEFAULT - whiskas));
    }

    [Test]
    public async Task Eat_WhenFoodAmountMoreThanHunger_HungerDoesNotBecomeNegative()
    {
        var catFood = 3;

        await Task.WhenAll(_testCatPet.Eat(catFood), _testCatPet.Eat(catFood), _testCatPet.Eat(catFood), _testCatPet.Eat(catFood));

        Assert.That(_testCatPet.Hunger, Is.EqualTo(AttributeValue.MIN));
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(4, 4)]
    [TestCase(7, 6)]
    [TestCase(11, 6)]
    [TestCase(23, 6)]
    public async Task Eat_WhenFeedingDefaultPet_ReturnsCorrectPortionsEaten(int foodValue, int expected)
    {
        var portionsEaten = _testCatPet.Eat(foodValue);

        Assert.That(await portionsEaten, Is.EqualTo(expected));
    }

    [TestCase(3)]
    [TestCase(-4)]
    [TestCase(108)]
    public async Task Eat_WhenCalled_CallsValidatorWithValue(int foodValue)
    {
        await _testCatPet.Eat(foodValue);

        _validatorMock.Verify(x => x.ValidateNonNegative(It.Is<int>(val => val == foodValue), It.IsAny<string>()), Times.Once());
    }

    [Test]
    public async Task Sleep_WhenNotTired_DoesNotSleep()
    {
        var maxEnergyCat = new CatPet(_timeServiceMock.Object, _validatorMock.Object, "Joseph", energy: AttributeValue.MAX, AttributeValue.MIN);

        await maxEnergyCat.Sleep();

        Assert.That(maxEnergyCat.Energy, Is.EqualTo(AttributeValue.MAX));
        _timeServiceMock.Verify(x => x.RunOperation(It.IsAny<int>()), Times.Never());
    }

    [Test]
    public async Task Sleep_WhenEnergyNotMax_IncrementsEnergy()
    {
        await _testCatPet.Sleep();

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.DEFAULT + DEFAULT_INCREMENT));
    }

    [Test]
    public async Task Sleep_WhenGivenSleepValue_CanIncrementEnergyByMoreThanDefault()
    {
        var sleepValue = 3;

        await _testCatPet.Sleep(sleepValue);

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.DEFAULT + sleepValue));
    }

    [Test]
    public async Task Sleep_WhenSleepValueWillIncreaseEnergyBeyondMax_EnergyLimitsToMaximum()
    {
        var sleepValue = 5;

        await Task.WhenAll(_testCatPet.Sleep(sleepValue), _testCatPet.Sleep(sleepValue));

        Assert.That(_testCatPet.Energy, Is.EqualTo(AttributeValue.MAX));
    }

    [TestCase(1, 1)]
    [TestCase(2, 2)]
    [TestCase(4, 4)]
    [TestCase(7, 4)]
    [TestCase(11, 4)]
    [TestCase(23, 4)]
    public async Task Sleep_WhenDefaultPetSleeps_ReturnsCorrectAmountSlept(int sleepValue, int expected)
    {
        var amountSlept = _testCatPet.Sleep(sleepValue);

        Assert.That(await amountSlept, Is.EqualTo(expected));
    }

    [TestCase(-35)]
    [TestCase(4)]
    [TestCase(18)]
    public async Task Sleep_WhenCalled_CallsValidatorWithValue(int sleepValue)
    {
        await _testCatPet.Sleep(sleepValue);

        _validatorMock.Verify(x => x.ValidateNonNegative(It.Is<int>(val => val == sleepValue), It.IsAny<string>()), Times.Once());
    }

    [Test]
    public void Play_WhenCalled_IncrementsHappiness()
    {
        _testCatPet.Play();

        Assert.That(_testCatPet.Happiness, Is.EqualTo(AttributeValue.DEFAULT + DEFAULT_INCREMENT));
    }

    [Test]
    public void Play_WhenUsedRepeatedly_DoesNotIncreaseHappinessToMoreThanMax()
    {
        for (var i = 0; i < 10; i++)
        {
            _testCatPet.Play();
        }

        Assert.That(_testCatPet.Happiness, Is.EqualTo(AttributeValue.MAX));
    }

    [Test] 
    public void Play_WhenGivenExcessPlay_CanIncrementMoreThanDefault()
    {
        var veryFunGame = 4; 
        
        _testCatPet.Play(veryFunGame);

        Assert.That(_testCatPet.Happiness, Is.EqualTo(AttributeValue.DEFAULT + veryFunGame));
    }

    [Test]
    public void Play_WhenHappinessAtOrBelowThreshold_PetDoesNotPlay()
    {
        var sadPet = new CatPet(_timeServiceMock.Object, _validatorMock.Object, "Misererio", AttributeValue.DEFAULT, AttributeValue.DEFAULT, AttributeValue.HAPPINESS_PLAY_THRESHOLD);
    }
}
