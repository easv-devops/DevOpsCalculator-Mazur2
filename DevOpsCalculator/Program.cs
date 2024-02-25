using DevOpsCalculator;
using Microsoft.EntityFrameworkCore;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseNpgsql("Host=db;Port=5432;Database=calc;Username=root;Password=password");
var dbContext = new AppDbContext(optionsBuilder.Options);
dbContext.Database.EnsureCreated();
var calculator = new Calculator();
var calculationService = new CalculationService(dbContext, calculator);

var input = WriteWelcomeScreen();
while (input != "Q")
{
    if (input == "H")
    {
        calculationService.ShowHistory();
        continue;
    }

    double n1, n2;
    // Get the numbers
    Console.WriteLine("Enter the first number:");
    n1 = double.Parse(Console.ReadLine() ?? string.Empty);
    Console.WriteLine("Enter the second number:");
    n2 = double.Parse(Console.ReadLine() ?? string.Empty);

    calculationService.PerformCalculation(n1, n2, input?.ToUpper() ?? "");

    input = WriteWelcomeScreen();
}

string? WriteWelcomeScreen()
{
    Console.WriteLine(
        """
        --------------------------------------
        | Welcome to the console calculator! |
        --------------------------------------

        Select one of the options by writing character:
            A - Addition
            S - Substraction
            M - Multiply
            D - Divide

            H - History
            Q - Quit
        Your input:
        """);

    return Console.ReadLine();
}

#region Data

public class AppDbContext : DbContext
{
    public DbSet<CalculationRecord> CalculationRecords { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}

public class CalculationRecord
{
    public int Id { get; set; }
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }
    public string Operator { get; set; }
    public double Result { get; set; }

    public CalculationRecord() { }
    public CalculationRecord(double n1, double n2, string oper, double result)
    {
        this.Operand1 = n1;
        this.Operand2 = n2;
        this.Operator = oper;
        this.Result = result;
    }
}

#endregion