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
    [Route("api/organizations/{orgId:int}/sites/{siteId}/[controller]")]
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
            int orgId,
            int siteId,
            [FromQuery(Name="include[]")] string[] include
        )
        {
            var includeFundings = include.Contains("fundings");
            var enrollments = await _enrollments.GetEnrollmentsForSiteAsync(
                siteId,
                includeFundings
            );

            if(include.Contains("child")) {
                var children = await _children.GetChildrenByIdsAsync(from e in enrollments select e.ChildId);

                if(include.Contains("family")) {

                    var includeDeterminations = include.Contains("determinations");
                    await _families.GetFamiliesByIdsAsync(
                        children.Where(c => c.FamilyId.HasValue).Select(c => c.FamilyId.Value),
                        includeDeterminations
                    );
                }
            }

            return enrollments;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> Get(
            int id,
            int orgId,
            int siteId,
            [FromQuery(Name="include[]")] string[] include
        )
        {
            var includeFundings = include.Contains("fundings");
            var enrollment = await _enrollments.GetEnrollmentForSiteAsync(id, siteId, includeFundings);

            if (include.Contains("child")) {
                var child = await _children.GetChildByIdAsync(enrollment.ChildId);

                if (include.Contains("family") && child.FamilyId.HasValue) {
                    var includeDeterminations = include.Contains("determinations");
                    await _families.GetFamilyByIdAsync(child.FamilyId.Value, includeDeterminations);
                }
            }

            return enrollment;
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> Post(
            int orgId,
            int siteId,
            Enrollment enrollment
        )
        {
            // TODO check/enforce that if a child object is submitted, child has orgId = orgId
            // TODO check/enforce that if child.Family object is submitted, family has orgId = orgId
            // Q? should we required the correct orgId, or will we also allow orgId = 0/null, and we'll update here?

            if(enrollment.Id != 0) return BadRequest();

            _enrollments.AddEnrollment(enrollment);
            await _enrollments.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { id = enrollment.Id, orgId = orgId, siteId = siteId },
                enrollment
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Enrollment>> Put(
            int id,
            int orgId,
            int siteId,
            Enrollment enrollment
        )
        {
            if (enrollment.Id != id) return BadRequest();

            try {
                _enrollments.UpdateEnrollment(enrollment);
                await _enrollments.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return NotFound();
            }

            return NoContent();
        }
    }
}