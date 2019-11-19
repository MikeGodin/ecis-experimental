using Xunit;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hedwig.Controllers;
using Hedwig.Repositories;
using Hedwig.Models;

namespace HedwigTests.Controllers
{
    public class EnrollmentsControllerTests
    {
        [Theory]
        [InlineData(new string[]{}, false, false, false, false)]
        [InlineData(new string[]{"child"}, false, true, false, false)]
        [InlineData(new string[]{"family"}, false, false, false, false)]
        [InlineData(new string[]{"determinations"}, false,  false, false, false)]
        [InlineData(new string[]{"fundings"}, true, false, false, false)]
        [InlineData(new string[]{"child", "family"}, false, true, true, false)]
        [InlineData(new string[]{"child", "determinations"}, false, true, false, false)]
        [InlineData(new string[]{"child", "family", "determinations"}, false, true, true, true)]
        [InlineData(new string[]{"child", "family", "determinations", "fundings"}, true, true, true, true)]
        [InlineData(new string[]{"family", "determinations"}, false, false, false, false)]
        [InlineData(new string[]{"family", "determinations", "fundings"}, true, false, false, false)]
        public async Task Get_IncludeEntities_GetsEnrollmentsForSite_WithEntities(
            string[] include,
            bool includeFundings,
            bool shouldGetChildren,
            bool shouldGetFamilies,
            bool includeDeterminations
        )
        {
            var orgId = 1;
            var siteId = 1;
            var _enrollments = new Mock<IEnrollmentRepository>();
            _enrollments.Setup(e => e.GetEnrollmentsForSiteAsync(siteId, It.IsAny<bool>(), null, null))
                .Returns(Task.FromResult(new List<Enrollment> { new Enrollment { ChildId = Guid.NewGuid() } }));
            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildrenByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
                .Returns(Task.FromResult(new List<Child> { new Child { FamilyId = 1 } }));
            var _families = new Mock<IFamilyRepository>();

            var controller = new EnrollmentsController(_enrollments.Object, _children.Object, _families.Object);

            await controller.Get(orgId, siteId, include);

            _enrollments.Verify(e => e.GetEnrollmentsForSiteAsync(siteId, includeFundings, null, null));
            var ctimes = shouldGetChildren ? Times.Once() : Times.Never();
            _children.Verify(c => c.GetChildrenByIdsAsync(It.IsAny<IEnumerable<Guid>>()), ctimes);
            var ftimes = shouldGetFamilies ? Times.Once() : Times.Never();
            _families.Verify(f => f.GetFamiliesByIdsAsync(It.IsAny<IEnumerable<int>>(), includeDeterminations), ftimes);
        }

        [Theory]
        [InlineData(new string[]{}, false, false, false, false)]
        [InlineData(new string[]{"child"}, false, true, false, false)]
        [InlineData(new string[]{"family"}, false, false, false, false)]
        [InlineData(new string[]{"determinations"}, false, false, false, false)]
        [InlineData(new string[]{"fundings"}, true, false, false, false)]
        [InlineData(new string[]{"child", "family"}, false, true, true, false)]
        [InlineData(new string[]{"child", "determinations"}, false, true, false, false)]
        [InlineData(new string[]{"child", "family", "determinations"}, false, true, true, true)]
        [InlineData(new string[]{"child", "family", "determinations", "fundings"}, true, true, true, true)]
        [InlineData(new string[]{"family", "determinations"}, false, false, false, false)]
        [InlineData(new string[]{"family", "determinations", "fundings"}, true, false, false, false)]
        public async Task Get_Id_IncludeEntities_GetsEnrollmentsForSite_WithEntities(
            string[] include,
            bool includeFundings,
            bool shouldGetChild,
            bool shouldGetFamily,
            bool includeDeterminations
        )
        {
            var orgId = 1;
            var siteId = 1;
            var enrollmentId = 1;
            var _enrollments = new Mock<IEnrollmentRepository>();
            _enrollments.Setup(e => e.GetEnrollmentForSiteAsync(enrollmentId, siteId, It.IsAny<bool>()))
                .Returns(Task.FromResult(new Enrollment { ChildId = Guid.NewGuid() }));
            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildByIdAsync(It.IsAny<Guid>(), null))
                .Returns(Task.FromResult(new Child { FamilyId = 1 }));
            var _families = new Mock<IFamilyRepository>();

            var controller = new EnrollmentsController(_enrollments.Object, _children.Object, _families.Object);

            await controller.Get(orgId, enrollmentId, siteId, include);

            _enrollments.Verify(e => e.GetEnrollmentForSiteAsync(enrollmentId, siteId, includeFundings));
            var ctimes = shouldGetChild ? Times.Once() : Times.Never();
            _children.Verify(c => c.GetChildByIdAsync(It.IsAny<Guid>(), null), ctimes);
            var ftimes = shouldGetFamily ? Times.Once() : Times.Never();
            _families.Verify(f => f.GetFamilyByIdAsync(It.IsAny<int>(), includeDeterminations), ftimes);
        }

        [Theory]
        [InlineData(false, true, typeof(CreatedAtActionResult))]
        [InlineData(true, false, typeof(BadRequestResult))]
        public async Task Post_AddsEnrollment_IfValid(
            bool hasId,
            bool shouldAddEnrollment,
            Type resultType
        )
        {
            var organizationId = 1;
            var siteId = 1;

            var _enrollments = new Mock<IEnrollmentRepository>();
            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new EnrollmentsController(_enrollments.Object, _children.Object, _families.Object);

            var enrollment = new Enrollment();
            if(hasId) enrollment.Id = 1;

            var result = await controller.Post(organizationId, siteId, enrollment);
            var times = shouldAddEnrollment ? Times.Once() : Times.Never();
            _enrollments.Verify(e => e.AddEnrollment(It.IsAny<Enrollment>()), times);

            Assert.IsType(resultType, result.Result);
        }

        [Theory]
        [InlineData(1, 1, false, true, typeof(NoContentResult))]
        [InlineData(1, 2, false, false, typeof(BadRequestResult))]
        [InlineData(1, 1, true, true, typeof(NotFoundResult))]
        public async Task Put_UpdatesEnrollment_IfValid_AndExists(
            int pathId,
            int id,
            bool shouldNotFind,
            bool shouldUpdateEnrollment,
            Type resultType
        )
        {
            var orgId = 1;
            var siteId = 1;

            var _enrollments = new Mock<IEnrollmentRepository>();
            if(shouldNotFind) {
                _enrollments.Setup(e => e.SaveChangesAsync())
                    .Throws(new DbUpdateConcurrencyException());
            }
            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new EnrollmentsController(_enrollments.Object, _children.Object, _families.Object);

            var enrollment = new Enrollment{ Id = id };

            var result = await controller.Put(pathId, orgId, siteId, enrollment);
            var times = shouldUpdateEnrollment ? Times.Once() : Times.Never();
            _enrollments.Verify(e => e.UpdateEnrollment(It.IsAny<Enrollment>()), times);
            Assert.IsType(resultType, result.Result);
        }
    }
}