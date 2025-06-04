using Application.Common.Interface.Presentation;

public class ConsoleUserInteractionService : IUserInteractionService
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
                ShowMessage($"Error: Ingrese un n�mero v�lido entre {minValue} y {maxValue}.");
                continue;
            }

            break;
        } while (true);

        return result;
    }
}
