using Application.Common.Interface.Presentation;
using System;

public class ConsoleUserInteractionService : IConsoleUserInteractionService
{
    public string GetInput(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine();
    }
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public int GetValidatedIntegerMaxMin(string message, int minValue, int maxValue)
    {
        int result;
        do
        {
            ShowMessage(message);
            var input = Console.ReadLine();

            if (!int.TryParse(input, out result) || result < minValue || result > maxValue)
            {
                ShowMessage($"Error: Ingrese un número válido entre {minValue} y {maxValue}.");
                continue;
            }

            break;
        } while (true);

        return result;
    }

    public decimal GetValidatedDecimalMaxMin(string message, decimal minValue, decimal maxValue)
    {
        decimal result;
        do
        {
            ShowMessage(message);
            var input = Console.ReadLine();
            if (!decimal.TryParse(input, out result) || result < minValue || result > maxValue)
            {
                ShowMessage($"Error: Ingrese un número válido entre {minValue} y {maxValue}.");
                continue;
            }
            break;
        } while (true);
        return result;
    }

    public void ShowCustomBar()
    {
        Console.WriteLine("\n===============================================\n");
    }

    public void ConleClear()
    {
        Console.Clear();
    }
}
