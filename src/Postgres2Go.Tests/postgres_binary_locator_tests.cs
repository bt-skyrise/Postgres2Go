using Xunit;
using Postgres2Go.Helper.Postgres;
namespace Postgres2Go.Tests
{
    public class postgres_binary_locator_tests
    {
        [Fact]
        public void can_find_postgres_binaries_with_default_settings()
        {
            //arrange
            var postgresBinaryLocator = new PostgresBinaryLocator(null);
            
            //act
            var foundDirectory = postgresBinaryLocator.Directory;
            
            //assert
            Assert.True(!string.IsNullOrEmpty(foundDirectory));
        }
    }
}
