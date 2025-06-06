using Application.Common.Interface.Presentation;

namespace Consola.Menu
{
    public interface IMenuFactory
    {
        List<IMenuOptionCommand> CreateMenuCommands();
        Dictionary<string, IMenuOptionCommand> CreateMenuDictionary();
        IMenuOptionCommand Login();
    }
}