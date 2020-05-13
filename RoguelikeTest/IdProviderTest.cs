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
        
        [Fact]
        public void NewIdShouldReturnUniqueValues2()
        {
            Assert.Equal(1, 2);
        }
    }
}
