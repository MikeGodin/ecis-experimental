using GraphQL.Types;
using GraphQL;
using Hedwig.Repositories;
using Hedwig.Schema.Types;
using Hedwig.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Hedwig.Security;

namespace Hedwig.Schema.Mutations
{
	public class FamilyMutation : ObjectGraphType<Family>, IAppSubMutation, IAuthorizedGraphType
	{
		public FamilyMutation(
			IFamilyRepository repository,
			IChildRepository childRepo)
		{
			FieldAsync<FamilyType>(
				"createFamilyWithChild",
				arguments: new QueryArguments(
					new QueryArgument<StringGraphType>{ Name = "addressLine1" },
					new QueryArgument<StringGraphType>{ Name = "addressLine2" },
					new QueryArgument<StringGraphType>{ Name = "town" },
					new QueryArgument<StringGraphType>{ Name = "state" },
					new QueryArgument<StringGraphType>{ Name = "zip" },
					new QueryArgument<BooleanGraphType>{ Name = "homelessness" },
					new QueryArgument<NonNullGraphType<StringGraphType>>{ Name = "childId" }
				),
				resolve: async context =>
				{
					var childStringId = context.GetArgument<string>("childId");
					var childId = Guid.Parse(childStringId);

					var child = (Child) await childRepo.GetChildByIdAsync(childId);
					if (child == null)
					{
						throw new ExecutionError(
							AppErrorMessages.NOT_FOUND("Child", childId.ToString())
						);
					}

					var addressLine1 = context.GetArgument<string>("addressLine1");
					var addressLine2 = context.GetArgument<string>("addressLine2");
					var town = context.GetArgument<string>("town");
					var state = context.GetArgument<string>("state");
					var zip = context.GetArgument<string>("zip");
					var homelessness = context.GetArgument<bool>("homelessness");

					var family = repository.CreateFamily(
						addressLine1: addressLine1,
						addressLine2: addressLine2,
						town: town,
						state: state,
						zip: zip,
						homelessness: homelessness
					);

					childRepo.UpdateFamily(child, family);

					return family;
				}
			);
		}

		public AuthorizationRules Permissions(AuthorizationRules rules)
		{
			rules.DenyNot("IsAuthenticatedPolicy");
			rules.Allow("IsDeveloperPolicy");
			rules.Allow("IsTestModePolicy");
			rules.Deny();
			return rules;
		}
	}
}