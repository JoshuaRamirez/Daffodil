using System.Collections.Generic;

namespace DaffodilApp.Presentation
{
    /// <summary>
    /// Represents everything bound to the splash screen.
    /// </summary>
    public class SplashScreen
    {
        /// <summary>
        /// Profiles component displayed on the splash screen.
        /// </summary>
        public Profiles Profiles { get; } = new Profiles();
    }

    /// <summary>
    /// Component that groups multiple profiles on the splash screen.
    /// </summary>
    public class Profiles
    {
        /// <summary>
        /// Collection of individual profile components.
        /// </summary>
        public List<Profile> Items { get; } = new();
    }

    /// <summary>
    /// Individual profile component.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Optional display name for the profile.
        /// </summary>
        public string? Name { get; set; }
    }
}
