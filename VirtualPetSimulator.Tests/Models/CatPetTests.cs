using NUnit.Framework;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Helpers;
using Moq;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Tests.Models;

public class CatPetTests
{
    private Mock<ITimeService> _timeServiceMock;
    private CatPet _testCatPet;
    private const int DEFAULT_INCREMENT = 1;

    [SetUp]
    public void SetUp()
    {
        _timeServiceMock = new Mock<ITimeService>();

        _timeServiceMock.Setup(mock => mock.Delay(It.IsAny<int>())).Returns(Task.CompletedTask);
        _testCatPet = new CatPet(_timeServiceMock.Object, "Simon");
    }

    [Test]
    public async Task Eat_WhenNotHungry_DoesNotEat()
    {
        var notHungryPet = new CatPet(_timeServiceMock.Object, "Joseph", AttributeValue.MEDIUM, AttributeValue.MIN);

        var portionsEaten = notHungryPet.Eat();

        Assert.That(notHungryPet.Hunger, Is.EqualTo(AttributeValue.MIN));
        Assert.That(await portionsEaten, Is.EqualTo(0));
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
    public async Task Eat_WhenFeedingDefaultPet_ReturnsPortionsEaten(int foodValue, int expected)
    {
        var portionsEaten = _testCatPet.Eat(foodValue);

        Assert.That(await portionsEaten, Is.EqualTo(expected));
    }

    [Test]
    public async Task Sleep_WhenNotTired_DoesNotSleep()
    {
        var maxEnergyCat = new CatPet(_timeServiceMock.Object, "Joseph", energy: AttributeValue.MAX, AttributeValue.MIN);

        await maxEnergyCat.Sleep();

        Assert.That(maxEnergyCat.Energy, Is.EqualTo(AttributeValue.MAX));
        _timeServiceMock.Verify(x => x.Delay(It.IsAny<int>()), Times.Never());
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
    public void Play_WhenGivenPlayValue_CanIncrementMoreThanDefault()
    {
        var veryFunGame = 4; 
        
        _testCatPet.Play(veryFunGame);

        Assert.That(_testCatPet.Happiness, Is.EqualTo(AttributeValue.DEFAULT + veryFunGame));
    }
}
