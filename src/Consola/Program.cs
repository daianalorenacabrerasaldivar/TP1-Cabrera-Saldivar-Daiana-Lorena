// See https://aka.ms/new-console-template for more information
using Application.Common.Interface.Presentation;
using Application.Dependecy;
using Consola.Menu;
using Consola.Menu.CommandMenu;
using Consola.Menu.CommandMenu.ApproveProject;
using Consola.Menu.CommandMenu.CreateProject;
using Consola.Menu.CommandMenu.ShowProjects;
using Consola.Menu.CommandMenu.UserSesion;
using Consola.PresentacionCommon;
using Infrastructure.Dependecy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);

services.AddDependecyInjectionApplication();
services.AddDependecyInjectionInfrastructure();


services.AddScoped<MenuManagerHandler>();
services.AddScoped<IConsoleUserInteractionService, ConsoleUserInteractionService>();
services.AddScoped<IProjectConsolePresenter, ProjectConsolePresenter>();
services.AddScoped<IProjectSelectionService, ProjectSelectionService>();
services.AddScoped<IMenuFactory, MenuFactory>();
// Registrar todos los comandos del menú
services.AddScoped<CreateProjectConsoleAction>();
services.AddScoped<ApproveProjectCommand>();
services.AddScoped<ShowMyProjectsCommand>();
services.AddScoped<ShowApprovedProjectsCommand>();
services.AddScoped<ShowRejectedProjectsCommand>();
services.AddScoped<UserSessionCommand>();
services.AddScoped<ExitCommand>();





Console.WriteLine("Hola, Bienvenido");
await Task.Delay(1000);



var serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var menuManager = scope.ServiceProvider.GetRequiredService<MenuManagerHandler>();

    await menuManager.RunMenuAsync();
}

