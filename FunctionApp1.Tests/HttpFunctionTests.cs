using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FunctionApp1.Tests
{
    public class Function1Tests
    {
        [Fact]
        public void Run_ShouldReturn_OkObjectResult()
        {
            // 1. ARRANGE
            // On crée un mock pour le Logger
            var mockLogger = new Mock<ILogger<Function1>>();

            // On crée un mock pour la requête HTTP (HttpContext est nécessaire pour HttpRequest)
            var context = new DefaultHttpContext();
            var request = context.Request;

            // On instancie la fonction avec le faux logger
            var function = new Function1(mockLogger.Object);

            // 2. ACT
            var result = function.Run(request);

            // 3. ASSERT
            // On vérifie que le résultat est de type OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result);

            // On vérifie le contenu du message
            Assert.Equal("Welcome to Azure Functions!", okResult.Value);

            // Optionnel : On vérifie que le logger a bien été appelé
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("C# HTTP trigger")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}