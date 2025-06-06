using Application.Common.Interface.Presentation;

namespace Consola.Menu
{
    public class MenuManagerHandler
    {
        private readonly IConsoleUserInteractionService _userInteractionService;
        private readonly IMenuFactory _menuFactory;
        private readonly Dictionary<string, IMenuOptionCommand> _menuOptions;

        public MenuManagerHandler(
            IConsoleUserInteractionService userInteractionService,
            IMenuFactory menuFactory)
        {
            _userInteractionService = userInteractionService ?? throw new ArgumentNullException(nameof(userInteractionService));
            _menuFactory = menuFactory ?? throw new ArgumentNullException(nameof(menuFactory));
            _menuOptions = _menuFactory.CreateMenuDictionary();
        }

        public async Task RunMenuAsync()
        {
            var userLogin = _menuFactory.Login();
             await  userLogin.ExecuteAsync();
            while (true)
            {
                DisplayMenu();
                string option = _userInteractionService.GetInput("Por favor, selecciona una opción:").Trim();

                if (_menuOptions.TryGetValue(option, out var command))
                {
                    _userInteractionService.ConleClear();
                    await ExecuteCommandAsync(command);
                }
                else
                {
                    _userInteractionService.ShowMessage("Opción no válida. Por favor, intente nuevamente.");
                }
            }
        }

        private void DisplayMenu()
        {
            _userInteractionService.ConleClear();
            _userInteractionService.ShowCustomBar();
            _userInteractionService.ShowMessage("--- Menú Principal ---");
            _userInteractionService.ShowMessage("Seleccione una opción:");

            foreach (var option in _menuOptions)
            {
                _userInteractionService.ShowMessage($"{option.Key}. {option.Value.Name}");
            }

            _userInteractionService.ShowCustomBar();
        }


        private async Task ExecuteCommandAsync(IMenuOptionCommand command)
        {
            try
            {
                _userInteractionService.ShowMessage($"\n--- {command.Name} ---\n");
                await command.ExecuteAsync();

                // No esperamos Enter después del comando de salir
                if (command.GetType().Name != "ExitCommand")
                {
                    _userInteractionService.ShowMessage("\nPresione Enter para volver al menú principal...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                _userInteractionService.ShowMessage($"Error al ejecutar el comando: {ex.Message}");
                _userInteractionService.ShowMessage("\nPresione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }
}