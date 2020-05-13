using web;
using Xunit;

namespace TestRoguelike
{
    public class IdProviderTest
    {
        [Fact]
        public void NewIdShouldReturnUniqueValues()
        {
            var idProvider = new IdProvider();
            var id1 = idProvider.NewId();
            var id2 = idProvider.NewId();

            Assert.NotEqual(id1, id2);
        }
    }
}
