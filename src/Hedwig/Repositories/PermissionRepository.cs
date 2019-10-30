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

        public Task<FamilyPermission> GetFamilyPermissionByFamilyIdAsync(int familyId)
        {
            return _context.EntityPermissions
                .OfType<FamilyPermission>()
                .FirstOrDefaultAsync(ep => ep.FamilyId == familyId);
        }

        public Task<List<UserSitePermission>> GetSitePermissionsByUserId(int userId, bool includeOrganizations = false)
        {
            var q = _context.UserPermissions
                .OfType<UserSitePermission>()
                .Where(sp => sp.UserId == userId);

            if(includeOrganizations)  {
                q.Include(sp => sp.Site)
                    .ThenInclude(s=> s.Organization);
            }

            return q.ToListAsync();
        }

        public Task<List<UserOrganizationPermission>> GetOrganizationPermissionsByUserId(int userId, bool includeSites = false)
        {
            var q = _context.UserPermissions
                .OfType<UserOrganizationPermission>()
                .Where(op => op.UserId == userId);

            if(includeSites) {
                q.Include(op => op.Organization)
                    .ThenInclude(o => o.Sites);                
            }
            
            return q.ToListAsync();
        }
    }

    public interface IPermissionRepository
    {
        Task<FamilyPermission> GetFamilyPermissionByFamilyIdAsync(int familyId);
        Task<List<UserSitePermission>> GetSitePermissionsByUserId(int userId, bool includeOrganizations = false);
        Task<List<UserOrganizationPermission>> GetOrganizationPermissionsByUserId(int userId, bool includeSites = false);
    }
}