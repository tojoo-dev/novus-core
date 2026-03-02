using System.Data;
using Novus.BooksModule.IntegrationTests.Infrastructure.DataSeeders;
using Microsoft.AspNetCore.TestHost;
using Microsoft.FeatureManagement;

namespace Novus.BooksModule.IntegrationTests.Infrastructure
{
    public class TestServerClientFixture
    {
        public HttpClient Client { get; }

        public TestServerClientFixture()
        {
            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseEnvironment("Test")
                        .ConfigureServices(services =>
                        {
                            var config = new ConfigurationBuilder()
                                .AddInMemoryCollection(new Dictionary<string, string>(1) { { "ConnectionStrings:SqliteDb", $"Data Source=tests_tempdb_{DateTimeOffset.Now.ToUnixTimeSeconds()}.db" } })
                                .Build();
                            services.AddRouting();
                            services.AddFeatureManagement();
                            services.AddBooksModule(config);
                        })
                        .Configure(app =>
                        {
                            app.InitBooksModule();

                            var db = app.ApplicationServices.GetService<IDbConnection>();
                            BooksDataSeeder.Seed(db);

                            app.UseRouting();
                            app.UseEndpoints(endpoints => endpoints.MapBooksModule());
                        })
                        .UseTestServer();
                })
                .Start();

            Client = host.GetTestClient();
        }
    }

    [CollectionDefinition(nameof(TestServerClientCollection))]
    public class TestServerClientCollection : ICollectionFixture<TestServerClientFixture>
    {
    }
}
