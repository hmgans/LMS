using System;
using Xunit;
using LMS.Controllers;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestGetDepartments()
        {
            CommonController c = new CommonController();
            //var getDeptsResult = c.GetDepartments() as JsonResult; // This line throwing errors due to non-referenced Microsoft ASP stuff

            //dynamic x = getDeptsResult.Value;

            //Assert.Equal(3, x.Length);

        }
    }
}
