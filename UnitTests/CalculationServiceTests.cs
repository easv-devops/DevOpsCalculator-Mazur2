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

    [Fact]
    public void TestPerformCalculation_Addition()
    {
        //Arrange
        double n1 = 1;
        double n2 = 2;

        //Act
        var result = _calculationService.PerformCalculation(n1, n2, "A");

        //Assert
        Assert.Equal(3, result);

        //Check that a record has been added to the DbContext
        var record = _dbContext.CalculationRecords.Single(); //this throws if there isn't exactly one record
        Assert.Equal(n1, record.Operand1);
        Assert.Equal(n2, record.Operand2);
        Assert.Equal("A", record.Operator);
        Assert.Equal(3, record.Result);
    }
}