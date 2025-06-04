using Application.Common.Interface.Presentation;

public class MenuFactory
{
    //private readonly IRepositoryQuery _repositoryBase;
    //private readonly IUserInteractionService _userInteractionService;
    //private readonly IProjectConsolePresenter _projectConsolePresenter;
    //private readonly IUserSessionService _userSessionService;
    //private readonly ICreateProjectProposal _createProjectService;
    //private readonly IProjectQueryService _projectQueryService;
    //public MenuFactory(IRepositoryQuery repositoryBase,
    //         IUserInteractionService userInteractionService,
    //         IProjectConsolePresenter projectConsolePresenter,
    //         IUserSessionService userSessionService,
    //         ICreateProjectProposal createProjectService, IProjectQueryService projectQueryService)
    //{
    //    _repositoryBase = repositoryBase;
    //    _userInteractionService = userInteractionService;
    //    _projectConsolePresenter = projectConsolePresenter;
    //    _userSessionService = userSessionService;
    //    _createProjectService = createProjectService;
    //    _projectQueryService = projectQueryService;
    //}



    //public void dicionario()
    //{
    //    Dictionary Menu = new Dictionary<string, IMenuOptionCommand>
    //{
    //    { "1", new CreateProjectCommand(_repositoryBase, _userInteractionService, _projectConsolePresenter,_userSessionService,_createProjectService) },
    //        { "2", new ApproveProjectCommand(_projectQueryService, _userInteractionService, _userSessionService) },
    //{ "3", new ShowMyProjectsCommand(new ProjectQuery(_repositoryBase), _projectConsolePresenter, _userSessionService.GetActiveUser(), _projectSelectionService, _userInteractionService) },
    //{ "4", new ShowApprovedProjectsCommand(_repositoryBase, _projectConsolePresenter) },
    //{ "5", new ShowRejectedProjectsCommand(_repositoryBase, _projectConsolePresenter) },
    //{ "6", new UserSessionCommand(_repositoryBase, _projectConsolePresenter) },
    //{ "7", new ExitCommand(_projectConsolePresenter) }
    //};
    //}

   //Dictionary<> menus= new Dictionary<string, MenuOption>
   // {
   //     { "1", new MenuOption {Description = "Crear un nuevo proyecto", Command = commands.OfType<CreateProjectCommand>().FirstOrDefault() } },
   //     { "2", new MenuOption { Description = "Aprobar un proyecto", Command = commands.OfType<ApproveProjectCommand>().FirstOrDefault() } },
   //     { "3", new MenuOption { Description = "Ver mis proyectos", Command = commands.OfType<ShowMyProjectsCommand>().FirstOrDefault() } },
   //     { "4", new MenuOption { Description = "Ver proyectos aprobados", Command = commands.OfType<ShowApprovedProjectsCommand>().FirstOrDefault() } },
   //     { "5", new MenuOption { Description = "Ver proyectos rechazados", Command = commands.OfType<ShowRejectedProjectsCommand>().FirstOrDefault() } },
   //     { "6", new MenuOption { Description = "Cambiar de usuario", Command = commands.OfType<UserSessionCommand>().FirstOrDefault() } },
   //     { "7", new MenuOption { Description = "Salir", Command = commands.OfType<ExitCommand>().FirstOrDefault() } }
   // };

}
public class MenuOption
{
    public string Description { get; set; }
    public IMenuOptionCommand Command { get; set; }
}
