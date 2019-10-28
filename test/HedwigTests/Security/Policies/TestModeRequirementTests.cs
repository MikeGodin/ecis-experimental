using Xunit;
using System.Collections.Generic;
using Moq;
using Hedwig.Security;
using System.Security.Claims;
using HedwigTests.Helpers;

namespace HedwigTests.Security
{
    public class TestModeRequirementTests
    {
        [Fact]
        public async void When_TestMode_Authorize_Produces_No_Error()
        {
          // If
          var requirement = new TestModeRequirement();
          var context = new AuthorizationContext();
          var claims = new Dictionary<string, string>() {
            { "test_mode", "true" }
          };
          context.User = AuthorizationRequirementHelper.CreatePrincipal("password", claims);

          // When
          await requirement.Authorize(context);

          // Then
          Assert.False(context.HasErrors);
        }

        [Fact]
        public async void When_Not_TestMode_Authorize_Produces_Error()
        {
          // If
          var requirement = new TestModeRequirement();
          var context = new AuthorizationContext();
          var claims = new Dictionary<string, string>() {
            { "test_mode", "false" }
          };
          context.User = AuthorizationRequirementHelper.CreatePrincipal("password", claims);

          // When
          await requirement.Authorize(context);

          // Then
          Assert.True(context.HasErrors);
        }
    }
}