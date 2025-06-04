using Application.Common.Interface.Presentation;

namespace ApprovalManagerConsole.Menu.Commands
{
    internal class ExitCommand : IMenuOptionCommand
    {
        public string Name => "Salir";
    
        IUserInteractionService _consoleDisplay;
        public ExitCommand(IUserInteractionService consoleDisplay)
        {
            _consoleDisplay = consoleDisplay;
        }


        public Task ExecuteAsync()
        {
            _consoleDisplay.ShowMessage("Salir");
            Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}