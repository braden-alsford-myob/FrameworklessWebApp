using System.Net;

namespace FrameworklessWebApp.API.ServiceControllers
{
    public interface IController
    {
        Response GetResponse(HttpListenerRequest request, string[] parameters);
    }
}