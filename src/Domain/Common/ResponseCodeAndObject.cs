using System.Net;

namespace Domain.Common
{
    public class ResponseCodeAndObject<T>
    {
        public T Response { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
        public string? Message { get; set; }
    }
}
