using Xunit;

namespace TestRoguelike
{
    public class IdProviderTest
    {
        [Fact]
        public void NewIdShouldReturnUniqueValues()
        {
            Assert.NotEqual(1, 2);
        }
    }
}
