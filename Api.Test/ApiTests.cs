using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using API;
using Api.Host;
using Api.Host.DependencyResolver;
using Api.Infrastructure.Util;
using Api.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Api.Tests
{
    [TestFixture]
    public class ApiTests
    {
        private HttpServer _httpServer;
        private HttpClient _httpClient;
        private const string Url = "http://api.pizza.com/"; // HttpServer is in memory, requests are not actually send to this url.

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/
            // We setup the container and http server, following the same code as in application start in global.asax in host project

            // Setup container
            var container = Global.BuildContainer();

            // Setup configuration
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.DependencyResolver = new StructureMapDependencyResolver(container);

            // Setup server
            _httpServer = new HttpServer(config);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _httpServer.Dispose();
        }
        
        [SetUp]
        public void SetUp()
        {
            _httpClient = new HttpClient(_httpServer);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async void CanCreatePizza()
        {
            using (var response = SendRequest("Pizza", HttpMethod.Post, new Pizza { Name = "Basic Pizza" }, "pizza", "pizza"))
            {
                var content = await response.Content.ReadAsStringAsync();
                var pizza = JsonConvert.DeserializeObject<Pizza>(content);

                Assert.AreEqual("Basic Pizza", pizza.Name);
                Assert.IsTrue(pizza.Baked);
            }
        }
        
        private HttpResponseMessage SendRequest(string url, HttpMethod method, object payload, string username = null, string password = null)
        {
            using (var request = new HttpRequestMessage())
            {
                // Set authorization header if username and password are specified
                if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    var credentials = string.Format("{0}:{1}", username, password);
                    var base64EncodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

                    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedCredentials);
                }

                request.RequestUri = new Uri(Url + url);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Method = method;
                request.Content = new JsonContent(payload);

                return _httpClient.SendAsync(request).Result;
            }
        }
    }
}
