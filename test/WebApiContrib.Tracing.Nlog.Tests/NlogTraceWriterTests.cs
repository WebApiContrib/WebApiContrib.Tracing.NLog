using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Tracing;
using NLog.Targets;
using NUnit.Framework;

namespace WebApiContrib.Tracing.Nlog.Tests
{
    [TestFixture]
    public class NlogTraceWriterTests
    {
        private HttpClient _client;
        private MemoryTarget _target;

        [TestFixtureSetUp]
        public void initiFixture()
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NlogTraceWriter());
            var server = new HttpServer(GlobalConfiguration.Configuration);
            _client = new HttpClient(server);
            _client.BaseAddress = new Uri("http://www.test.com");
        }

        [SetUp]
        public void init()
        {
            _target = new MemoryTarget();
            _target.Layout = "${message}";

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(_target);
        }

        [Test]
        public void info_trace()
        {
            var result = _client.GetAsync("test").Result;

            Assert.NotNull(_target.Logs);
            Assert.NotNull(_target.Logs.Where(x => x == "GET http://www.test.com/test WebApiContrib.Tracing.Nlog.Tests.Controllers.TestController Info loaded: 3"));
        }

        [Test]
        public void warn_trace()
        {
            var result = _client.GetAsync("test/0").Result;

            Assert.NotNull(_target.Logs);
            Assert.NotNull(_target.Logs.Where(x => x == "GET http://www.test.com/test/0 WebApiContrib.Tracing.Nlog.Tests.Controllers.TestController Warn Requested: 0"));
        }

        [Test]
        public void debug_trace()
        {
            var result = _client.GetAsync("test/1").Result;

            Assert.NotNull(_target.Logs);
            Assert.NotNull(_target.Logs.Where(x => x == "GET http://www.test.com/test/1 WebApiContrib.Tracing.Nlog.Tests.Controllers.TestController Debug Requested: 1"));
        }

        [Test]
        public void fatal_trace()
        {
            var result = _client.GetAsync("test/2").Result;

            Assert.NotNull(_target.Logs);
            Assert.NotNull(_target.Logs.Where(x => x == "GET http://www.test.com/test/2 WebApiContrib.Tracing.Nlog.Tests.Controllers.TestController Fatal Requested: 2"));
        }

        [Test]
        public void error_trace()
        {
            var result = _client.GetAsync("test/3").Result;

            Assert.NotNull(_target.Logs);
            Assert.NotNull(_target.Logs.Where(x => x == "GET http://www.test.com/test/3 WebApiContrib.Tracing.Nlog.Tests.Controllers.TestController Error Requested: 3"));
        }
    }
}
