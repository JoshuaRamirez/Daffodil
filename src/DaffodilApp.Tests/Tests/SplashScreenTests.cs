using DaffodilApp.Presentation;
using Xunit;

namespace DaffodilApp.Tests
{
    public class SplashScreenTests
    {
        [Fact]
        public void SplashScreen_HasProfilesComponent()
        {
            var screen = new SplashScreen();
            Assert.NotNull(screen.Profiles);
            Assert.NotNull(screen.Profiles.Items);
        }
    }
}
