using Application.Common.Interface;
using Application.Common.Interface.Presentation;
using Domain.Entity;
using System.Text.RegularExpressions;

namespace Consola.Menu.CommandMenu.UserSesion
{
    internal class UserSessionCommand : IMenuOptionCommand
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IConsoleUserInteractionService _userInteractionService;

        public string Name => "Iniciar Sesión";

        public UserSessionCommand(
            IUserSessionService userSessionService,
            IConsoleUserInteractionService userInteractionService)
        {
            _userSessionService = userSessionService;
            _userInteractionService = userInteractionService;
        }

        public async Task ExecuteAsync()
        {
            User activeUser = null;

            do
            {
                Console.Clear();
                _userInteractionService.ShowMessage("=== BIENVENIDO ===");
                _userInteractionService.ShowMessage("=== INICIAR SESIÓN ===");
                _userInteractionService.ShowMessage("Ingrese su email:");
                var email = Console.ReadLine();

                if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
                {
                    var result = await _userSessionService.LoginAsync(email);

                    if (result.IsFailed)
                    {
                        _userInteractionService.ShowMessage("No se encontró un usuario con ese email. Intente nuevamente.");
                        await Task.Delay(2000);
                        continue;
                    }

                    activeUser = result.Value;
                    Console.Clear();
                    _userInteractionService.ShowMessage($"Bienvenido, {activeUser.Name}!");
                    await Task.Delay(1000);
                }
                else
                {
                    _userInteractionService.ShowMessage("El email no cumple con un formato válido. Intente nuevamente.");
                    await Task.Delay(2000);
                }

            } while (activeUser == null);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

    }
}