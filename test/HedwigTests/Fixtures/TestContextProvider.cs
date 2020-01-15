using System;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Hedwig.Data;
using Moq;
using Hedwig;
using Microsoft.Extensions.Configuration;

namespace HedwigTests.Fixtures
{
	public class TestContextProvider : IDisposable
	{
		public TestHedwigContext Context { get; private set; }
		public IHttpContextAccessor HttpContextAccessor { get; private set; }

		public TestContextProvider()
		{
			var configuration = Program.GetIConfigurationRoot();
			var optionsBuilder = new DbContextOptionsBuilder<HedwigContext>()
				.UseSqlServer(configuration.GetConnectionString("HEDWIG"))
				.EnableSensitiveDataLogging();

			if(TestEnvironmentFlags.ShouldLogSQL())
			{
				var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
				optionsBuilder.UseLoggerFactory(loggerFactory);
			}

			HttpContextAccessor = new TestHttpContextAccessorProvider().HttpContextAccessor;
			Context = new TestHedwigContext(optionsBuilder.Options, HttpContextAccessor);
		}
		public void Dispose()
		{
			Context?.Dispose();
		}
	}
}
