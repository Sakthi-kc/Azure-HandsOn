using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Xml.Linq;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Name
{
    [TestFixture]
    public class Testing
    {
        private string ApimBaseURL;
        private string PlantId;

        [SetUp]
        public void SetUp()
        {
            ApimBaseURL = "API-URL";
            PlantId = "Plant-Id";
        }

        [Test]
        public async Task GetUniqueIdDetails()
        {
            try
            {
                //Arrange

                //Initialize request object
                string UniqueIdId = "{Value}";

                // Create a C# object representing the desired JSON structure
                var UniqueIdResponse = new
                {
                    UniqueId = new[]
                    {
                    new
                    {
                        Field1 = PlantId,
                        Field2 = "123",
                        Activity = new[]
                        {
                           new{
                              Field3 = "value",
                              Field4 = "value",
                              Field5 = "value",
                              Field6 = "value"
                            }
                        }
                    }
                },
                };

                // Convert the C# object to a JSON string
                string UniqueIdResponseString = JsonConvert.SerializeObject(UniqueIdResponse);

                //Act

                var HttpCall = new Mock<HttpMessageHandler>();
                HttpCall.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(UniqueIdResponseString, Encoding.UTF8, "application/json")
                    });

                var HttpClient = new HttpClient(HttpCall.Object);

                // Inject the mock
                var PolicyExecutor = new APIMPolicyExecutor(HttpClient);
                
                var Request = new HttpRequestMessage(HttpMethod.Get, ApimBaseURL + "/get-api?UniqueId=" + UniqueId + "&plantid=" + PlantId);
                var Result = await PolicyExecutor.ExecuteGetAsync(Request);

                //Assert
                Assert.NotNull(Result);
                Console.WriteLine(Result);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        [Test]
        public async Task CreateDetails()
        {
            try
            {
                //Arrange

                // Create a C# object representing the desired JSON structure
                var UniqueIdResponse = new
                {
                    Field1 = "ABC",
                    Field2 = "123"
                };

                // Convert the C# object to a JSON string
                string UniqueIdResponseString = JsonConvert.SerializeObject(UniqueIdResponse);

                //Act

                var HttpCall = new Mock<HttpMessageHandler>();
                HttpCall.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Created,
                        Content = new StringContent(UniqueIdResponseString, Encoding.UTF8, "application/json")
                    });

                var UniqueIdRequest = new
                {
                    //Pass the request body as above in response
                };

                var HttpClient = new HttpClient(HttpCall.Object);

                // Inject the mock
                var PolicyExecutor = new APIMPolicyExecutor(HttpClient);

                var Request = new HttpRequestMessage(HttpMethod.Post, ApimBaseURL + "/V1/create-api");
                var Result = await PolicyExecutor.ExecutePostAsync(Request);

                //Assert
                Assert.NotNull(Result);
                Console.WriteLine(Result);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        [Test]
        public async Task UpdateDetails()
        {
            try
            {
                //Arrange

                // Create a C# object representing the desired JSON structure
                var UniqueIdResponse = new
                {
                    Field1 = "ABC",
                    Field2 = "123"
                };

                // Convert the C# object to a JSON string
                string UniqueIdResponseString = JsonConvert.SerializeObject(UniqueIdResponse);

                //Act

                var HttpCall = new Mock<HttpMessageHandler>();
                HttpCall.Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(UniqueIdResponseString, Encoding.UTF8, "application/json")
                    });

                var UniqueIdRequest = new
                {
                    // Pass the request body as above in response
                };

                var HttpClient = new HttpClient(HttpCall.Object);

                // Inject the mock
                var PolicyExecutor = new APIMPolicyExecutor(HttpClient);

                var Request = new HttpRequestMessage(HttpMethod.Put, ApimBaseURL + "/V1/update-api");
                var Result = await PolicyExecutor.ExecutePutAsync(Request);

                //Assert
                Assert.NotNull(Result);
                Console.WriteLine(Result);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
    }

    public class APIMPolicyExecutor
    {
        private readonly HttpClient httpClient;

        public APIMPolicyExecutor(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> ExecuteGetAsync(HttpRequestMessage Request)
        {
            var response = await httpClient.GetAsync(Request.RequestUri);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new Exception($"APIM policy execution failed with status code: {response.StatusCode}");
            }
        }

        public async Task<string> ExecutePostAsync(HttpRequestMessage request)
        {
            var response = await httpClient.PostAsync(request.RequestUri, request.Content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new Exception($"APIM policy execution failed with status code: {response.StatusCode}");
            }
        }

        public async Task<string> ExecutePutAsync(HttpRequestMessage request)
        {
            var response = await httpClient.PostAsync(request.RequestUri, request.Content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                throw new Exception($"APIM policy execution failed with status code: {response.StatusCode}");
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            httpClient.Dispose();
        }
    }
}
