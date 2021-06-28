using ExtensionMethods;
using FluentAssertions;
using Xunit;

namespace OPA_AspDotNetCore_Middleware.Test
{
    public class RequestProviderTest
    {
        [Fact]
        public void CartesianProduct_twoFullVectors_twoPermissions()
        {
            // Arrange
            var permissions = new string[] { "manager" };
            var subPermissions = new string[] { "read", "edit" };

            // Act
            var res = permissions.CartesianProduct(subPermissions);
        }

        [Fact]
        public void CartesianProduct_twoEmptyVectors_emptyArray()
        {
            // Arrange
            var permissions = new string[] { };
            var subPermissions = new string[] { };

            // Act
            var res = permissions.CartesianProduct(subPermissions);

            // Assert
            res.Should().BeEmpty();
        }

        [Fact]
        public void CartesianProduct_twoLevels_fourPermissions()
        {
            // Arrange
            var permissions = new string[] { "manager", "teacher" };
            var subPermissions = new string[] { "read", "edit" };

            // Act
            var res = permissions.CartesianProduct(subPermissions);

            // Assert
            res.Should().Contain(x => x[0] == "manager" && x[1] == "read");
            res.Should().Contain(x => x[0] == "manager" && x[1] == "edit");
            res.Should().Contain(x => x[0] == "teacher" && x[1] == "edit");
            res.Should().Contain(x => x[0] == "teacher" && x[1] == "edit");
        }
    }
}
