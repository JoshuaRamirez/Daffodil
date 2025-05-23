using DaffodilApp.Authorship;

namespace DaffodilApp;

/// <summary>
/// Root factory for the Authorship domain.
/// </summary>
public class AuthorshipDomain
{
    /// <summary>Operations aggregate root.</summary>
    public Operations Operations { get; }

    /// <summary>Presentations aggregate root.</summary>
    public Presentations Presentations { get; }

    /// <summary>Features aggregate root.</summary>
    public Features Features { get; }

    /// <summary>Interactions aggregate root.</summary>
    public Interactions Interactions { get; }

    /// <summary>
    /// Constructs an AuthorshipDomain and initializes its aggregates.
    /// </summary>
    public AuthorshipDomain()
    {
        Operations = new Operations();
        Presentations = new Presentations();
        Features = new Features();
        Interactions = new Interactions();
    }
}
