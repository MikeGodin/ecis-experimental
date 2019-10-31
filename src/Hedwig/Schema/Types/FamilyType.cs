using Hedwig.Models;
using Hedwig.Repositories;
using GraphQL.DataLoader;
using Hedwig.Security;
using GraphQL.Types;
using System;

namespace Hedwig.Schema.Types
{
	public class FamilyType : TemporalGraphType<Family>
	{
		public FamilyType(IDataLoaderContextAccessor dataLoader, IFamilyDeterminationRepository determinations)
		{
			Field(f => f.Id);
			Field(f => f.CaseNumber, type: typeof(IntGraphType));
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<FamilyDeterminationType>>>>(
				"determinations",
				resolve: context =>
				{
					DateTime? asOf = GetAsOfGlobal(context);
					String loaderCacheKey = $"GetDeterminationsByFamilyIdsAsync{asOf.ToString()}";

					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, FamilyDetermination>(
						loaderCacheKey,
						(ids) => determinations.GetDeterminationsByFamilyIdsAsync(ids, asOf)
					);

					return loader.LoadAsync(context.Source.Id);
				}
			);
		}
	}

	public class FamilyQueryType : FamilyType, IAuthorizedGraphType
	{
		public FamilyQueryType(IDataLoaderContextAccessor dataLoader, IFamilyDeterminationRepository determinations)
			: base (dataLoader, determinations)
		{ }

		public AuthorizationRules Permissions(AuthorizationRules rules)
		{
			rules.DenyNot(Hedwig.Security.Permissions.IS_AUTHENTICATED_USER_POLICY);
			rules.Allow(Hedwig.Security.Permissions.USER_CAN_ACCESS_FAMILY_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_DEVELOPER_IN_DEV_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_TEST_MODE_POLICY);
			rules.Deny();
			return rules;
		}
	}
}
