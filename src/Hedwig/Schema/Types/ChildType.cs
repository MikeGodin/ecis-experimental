using System;
using Hedwig.Models;
using Hedwig.Repositories;
using Hedwig.Security;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Hedwig.Schema.Types
{
	public class ChildType : TemporalGraphType<Child>
	{
		public ChildType(IDataLoaderContextAccessor dataLoader, IFamilyRepository families)
		{
			Field(c => c.Id, type: typeof(NonNullGraphType<IdGraphType>));
			Field(c => c.FirstName);
			Field(c => c.MiddleName, nullable: true);
			Field(c => c.LastName);
			Field(c => c.Suffix, nullable: true);
			Field(c => c.Birthdate);
			Field(c => c.Gender, type: typeof(NonNullGraphType<GenderEnumType>));
			Field(c => c.AmericanIndianOrAlaskaNative, nullable: true);
			Field(c => c.Asian, nullable: true);
			Field(c => c.BlackOrAfricanAmerican, nullable: true);
			Field(c => c.NativeHawaiianOrPacificIslander, nullable: true);
			Field(c => c.White, nullable: true);
			Field(c => c.HispanicOrLatinxEthnicity, nullable: true);
			Field<FamilyType>(
				"family",
				resolve: context =>
				{
					if (!context.Source.FamilyId.HasValue) { return null; }

					var familyId = context.Source.FamilyId.Value;
	 				var asOf = GetAsOfGlobal(context);
					String loaderCacheKey = $"GetFamiliesByIdsAsync{asOf.ToString()}";

					var loader = dataLoader.Context.GetOrAddBatchLoader<int, Family>(
						loaderCacheKey,
						(ids) => families.GetFamiliesByIdsAsync(ids, asOf));

					return loader.LoadAsync(familyId);
				}
			);
		}
	}

	public class ChildQueryType : ChildType, IAuthorizedGraphType
	{
		public ChildQueryType(IDataLoaderContextAccessor dataLoader, IFamilyRepository families)
			: base(dataLoader, families)
		{}

		public AuthorizationRules Permissions(AuthorizationRules rules)
		{
			rules.DenyNot(Hedwig.Security.Permissions.IS_AUTHENTICATED_USER_POLICY);
			rules.Allow(Hedwig.Security.Permissions.USER_CAN_ACCESS_CHILD_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_DEVELOPER_IN_DEV_POLICY);
			rules.Allow(Hedwig.Security.Permissions.IS_TEST_MODE_POLICY);
			rules.Deny();
			return rules;
		}
	}
}
