using DevOpsCalculator;
using Xunit;

namespace UnitTests;

[Collection("Calculator")]
public class CalculationTests
{
    private readonly ICalculator _calculator;

    public CalculationTests()
    {
        _calculator = new Calculator();
    }

    [Theory]
    [InlineData(2, 2, 4)]
    [InlineData(-2, 2, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(-2, -2, -4)]
    public void Add_ValidReturnsExpectedValue(double n1, double n2, double expected)
    {
        var result = _calculator.Add(n1, n2);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3, 2, 1)]
    [InlineData(-2, -3, 1)]
    [InlineData(0, 0, 0)]
    public void Subtract_ValidReturnsExpectedValue(double n1, double n2, double expected)
    {
        var result = _calculator.Subtract(n1, n2);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3, 2, 6)]
    [InlineData(-2, -3, 6)]
    [InlineData(0, 0, 0)]
    public void Multiply_ValidReturnsExpectedValue(double n1, double n2, double expected)
    {
        var result = _calculator.Multiply(n1, n2);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(6, 2, 3)]
    [InlineData(-6, -2, 3)]
    public void Divide_ValidReturnsExpectedValue(double n1, double n2, double expected)
    {
        var result = _calculator.Divide(n1, n2);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Divide_ByZeroThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(1, 0));
    }
}