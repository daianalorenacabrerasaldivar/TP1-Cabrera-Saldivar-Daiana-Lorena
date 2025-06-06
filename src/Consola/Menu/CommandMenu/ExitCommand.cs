using Application.Common.Interface.Presentation;

namespace Consola.Menu.CommandMenu
{
    internal class ExitCommand : IMenuOptionCommand
    {
        public string Name => "Salir";

        IConsoleUserInteractionService _consoleDisplay;
        public ExitCommand(IConsoleUserInteractionService consoleDisplay)
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