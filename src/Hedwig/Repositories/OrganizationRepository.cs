using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Hedwig.Models;
using Hedwig.Data;

namespace Hedwig.Repositories
{
	public class OrganizationRepository : HedwigRepository, IOrganizationRepository
	{

		public OrganizationRepository(HedwigContext context) : base(context) {}

		public async Task<IDictionary<int, Organization>> GetOrganizationsByIdsAsync(IEnumerable<int> ids)
		{
			var dict = await _context.Organizations
				.Where(o => ids.Contains(o.Id))
				.ToDictionaryAsync(x => x.Id);
			return dict as IDictionary<int, Organization>;
		}
	}

	public interface IOrganizationRepository
	{
		Task<IDictionary<int, Organization>> GetOrganizationsByIdsAsync(IEnumerable<int> ids);
	}
}
