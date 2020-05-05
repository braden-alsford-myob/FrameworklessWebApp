using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests
{
    public class BasicTests : IClassFixture<WebApplicationFactory<RazorPagesProject.Startup>>
    {

        [SetUp]
        public void Setup()
        {
            
        }

        [Fact]
        public void Test1()
        {
        }
    }
}