// using System.Net;
// using FrameworklessWebApp.API;
// using FrameworklessWebApp.API.ServiceControllers;
// using Moq;
// using NUnit.Framework;
//
// namespace TestProject1.Unit
// {
//     public class RouterTests
//     {
//         private Router _router;
//         
//         private Mock<IController> _generalJournalEntryController;
//         private Mock<IController> _specificJournalEntryController;
//         private Mock<IController> _generalClientController;
//         private Mock<IController> _specificClientController;
//         
//         [SetUp]
//         public void Setup()
//         {
//             _generalClientController = new Mock<IController>();
//             _specificClientController = new Mock<IController>();
//             _generalJournalEntryController = new Mock<IController>();
//             _specificJournalEntryController = new Mock<IController>();
//             
//             _generalClientController.Setup(c => c.GetResponse(It.IsAny<HttpListenerRequest>(), It.IsAny<string[]>())).Verifiable();
//             _specificClientController.Setup(c => c.GetResponse(It.IsAny<HttpListenerRequest>(), It.IsAny<string[]>())).Verifiable();
//             _generalJournalEntryController.Setup(c => c.GetResponse(It.IsAny<HttpListenerRequest>(), It.IsAny<string[]>())).Verifiable();
//             _specificJournalEntryController.Setup(c => c.GetResponse(It.IsAny<HttpListenerRequest>(), It.IsAny<string[]>())).Verifiable();
//             
//             _router = new Router(_generalJournalEntryController.Object, _specificJournalEntryController.Object, _generalClientController.Object, _specificClientController.Object);
//         }
//
//
//         public void Endpoints_Should_Be_Routed_Correctly()
//         {
//             var alskdjf = new HttpListenerRequest();
//         }
//     }
// }
