using EnumerableExtensions;
using FluentAssertions;
using Xunit;

namespace OPA_AspDotNetCore_Middleware.Test
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void CartesianProduct_TwoFullVectors_TwoPermissions()
        {
            // Arrange
            var permissions = new string[]
            {
                "manager",
            };
            var subPermissions = new string[]
            {
                "read", "edit",
            };

            // Act
            var res = permissions.CartesianProduct(subPermissions);

            // Assert
            res.Should().HaveCount(2);
            res.Should().Contain(x => x[0] == "manager" && x[1] == "read");
            res.Should().Contain(x => x[0] == "manager" && x[1] == "edit");
        }

        [Fact]
        public void CartesianProduct_TwoEmptyVectors_EmptyArray()
        {
            // Arrange
            var permissions = new string[]
            {
            };
            var subPermissions = new string[]
            {
            };

            // Act
            var res = permissions.CartesianProduct(subPermissions);

            // Assert
            res.Should().BeEmpty();
        }

        [Fact]
        public void CartesianProduct_TwoLevels_FourPermissions()
        {
            // Arrange
            var permissions = new string[]
            {
                "manager", "teacher",
            };
            var subPermissions = new string[]
            {
                "read", "edit",
            };

            // Act
            var res = permissions.CartesianProduct(subPermissions);

            // Assert
            res.Should().HaveCount(4);
            res.Should().Contain(x => x[0] == "manager" && x[1] == "read");
            res.Should().Contain(x => x[0] == "manager" && x[1] == "edit");
            res.Should().Contain(x => x[0] == "teacher" && x[1] == "read");
            res.Should().Contain(x => x[0] == "teacher" && x[1] == "edit");
        }
    }
}
