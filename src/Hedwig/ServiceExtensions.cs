using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Hedwig.Data;
using Hedwig.Repositories;
using Hedwig.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Hedwig
{
	public static class ServiceExtensions
	{
		public static void ConfigureSqlServer(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<HedwigContext>(options =>
				options.UseSqlServer(connectionString)
			);
		}
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", builder =>
				{
					builder.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowAnyOrigin();
				});
			});
		}

		public static void ConfigureSpa(this IServiceCollection services)
		{
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/build";
			});
		}

		public static void ConfigureRepositories(this IServiceCollection services)
		{
			services.AddScoped<IChildRepository, ChildRepository>();
			services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
			services.AddScoped<IFamilyDeterminationRepository, FamilyDeterminationRepository>();
			services.AddScoped<IFamilyRepository, FamilyRepository>();
			services.AddScoped<IFundingRepository, FundingRepository>();
			services.AddScoped<IOrganizationRepository, OrganizationRepository>();
			services.AddScoped<IReportRepository, ReportRepository>();
			services.AddScoped<ISiteRepository, SiteRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IPermissionRepository, PermissionRepository>();
		}

		public static void ConfigureAuthentication(this IServiceCollection services, string wingedKeysUri)
		{
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
					{
						options.Authority = wingedKeysUri;
						options.Audience = "hedwig_backend";
						options.BackchannelHttpHandler = new HttpClientHandler
						{
							ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
						};
					});
		}

		public static void ConfigureAuthorization(this IServiceCollection services)
		{
			services.AddScoped<IAuthorizationHandler, RequirementsHandler>();
			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					UserSiteAccessRequirement.NAME,
					policy => policy .AddRequirements(new UserSiteAccessRequirement())
				);
				options.AddPolicy(
					UserOrganizationAccessRequirement.NAME,
					policy => policy.AddRequirements(new UserOrganizationAccessRequirement())
				);
			});
		}

		public static void ConfigureControllers(this IServiceCollection services)
		{
			services
			.AddControllers()
			.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
				});
		}
	}
}
