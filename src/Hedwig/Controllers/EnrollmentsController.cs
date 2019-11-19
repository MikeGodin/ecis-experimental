using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Hedwig.Models;
using Hedwig.Repositories;
using Hedwig.Security_NEW;

namespace Hedwig.Controllers
{
    [Route("api/sites/{siteId}/[controller]")]
    [ApiController]
    [Authorize(Policy = UserSiteAccessRequirement.NAME)]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollments;
        private readonly IChildRepository _children;
        private readonly IFamilyRepository _families;
        public EnrollmentsController(
            IEnrollmentRepository enrollments,
            IChildRepository children,
            IFamilyRepository families
        )
        {
            _enrollments = enrollments;
            _children = children;
            _families = families;
        }

        [HttpGet]
        public async Task<ActionResult<List<Enrollment>>> Get(
            int siteId,
            [FromQuery(Name="include[]")] string[] include,
            [FromQuery(Name="from")] DateTime? from,
            [FromQuery(Name="to")] DateTime? to
        )
        {
            var includeFundings = include.Contains("funding");
            var enrollments = await _enrollments.GetEnrollmentsForSiteAsync(
                siteId,
                includeFundings,
                from,
                to
            );

            if(include.Contains("child")) {
                var children = await _children.GetChildrenByIdsAsync(from e in enrollments select e.ChildId);

                if(include.Contains("family")) {

                    var includeDeterminations = include.Contains("determinations");
                    await _families.GetFamiliesByIdsAsync(
                        children.Where(c => c.FamilyId.HasValue).Select(c => c.FamilyId.Value).ToArray(),
                        includeDeterminations
                    );
                }
            }

            return enrollments;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Enrollment>>> Get(
            int id,
            int siteId,
            [FromQuery(Name="include[]")] string[] include
        )
        {
            void includeFundings = include.Contains("funding");
            var enrollments = await _enrollments.GetEnrollm

        }
    }
}