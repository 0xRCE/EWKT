using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EWKT.Parsers
{
    public class EWKTLexer
    {
        private readonly TextReader reader;
        private char[] charBuffer;
        private string currentToken;
        private TokenType currentTokenType;
        private TokenType previousTokenType;
        private readonly TokenType[] separatorTokens = {
            TokenType.CoordinateSeparator,
            TokenType.GeometryChildSeparator,
            TokenType.GeometryEndSeparator,
            TokenType.GeometryStartSeparator
        };

        private readonly IEnumerable<KeyValuePair<Func<char, bool>, TokenType>> rules = new List<KeyValuePair<Func<char, bool>, TokenType>>
            {
                new KeyValuePair<Func<char, bool>, TokenType>('.'.Equals, TokenType.DecimalSeparator),
                new KeyValuePair<Func<char, bool>, TokenType>('-'.Equals, TokenType.Minus),
                new KeyValuePair<Func<char, bool>, TokenType>(char.IsDigit, TokenType.Number),
                new KeyValuePair<Func<char, bool>, TokenType>(char.IsLetter, TokenType.Word),
                new KeyValuePair<Func<char, bool>, TokenType>('\n'.Equals, TokenType.Eol),
                new KeyValuePair<Func<char, bool>, TokenType>(char.IsWhiteSpace, TokenType.Whitespace),
                new KeyValuePair<Func<char, bool>, TokenType>(char.IsControl, TokenType.Whitespace),
                new KeyValuePair<Func<char, bool>, TokenType>('('.Equals, TokenType.GeometryStartSeparator),
                new KeyValuePair<Func<char, bool>, TokenType>(')'.Equals, TokenType.GeometryEndSeparator),
                new KeyValuePair<Func<char, bool>, TokenType>(','.Equals, TokenType.CoordinateSeparator)
            };

        public int Column { get; private set; }
        public int LineNumber { get; private set; }

        public EWKTLexer(TextReader ewkt)
        {
            reader = ewkt;
        }

        public IEnumerable<Token> Tokenize()
        {
            ResetState();

            var continueLexing = ReadNextChar();
            while (continueLexing)
            {
                var currentChar = charBuffer[0];
                currentToken += currentChar;
                var nextTokenType = PeekNextTokenType();

                if (currentTokenType == TokenType.Minus &&
                    nextTokenType == TokenType.Number &&
                    ReadNextChar()) //read the Number and append
                {
                    currentToken += charBuffer[0];
                    nextTokenType = PeekNextTokenType();
                }
                else if (currentTokenType == TokenType.Number &&
                    nextTokenType == TokenType.DecimalSeparator &&
                    ReadNextChar()) //read the DecimalSeparator and append
                {
                    currentToken += charBuffer[0];
                }
                else if (currentTokenType == TokenType.Word &&
                    nextTokenType == TokenType.Whitespace &&
                    ReadNextChar())
                {
                    nextTokenType = PeekNextTokenType();
                    //(Z and M types, separated by whitespace)
                    if (nextTokenType == TokenType.Word)
                    {
                        currentToken += charBuffer[0];
                        ReadNextChar();
                        nextTokenType = PeekNextTokenType();
                        currentToken += charBuffer[0];
                    }
                    else if (nextTokenType == TokenType.GeometryStartSeparator || //primitive name and space between open parenthesis
                        (nextTokenType == TokenType.Whitespace)) //support for oracle wkt (multiple spaces bewteen primitive and GeometryStartSeparator
                    {
                        //ignore the space
                        currentTokenType = previousTokenType;
                    }

                }
                else if (currentTokenType == TokenType.CoordinateSeparator &&
                    previousTokenType == TokenType.GeometryEndSeparator)
                {
                    currentTokenType = TokenType.GeometryChildSeparator;
                }



                if (currentTokenType != nextTokenType ||
                    separatorTokens.Contains(currentTokenType))
                {
                    if (currentTokenType != TokenType.Whitespace)
                    {
                        yield return CreateToken(currentTokenType, currentToken);
                    }
                    currentToken = string.Empty;
                }

                continueLexing = ReadNextChar();
            }
        }

        private bool ReadNextChar()
        {
            if (currentTokenType != TokenType.Eol)
            {
                previousTokenType = currentTokenType;
            }

            var nextCharRead = reader.Read(charBuffer, 0, 1) != 0;
            if (nextCharRead)
            {
                Column++;
                currentTokenType = GetTokenType(charBuffer[0]);
                if (currentTokenType == TokenType.Eol)
                {
                    Column = 1;
                    LineNumber++;
                    return ReadNextChar();
                }
            }
            else
            {
                currentTokenType = TokenType.Eof;
            }

            return nextCharRead;
        }

        private void ResetState()
        {
            Column = 1;
            LineNumber = 1;
            charBuffer = new char[1];
            currentToken = string.Empty;
            currentTokenType = TokenType.Eof;
            previousTokenType = TokenType.Eof;
        }

        private Token CreateToken(TokenType token, string value)
        {
            return new Token { Column = Column, Linenumber = LineNumber, RawValue = value, Type = token };
        }

        private TokenType PeekNextTokenType()
        {
            int character = reader.Peek();
            const int EOF = -1;
            if (character == EOF)
            {
                return TokenType.Eof;
            }

            return GetTokenType((char)character);
        }


        private TokenType GetTokenType(char character)
        {
            foreach (var rule in rules)
            {
                var match = rule.Key(character);
                if (match)
                {
                    return rule.Value;
                }
            }

            throw new NotSupportedException(String.Format("Character: '{0}'", character));
        }


    }
}
