namespace FrameworklessWebApp.API
{
    public static class BasicResponseBuilder
    {
        
        public static Response GetNotFound()
        {
            return new Response(404, "Not Found");
        }


        public static Response GetBadRequest()
        {
            return new Response(400, "Bad Request");
        }
    }
}