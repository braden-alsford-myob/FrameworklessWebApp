namespace FrameworklessWebApp.API
{
    public class Response
    {
        public Response(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
        

        public int StatusCode { get; }
        public string Body { get; }
    }
}