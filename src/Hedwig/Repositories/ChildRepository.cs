using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Hedwig.Models;
using Hedwig.Data;
using System.Threading;

namespace Hedwig.Repositories
{
	public class ChildRepository : TemporalRepository, IChildRepository
	{
		public ChildRepository(HedwigContext context) : base(context) {}

		public Task<List<Child>> GetChildrenForOrganizationAsync(int organizationId)
		{
			return _context.Children
				.Where(c => c.OrganizationId.HasValue 
					&& c.OrganizationId.Value == organizationId)
				.ToListAsync();
		}

		public Task<Child> GetChildForOrganizationByIdAsync(Guid id, int organizationId)
		{
			return GetBaseQuery<Child>(null)
				.Where(c => c.Id == id
					&& (
						c.OrganizationId.HasValue 
						&& c.OrganizationId.Value == organizationId
					)
				)
				.FirstOrDefaultAsync();
		}

		public Task<List<Child>> GetChildrenForSiteAsync(int siteId)
		{
			return GetBaseQuery<Child>(null)
				.Include(c => c.Enrollments)
				.Where(c => c.Enrollments.Select(e => e.SiteId).Contains(siteId))
				.ToListAsync();
		}

		public Task<Child> GetChildForSiteByIdAsync(Guid id, int siteId)
		{
			return GetBaseQuery<Child>(null)
				.Include(c => c.Enrollments)
				.Where(c => c.Id == id
					&& c.Enrollments.Select(e => e.SiteId).Contains(siteId))
				.FirstOrDefaultAsync();
		}

		public void AddChild(Child child) 
		{
			_context.Add(child);
		}

		public void UpdateChild(Child child)
		{
			_context.Entry(child).State = EntityState.Modified;
		}

		public Task<int> SaveChangesAsync()
		{
			return _context.SaveChangesAsync();
		}
		public async Task<IDictionary<Guid, Child>> GetChildrenByIdsAsync_OLD(IEnumerable<Guid> ids, DateTime? asOf = null)
		{
			var dict = await GetBaseQuery<Child>(asOf)
				.Where(c => ids.Contains(c.Id))
				.ToDictionaryAsync(x => x.Id);
			return dict as IDictionary<Guid, Child>;
		}

		public Task<List<Child>> GetChildrenByIdsAsync(IEnumerable<Guid> ids)
		{
			return _context.Children
				.Where(c => ids.Contains(c.Id))
				.ToListAsync();
		}

		public async Task<Child> GetChildByIdAsync(Guid id, DateTime? asOf = null)
		{
			return await GetBaseQuery<Child>(asOf)
				.Where(c => c.Id == id)
				.SingleOrDefaultAsync();
		}

		public async Task<ILookup<int, Child>> GetChildrenByFamilyIdsAsync(IEnumerable<int> familyIds, DateTime? asOf = null)
		{
			var children = await GetBaseQuery<Child>(asOf)
				.Where(c => c.FamilyId != null && familyIds.Contains((int) c.FamilyId))
				.ToListAsync();

			return children.ToLookup(c => (int) c.FamilyId);
		}

		public Child UpdateFamily(Child child, Family family)
		{
			child.Family = family;
			return child;
		}

		public Child CreateChild(
		  string sasid,
			string firstName,
			string lastName,
			string middleName = null,
			string suffix = null,
			DateTime? birthdate = null,
			string birthCertificateId = null,
			string birthTown = null,
			string birthState = null,
			bool americanIndianOrAlaskaNative = false,
			bool asian = false,
			bool blackOrAfricanAmerican = false,
			bool nativeHawaiianOrPacificIslander = false,
			bool white = false,
			bool hispanicOrLatinxEthnicity = false,
			Gender gender = Gender.Unspecified,
			bool foster = false,
			int? familyId = null)
		{
			var child = new Child {
				Sasid = sasid,
				FirstName = firstName,
				MiddleName = middleName,
				LastName = lastName,
				Suffix = suffix,
				Birthdate = birthdate,
				BirthCertificateId = birthCertificateId,
				BirthTown = birthTown,
				BirthState = birthState,
				AmericanIndianOrAlaskaNative = americanIndianOrAlaskaNative,
				Asian = asian,
				BlackOrAfricanAmerican = blackOrAfricanAmerican,
				NativeHawaiianOrPacificIslander = nativeHawaiianOrPacificIslander,
				White = white,
				HispanicOrLatinxEthnicity = hispanicOrLatinxEthnicity,
				Gender = gender,
				Foster = foster
			};

			_context.Add<Child>(child);
			return child;
		}
	}

	public interface IChildRepository
	{
		Task<List<Child>> GetChildrenForOrganizationAsync(int organizationId);
		Task<Child> GetChildForOrganizationByIdAsync(Guid id, int organizationId);
		Task<List<Child>> GetChildrenForSiteAsync(int siteId);
		Task<Child> GetChildForSiteByIdAsync(Guid id, int siteId);
		void AddChild(Child child);

		void UpdateChild(Child child);
		Task<int> SaveChangesAsync();
		Task<IDictionary<Guid, Child>> GetChildrenByIdsAsync_OLD(IEnumerable<Guid> ids, DateTime? asOf = null);
		Task<List<Child>> GetChildrenByIdsAsync(IEnumerable<Guid> ids);
		Task<Child> GetChildByIdAsync(Guid id, DateTime? asOf = null);
		Task<ILookup<int, Child>> GetChildrenByFamilyIdsAsync(IEnumerable<int> familyIds, DateTime? asOf = null);
		Child UpdateFamily(Child child, Family family);
		Child CreateChild(
		  string sasid,
			string firstName,
			string lastName,
			string middleName = null,
			string suffix = null,
			DateTime? birthdate = null,
			string birthCertificateId = null,
			string birthTown = null,
			string birthState = null,
			bool americanIndianOrAlaskaNative = false,
			bool asian = false,
			bool blackOrAfricanAmerican = false,
			bool nativeHawaiianOrPacificIslander = false,
			bool white = false,
			bool hispanicOrLatinxEthnicity = false,
			Gender gender = Gender.Unspecified,
			bool foster = false,
			int? familyId = null);
	}
}
