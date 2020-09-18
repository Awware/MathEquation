using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Lexer.Tokens;
using NUnit.Framework;

namespace MathEquation.Tests
{
    public class MathLexerTests
    {
        private MathLexer Lexer;
        [SetUp]
        public void Setup()
        {
            Lexer = new MathLexer();
        }

        [Test]
        public void NumberTests()
        {
            var tokens = Lexer.Tokenize("1 + 4");
            Assert.AreEqual(tokens.Count, 3);

            Assert.AreEqual((int)tokens[0].Value == 1, true);
            Assert.AreEqual(tokens[1].Kind == SyntaxKind.ADD, true);
            Assert.AreEqual((int)tokens[2].Value == 4, true);
        }
    }
}