using DevOpsCalculator;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests;

[Collection("Calculator")]
public class CalculationServiceTests
{
    private CalculationService _calculationService;
    private AppDbContext _dbContext;

    public CalculationServiceTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "Test_Database");
        _dbContext = new AppDbContext(optionsBuilder.Options);
        _calculationService = new CalculationService(_dbContext, new Calculator());
    }

    [Theory]
    [InlineData(2, 2, "A", 4)]
    [InlineData(-2, 2, "A", 0)]
    [InlineData(0, 0, "A", 0)]
    [InlineData(-2, -2, "A", -4)]
    public void TestPerformCalculation_Addition(double n1, double n2, string operation, double expectedResult)
    {
        var result = _calculationService.PerformCalculation(n1, n2, operation);

        Assert.Equal(expectedResult, result);

        var record = _dbContext.CalculationRecords.Last();
        Assert.Equal(n1, record.Operand1);
        Assert.Equal(n2, record.Operand2);
        Assert.Equal(operation, record.Operator);
        Assert.Equal(expectedResult, record.Result);
    }

    [Theory]
    [InlineData(3, 2, "S", 1)]
    [InlineData(2, 3, "S", -1)]
    [InlineData(0, 0, "S", 0)]
    [InlineData(-2, -2, "S", 0)]
    public void TestPerformCalculation_Subtraction(double n1, double n2, string operation, double expectedResult)
    {
        var result = _calculationService.PerformCalculation(n1, n2, operation);

        Assert.Equal(expectedResult, result);

        var record = _dbContext.CalculationRecords.Last();
        Assert.Equal(n1, record.Operand1);
        Assert.Equal(n2, record.Operand2);
        Assert.Equal(operation, record.Operator);
        Assert.Equal(expectedResult, record.Result);
    }

    [Theory]
    [InlineData(2, 2, "M", 4)]
    [InlineData(0, 3, "M", 0)]
    [InlineData(-2, 2, "M", -4)]
    public void TestPerformCalculation_Multiplication(double n1, double n2, string operation, double expectedResult)
    {
        var result = _calculationService.PerformCalculation(n1, n2, operation);

        Assert.Equal(expectedResult, result);

        var record = _dbContext.CalculationRecords.Last();
        Assert.Equal(n1, record.Operand1);
        Assert.Equal(n2, record.Operand2);
        Assert.Equal(operation, record.Operator);
        Assert.Equal(expectedResult, record.Result);
    }
    
    [Theory]
    [InlineData(4, 2, "D", 2)]
    [InlineData(-2, 2, "D", -1)]
    public void TestPerformCalculation_Division(double n1, double n2, string operation, double expectedResult)
    {
        var result = _calculationService.PerformCalculation(n1, n2, operation);

        Assert.Equal(expectedResult, result);

        var record = _dbContext.CalculationRecords.Last();
        Assert.Equal(n1, record.Operand1);
        Assert.Equal(n2, record.Operand2);
        Assert.Equal(operation, record.Operator);
        Assert.Equal(expectedResult, record.Result);
    }
    
    [Theory]
    [InlineData(4, 2, "X", 2)]
    public void TestPerformCalculation_InvalidOperation(double n1, double n2, string operation, double expectedResult)
    {
        Exception ex = Assert.Throws<ArgumentException>(() => _calculationService.PerformCalculation(n1, n2, operation));

        Assert.Equal("Invalid operation: X", ex.Message);
    }
    
    [Fact]
    public void TestGetCalculationsHistory()
    {
        var record1 = new CalculationRecord(1, 2, "A", 3);
        var record2 = new CalculationRecord(2, 2, "A", 4);
        _dbContext.CalculationRecords.Add(record1);
        _dbContext.CalculationRecords.Add(record2);
        _dbContext.SaveChanges();

        var historyRecords = _calculationService.GetCalculationsHistory().Where(r => r.Id == record1.Id || r.Id == record2.Id).ToList();

        Assert.Equal(2, historyRecords.Count);
    
        Assert.Equal(record1.Id, historyRecords[0].Id);
        Assert.Equal(record1.Operand1, historyRecords[0].Operand1);
        Assert.Equal(record1.Operand2, historyRecords[0].Operand2);
        Assert.Equal(record1.Operator, historyRecords[0].Operator);
        Assert.Equal(record1.Result, historyRecords[0].Result);
    
        Assert.Equal(record2.Id, historyRecords[1].Id);
        Assert.Equal(record2.Operand1, historyRecords[1].Operand1);
        Assert.Equal(record2.Operand2, historyRecords[1].Operand2);
        Assert.Equal(record2.Operator, historyRecords[1].Operator);
        Assert.Equal(record2.Result, historyRecords[1].Result);
    }

    [Fact]
    public void TestShowHistory()
    {
        // Arrange
        var record1 = new CalculationRecord(1,2,"A",3);
        var record2 = new CalculationRecord(2,2,"A",4);
        _dbContext.CalculationRecords.Add(record1);
        _dbContext.CalculationRecords.Add(record2);
        _dbContext.SaveChanges();

        // Act
        var history = _calculationService.ShowHistory();

        // Assert
        Assert.Contains("ID: 1", history);
        Assert.Contains("ID: 2", history);
        // Assert other parts of the output as needed...
    }
}