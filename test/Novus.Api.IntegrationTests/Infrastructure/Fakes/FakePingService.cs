using System.Net;
using Novus.Api.BackgroundServices;

namespace Novus.Api.IntegrationTests.Infrastructure.Fakes
{
    public class FakePingService : IPingService
    {
        internal const HttpStatusCode Result = HttpStatusCode.EarlyHints;

        public HttpStatusCode WebsiteStatusCode => Result;
    }
}
