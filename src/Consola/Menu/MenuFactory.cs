using Application.Common.Interface.Presentation;
using Consola.Menu.CommandMenu;
using Consola.Menu.CommandMenu.ApproveProject;
using Consola.Menu.CommandMenu.CreateProject;
using Consola.Menu.CommandMenu.ShowProjects;
using Consola.Menu.CommandMenu.UserSesion;
using Microsoft.Extensions.DependencyInjection;

namespace Consola.Menu
{
    /// <summary>
    /// Fábrica responsable de crear y proporcionar los comandos del menú
    /// </summary>
    public class MenuFactory: IMenuFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MenuFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public List<IMenuOptionCommand> CreateMenuCommands()
        {
            return new List<IMenuOptionCommand>
            {
                _serviceProvider.GetRequiredService<CreateProjectConsoleAction>(),
                _serviceProvider.GetRequiredService<ApproveProjectCommand>(),
                _serviceProvider.GetRequiredService<ShowMyProjectsCommand>(),
                _serviceProvider.GetRequiredService<ShowApprovedProjectsCommand>(),
                _serviceProvider.GetRequiredService<ShowRejectedProjectsCommand>(),
                _serviceProvider.GetRequiredService<UserSessionCommand>(),
                _serviceProvider.GetRequiredService<ExitCommand>()
            };
        }

        public IMenuOptionCommand Login() { 
            return _serviceProvider.GetRequiredService<UserSessionCommand>();
        }
        public Dictionary<string, IMenuOptionCommand> CreateMenuDictionary()
        {
            var commands = CreateMenuCommands();
            var menuDict = new Dictionary<string, IMenuOptionCommand>();

            for (int i = 0; i < commands.Count; i++)
            {
                menuDict.Add((i + 1).ToString(), commands[i]);
            }

            return menuDict;
        }
    }
}