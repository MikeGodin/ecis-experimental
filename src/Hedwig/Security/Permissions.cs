using Microsoft.AspNetCore.Authorization;

namespace Hedwig.Security
{
    public class Permissions
    {
      public static string IS_AUTHENTICATED_USER_POLICY = "IsAuthenticatedUserPolicy";
      public static string IS_CURRENT_USER_POLICY = "IsCurrentUserPolicy";
      public static string IS_DEVELOPER_IN_DEV_POLICY = "IsDeveloperInDevPolicy";
      public static string IS_TEST_MODE_POLICY = "IsTestModePolicy";
      public static string USER_CAN_ACCESS_CHILD_POLICY = "UserCanAccessChildPolicy";
      
      // DevelopmentRequirement needs DI access to IHostingEnvironment
      private readonly DevelopmentRequirement _developmentRequirement;

      public Permissions(
        DevelopmentRequirement developmentRequirement
      )
      {
        _developmentRequirement = developmentRequirement;
      }

      public AuthorizationSettings GetAuthorizationSettings()
      {
        var authSettings = new AuthorizationSettings();

        authSettings.AddPolicy(IS_AUTHENTICATED_USER_POLICY, policy =>
          policy.AddRequirement(new AuthenticatedUserRequirement()));

        authSettings.AddPolicy(IS_CURRENT_USER_POLICY, policy =>
          policy.AddRequirement(new CurrentUserRequirement()));

        authSettings.AddPolicy(IS_DEVELOPER_IN_DEV_POLICY, policy =>
          policy.AddRequirement(new DeveloperUserRequirement())
            .AddRequirement(_developmentRequirement));

        authSettings.AddPolicy(IS_TEST_MODE_POLICY, policy =>
          policy.AddRequirement(new TestModeRequirement())
            .AddRequirement(_developmentRequirement));

        authSettings.AddPolicy(USER_CAN_ACCESS_CHILD_POLICY, policy => 
          policy.AddRequirement(new ChildAccessRequirement()));

        return authSettings;
      }
    }
}