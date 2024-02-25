using System.Text;

namespace DevOpsCalculator;

public class CalculationService
{
    private readonly Calculator _calculator;
    private readonly AppDbContext _context;

    public CalculationService(AppDbContext context, Calculator calculator)
    {
        _context = context;
        _calculator = calculator;
    }

    public double PerformCalculation(double n1, double n2, string operation)
    {
        double result;

        switch(operation)
        {
            case "A":
                result = _calculator.Add(n1, n2);
                break;
            case "S":
                result = _calculator.Subtract(n1, n2);
                break;
            case "M":
                result = _calculator.Multiply(n1, n2);
                break;
            case "D":
                result = _calculator.Divide(n1, n2);
                break;
            default:
                throw new ArgumentException($"Invalid operation: {operation}");
        }

        _context.CalculationRecords.Add(new CalculationRecord(n1, n2, operation, result));
        _context.SaveChanges();

        return result;
    }

    public IEnumerable<CalculationRecord> GetCalculationsHistory()
    {
        return _context.CalculationRecords.ToList();
    }
    
    public string ShowHistory()
    {
        StringBuilder history = new StringBuilder();
        foreach (var record in _context.CalculationRecords)
        {
            history.AppendLine($"ID: {record.Id}, Operator1: {record.Operand1}, Operator2: {record.Operand2}, Operator: {record.Operator}, Result: {record.Result}");
        }
        return history.ToString();
    }
}