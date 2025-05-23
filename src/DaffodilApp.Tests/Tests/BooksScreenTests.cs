using DaffodilApp.Presentation;
using Xunit;

namespace DaffodilApp.Tests
{
    public class BooksScreenTests
    {
        [Fact]
        public void BooksScreen_HasBooksComponent()
        {
            var screen = new BooksScreen();
            Assert.NotNull(screen.Books);
            Assert.NotNull(screen.Books.Items);
        }
    }
}
