using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Hedwig.Models;
using Hedwig.Repositories;
using Hedwig.Security_NEW;

namespace Hedwig.Controllers
{
    [Route("api/organizations/{orgId:int}/[controller]")]
    [ApiController]
    [Authorize(Policy = UserOrganizationAccessRequirement.NAME)]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildRepository _children;
        private readonly IFamilyRepository _families;

        public ChildrenController(IChildRepository children,  IFamilyRepository families)
        {
            _children = children;
            _families = families;
        }

        [HttpGet]
        public async Task<ActionResult<List<Child>>> Get(
            int orgId,
            [FromQuery(Name="include[]")] string[] include
        ) 
        {
            var children = await _children.GetChildrenForOrganizationAsync(orgId);

            if(include.Contains("family")) {
                var includeDeterminations = include.Contains("determinations");
                await _families.GetFamiliesByIdsAsync(
                    children.Where(c => c.FamilyId != null).Select(c => c.FamilyId.Value).ToList(),
                    includeDeterminations
                );
            }

            return children;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Child>> Get(
            Guid id,
            int orgId,
            [FromQuery(Name="include[]")] string[] include
        )
        {
            var child = await _children.GetChildForOrganizationAsync(id, orgId);

            if(include.Contains("family") && child.FamilyId.HasValue) {
                var includeDeterminations = include.Contains("determinations");
                await _families.GetFamilyByIdAsync(child.FamilyId.Value, includeDeterminations);
            }

            return child;
        }

        [HttpPost]
        public async Task<ActionResult<Child>> Post(int orgId, Child child)
        {
            if(child.Id != new Guid()) return BadRequest();
            
            // TODO validate that orgId == child.organizationId (as part of model validation?)
            _children.AddChild(child);
            await _children.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new {id = child.Id, orgId = orgId },
                child
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Child>> Put(Guid id, int orgId, Child child)
        {
            if (child.Id != id) return BadRequest();

            try {
                _children.UpdateChild(child);
                await _children.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                return NotFound();
            }

            return NoContent();
        }
    }
}