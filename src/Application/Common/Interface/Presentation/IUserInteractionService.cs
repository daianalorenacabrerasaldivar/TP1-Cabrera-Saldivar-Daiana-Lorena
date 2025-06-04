
namespace Application.Common.Interface.Presentation
{
    public interface IUserInteractionService
    {
        void ShowMessage(string message);
        string GetInput(string message);
        int GetValidatedIntegerMaxMin(string message, int minValue, int maxValue);
        decimal GetValidatedDecimalMaxMin(string message, decimal minValue, decimal maxValue);
    }
}