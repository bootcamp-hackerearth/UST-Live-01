using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HealthcareMvcApiTests.Fakes
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public HttpRequestMessage Request { get; private set; }

        public List<HttpRequestMessage> Requests { get; }

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
            Requests = new List<HttpRequestMessage>();
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Request = request;
            Requests.Add(request);

            return Task.FromResult(_response);
        }
    }
}