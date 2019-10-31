using System;
using System.Threading.Tasks;
using Hedwig.Data;
using Hedwig.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hedwig.Repositories
{
    public class PermissionRepository : HedwigRepository, IPermissionRepository
    {
        public PermissionRepository(HedwigContext context) : base(context) {}

        public Task<List<SitePermission>> GetSitePermissionsByUserId(int userId, bool includeOrganizations = false)
        {
            var q = _context.Permissions
                .OfType<SitePermission>()
                .Where(sp => sp.UserId == userId);

            if(includeOrganizations)  {
                q = q.Include(sp => sp.Site)
                    .ThenInclude(s=> s.Organization);
            }

            return q.ToListAsync();
        }

        public Task<List<OrganizationPermission>> GetOrganizationPermissionsByUserId(int userId, bool includeSites = false)
        {
            var q = _context.Permissions
                .Where(op => op.UserId == userId)
                .OfType<OrganizationPermission>();

            if(includeSites) {
                q = q.Include(op => op.Organization)
                    .ThenInclude(o => o.Sites);                
            }
            
            return q.ToListAsync();
        }
    }

    public interface IPermissionRepository
    {
        Task<List<SitePermission>> GetSitePermissionsByUserId(int userId, bool includeOrganizations = false);
        Task<List<OrganizationPermission>> GetOrganizationPermissionsByUserId(int userId, bool includeSites = false);
    }
}