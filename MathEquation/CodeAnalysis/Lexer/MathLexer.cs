﻿using MathEquation.CodeAnalysis.Impl;
using MathEquation.CodeAnalysis.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer
{
    public class MathLexer
    {
        private readonly string Content;
        private int ContentLen { get => Content.Length; }
        private LexerPosition LexerPosition;
        private SyntaxKind Kind;
        private object Value;
        public MathLexer(string content)
        {
            LexerPosition = new LexerPosition(0, 0);
            Content = content;
        }
        private char Current { get => Peek(0); }
        private char Lookahead { get => Peek(1); }
        private char Peek(int offset = 0)
        {
            int index = LexerPosition.CurrentPosition + offset;
            if (index >= ContentLen)
                return ' ';
            return Content[index];
        }
        public TokenCollection Tokenize()
        {
            TokenCollection collection = new TokenCollection();
            SyntaxToken token;
            for (int i = 0; i < ContentLen; i++)
            {
                do
                {
                    token = Get();
                    if (token.Kind == SyntaxKind.InvalidToken)
                        throw new InvalidTokenException(LexerPosition.CurrentPosition);
                    if (token.Kind != SyntaxKind.Invisible)
                        collection.Add(token);
                }
                while (token.Kind != SyntaxKind.EOE);
            }
            return collection;
        }
        private SyntaxToken Get()
        {
            LexerPosition.StartPosition = LexerPosition.CurrentPosition;
            Kind = SyntaxKind.InvalidToken;
            Value = null;
            switch (Current)
            {
                case '\n':
                case ' ':
                case '\t':
                case '\r':
                    ReadWhiteSpace();
                    break;
                case '+':
                case '-':
                case '/':
                case '*':
                case '=':
                    ReadOperators();
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0':

                    break;
                default:
                    if (char.IsWhiteSpace(Current))
                        ReadWhiteSpace();
                    break;
            }
            string Text = Content.Substring(LexerPosition.StartPosition, LexerPosition.CurrentPosition - LexerPosition.StartPosition);
            return new SyntaxToken(Kind, Text, new ElementPosition(LexerPosition.StartPosition, LexerPosition.CurrentPosition - LexerPosition.StartPosition), Value);
        }
        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                LexerPosition.CurrentPosition++;
            Kind = SyntaxKind.Invisible;
        }
        private void ReadNumber()
        {
            while (char.IsDigit(Current))
                LexerPosition.CurrentPosition++;

            int len = LexerPosition.StartPosition - LexerPosition.CurrentPosition;
            string str = Content.Substring(LexerPosition.StartPosition, len);
            if (!int.TryParse(str, out int value))
                throw new Exception($"Invalid Number {str}");
            Value = value;
            Kind = SyntaxKind.NUMBER;
        }
        private void ReadOperators()
        {
            if (Current is '+')
                Kind = SyntaxKind.ADD;
            else if (Current is '-')
                Kind = SyntaxKind.SUB;
            else if (Current is '/')
                Kind = SyntaxKind.DIV;
            else if (Current is '*')
                Kind = SyntaxKind.MUL;
            else if (Current is '=')
                Kind = SyntaxKind.EQUALLY;
            LexerPosition.CurrentPosition++;
        }
    }
}
