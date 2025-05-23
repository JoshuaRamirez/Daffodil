using System.Collections.Generic;

namespace DaffodilApp.Presentation
{
    /// <summary>
    /// Represents everything bound to the books screen.
    /// </summary>
    public class BooksScreen
    {
        /// <summary>
        /// Books component displayed on the books screen.
        /// </summary>
        public Books Books { get; } = new Books();
    }

    /// <summary>
    /// Component that groups multiple books on the books screen.
    /// </summary>
    public class Books
    {
        /// <summary>
        /// Collection of individual book components.
        /// </summary>
        public List<Book> Items { get; } = new();
    }

    /// <summary>
    /// Individual book component.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Optional title for the book.
        /// </summary>
        public string? Title { get; set; }
    }
}
