using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace WebApiContrib.Tracing.Nlog.Tests.Controllers
{
	public class TestController : ApiController
	{
		private static readonly int[] Items = new[] { 1,2,3 };
		private readonly ITraceWriter _traceWriter;

		public TestController()
		{
			_traceWriter = GlobalConfiguration.Configuration.Services.GetTraceWriter();
		}

		public IEnumerable<int> GetAll()
		{
			_traceWriter.Info(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Info loaded: " + Items.Length);
			return Items;
		}

		public int Get(int id)
		{
		    switch (id)
		    {
                case 0:
                    _traceWriter.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Requested: " + id);
                    break;
                case 1:
                    _traceWriter.Debug(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Requested: " + id);
                    break;
                case 2:
                    _traceWriter.Fatal(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Requested: " + id);
                    break;
                case 3:
                    _traceWriter.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName, "Requested: " + id);
                    break;
		    }

			return id;
		}
	}
}
