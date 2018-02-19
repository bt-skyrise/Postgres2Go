using Xunit;
using Postgres2Go.Helper.Postgres;
namespace Postgres2Go.Tests
{
    public class posgres_binary_locator_tests
    {
        [Fact]
        public void can_find_postgres_binaries_with_default_settings()
        {
            var foundDirectory = new PostgresBinaryLocator(null).Directory;
            
            Assert.True(!string.IsNullOrEmpty(binDirectory));
        }
    }
}
