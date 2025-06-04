
namespace Application.Common.Interface.Presentation
{
    public interface IUserInteractionService
    {
        string GetInput(string message);
        int GetValidatedIntegerMaxMin(string message, int minValue, int maxValue);
        void ShowMessage(string message);
    }
}