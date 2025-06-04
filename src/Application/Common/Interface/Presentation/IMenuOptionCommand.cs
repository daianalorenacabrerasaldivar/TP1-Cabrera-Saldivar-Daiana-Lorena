namespace Application.Common.Interface.Presentation
{
    public interface IMenuOptionCommand
    {
        public string Name { get; }
        Task ExecuteAsync();
    }

}

