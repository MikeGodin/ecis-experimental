using System;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Hedwig.Data;
using Moq;

namespace HedwigTests.Fixtures
{
	public class TestContextProvider : IDisposable
	{
		public TestHedwigContext Context { get; private set; }
		public IHttpContextAccessor HttpContextAccessor { get; private set; }

		public TestContextProvider(bool retainObjects = false)
		{
			
			var loggerFactory = LoggerFactory.Create(builder => {builder.AddConsole();});
			var optionsBuilder = new DbContextOptionsBuilder<HedwigContext>()
				.UseSqlServer(Environment.GetEnvironmentVariable("SQLCONNSTR_HEDWIG"))
				.EnableSensitiveDataLogging();

			if(TestEnvironmentFlags.ShouldLogSQL()) {
                optionsBuilder.UseLoggerFactory(loggerFactory);
			}

           	HttpContextAccessor = new TestHttpContextAccessorProvider().HttpContextAccessor;
			Context = new TestHedwigContext(optionsBuilder.Options, HttpContextAccessor, retainObjects);
		}

		public void Dispose()
		{
			Context?.Dispose();
		}
	}
}
