namespace Application.Common.Exceptions
{
    public class CustomResponseException : Exception
    {
        public CustomResponseException()
        {

        }
        public CustomResponseException(string message) : base(message)
        {

        }
    }
}
