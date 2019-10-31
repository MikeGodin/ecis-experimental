using System;
using Hedwig.Models;
using Hedwig.Repositories;
using Hedwig.Security;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Hedwig.Schema.Types
{
	public class EnrollmentType : TemporalGraphType<Enrollment> 
	{
		public EnrollmentType(IDataLoaderContextAccessor dataLoader, IChildRepository children, IFundingRepository fundings)
		{
			Field(e => e.Id);
			Field<DateTime>(e => e.Entry);
			Field<DateTime?>(e => e.Exit, nullable: true);
			Field<NonNullGraphType<ChildType>>(
				"child",
				resolve: context =>
				{
					DateTime? asOf = GetAsOfGlobal(context);
					String loaderCacheKey = $"GetChildByIdsAsync{asOf.ToString()}";
					var loader = dataLoader.Context.GetOrAddBatchLoader<Guid, Child>(
						loaderCacheKey,
						(ids) => children.GetChildrenByIdsAsync(ids, asOf));

					return loader.LoadAsync(context.Source.ChildId);
				}
			);
			Field<NonNullGraphType<ListGraphType<NonNullGraphType<FundingType>>>>(
				"fundings",
				resolve: context =>
				{
					DateTime? asOf = GetAsOfGlobal(context);
					String loaderCacheKey = $"GetFundingsByEnrollmentIdsAsync{asOf.ToString()}";
					var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<int, Funding>(
						loaderCacheKey,
						(ids) => fundings.GetFundingsByEnrollmentIdsAsync(ids, asOf));

					return loader.LoadAsync(context.Source.Id);
				}
			);
		}

	}
	public class EnrollmentQueryType : EnrollmentType, IAuthorizedGraphType
	{ 
		public EnrollmentQueryType(IDataLoaderContextAccessor dataLoader, IChildRepository children, IFundingRepository funding)
		: base(dataLoader, children, funding)
		{}	
		public AuthorizationRules Permissions(AuthorizationRules rules)
		{
			rules.DenyNot(Hedwig.Security.Permissions.IS_AUTHENTICATED_USER_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_DEVELOPER_IN_DEV_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_TEST_MODE_POLICY);
			rules.Allow(Hedwig.Security.Permissions.USER_CAN_ACCESS_ENROLLMENT_POLICY);
			rules.Deny();
			return rules;
		}
	}
}
