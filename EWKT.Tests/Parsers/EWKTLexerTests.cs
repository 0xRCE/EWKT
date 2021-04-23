using EWKT.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EWKT.Tests.Parsers
{
    public class EWKTLexerTests
    {
        [Test]
        public void Test_Lexer_Simple_WKT()
        {
            var ewkt = "POINT(1 2)";
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.AreEqual(TokenType.Number, tokens[2].Type);
            Assert.AreEqual("1", tokens[2].RawValue);

            Assert.AreEqual(TokenType.Number, tokens[3].Type);
            Assert.AreEqual("2", tokens[3].RawValue);
        }

        [Test]
        public void Test_Lexer_Tokens()
        {
            var ewkt = "CURVEPOLYGON((1 0, 2 0))"; //note: invalid curve polygon, must have at least four points
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "CURVEPOLYGON");

            Assert.AreEqual(tokens[1].Type, TokenType.GeometryStartSeparator);
            Assert.AreEqual(tokens[2].Type, TokenType.GeometryStartSeparator);

            Assert.AreEqual(tokens[3].Type, TokenType.Number);
            Assert.AreEqual(tokens[4].Type, TokenType.Number);

            Assert.AreEqual(tokens[5].Type, TokenType.CoordinateSeparator);

            Assert.AreEqual(tokens[6].Type, TokenType.Number);
            Assert.AreEqual(tokens[7].Type, TokenType.Number);

            Assert.AreEqual(tokens[8].Type, TokenType.GeometryEndSeparator);
            Assert.AreEqual(tokens[9].Type, TokenType.GeometryEndSeparator);
        }

        [Test]
        public void Test_Lexer_Tokens_Should_Contain_NegativeNumber()
        {
            var ewkt = "POINT(1 -2)";
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "POINT");

            Assert.AreEqual(tokens[1].Type, TokenType.GeometryStartSeparator);

            Assert.AreEqual(tokens[2].Type, TokenType.Number);
            Assert.AreEqual(tokens[2].RawValue, "1");
            Assert.AreEqual(tokens[3].Type, TokenType.Number);
            Assert.AreEqual(tokens[3].RawValue, "-2");

            Assert.AreEqual(tokens[4].Type, TokenType.GeometryEndSeparator);
        }

        [Test]
        public void Test_Lexer_ReadEntireStream_SingleLine()
        {
            var complex_ewkt_sinleline = "CURVEPOLYGON(CIRCULARSTRING(0 0, 4 0, 4 4, 0 4, 0 0), (1 1, 1 4, 3 3, 3 2, 1 1), (1 0, 2 0, 3 1, 1 0))";

            var reader = new StringReader(complex_ewkt_sinleline);
            var lexer = new EWKTLexer(reader);

            lexer.Tokenize().ToList();

            Assert.AreEqual(1, lexer.LineNumber);
            Assert.AreEqual(complex_ewkt_sinleline.Length + 1, lexer.Column);
        }

        [Test]
        public void Test_Lexer_ReadEntireStream_MultiLine()
        {
            var complex_ewkt_multiline = @"CURVEPOLYGON(
CIRCULARSTRING(0 0, 4 0, 4 4, 0 4, 0 0), 

(1 1,1 4 , 3 3, 3 2, 1 1), (1 0, 2 0, 3 1, 1 0))";

            var reader = new StringReader(complex_ewkt_multiline);
            var lexer = new EWKTLexer(reader);

            lexer.Tokenize().ToList();

            Assert.AreEqual(4, lexer.LineNumber);
            Assert.AreEqual(49, lexer.Column);
        }

        [Test]
        public void Test_Lexer_TokensNested()
        {
            var ewkt = "CURVEPOLYGON(CIRCULARSTRING(0 0, 4 0, 4 4, 0 4, 0 0), (1 1,1 4 , 3 3, 3 2, 1 1), (1 0, 2 0, 3 1, 1 0))";
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "CURVEPOLYGON");

            Assert.AreEqual(TokenType.GeometryStartSeparator, tokens[1].Type);
            Assert.AreEqual(TokenType.Word, tokens[2].Type);
            Assert.AreEqual("CIRCULARSTRING", tokens[2].RawValue);

            Assert.AreEqual(TokenType.GeometryStartSeparator, tokens[3].Type);
            Assert.AreEqual(TokenType.Number, tokens[4].Type);

            Assert.AreEqual(TokenType.GeometryStartSeparator, tokens[3].Type); //open ( CIRCULARSTRING
            Assert.AreEqual(TokenType.GeometryEndSeparator, tokens[18].Type); //close ) CIRCULARSTRING

            Assert.AreEqual(TokenType.GeometryEndSeparator, tokens[50].Type); //close ) CURVEPOLYGON
        }

        [Test]
        public void Test_Lexer_Token_NotSupported()
        {
            var ewkt = ((char)33).ToString();
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            Assert.Throws<NotSupportedException>(() => lexer.Tokenize().ToList());
        }

        [Test]
        public void Test_Lexer_Token_With_NumberIntegerAndDecimal()
        {
            var ewkt = "POINT(1 2.0)";
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();


            Assert.AreEqual(TokenType.Number, tokens[2].Type);
            Assert.AreEqual("1", tokens[2].RawValue);

            Assert.AreEqual(TokenType.Number, tokens[3].Type);
            Assert.AreEqual("2.0", tokens[3].RawValue);
        }

        [Test]
        public void Test_Lexer_PrimitiveNode_With_Space_Between_Primitive_And_Seperator()
        {
            var wkt = "POINT (30 10)";

            var reader = new StringReader(wkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "POINT");

            Assert.AreEqual(tokens[1].Type, TokenType.GeometryStartSeparator);
        }

        [Test]
        public void Test_Lexer_GeometryWithSpace_And_Z()
        {
            var ewkt = "LINESTRING Z (75.15 29.53 1, 77 29 1, 77.6 29.5 1, 75.15 29.53 1)";

            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "LINESTRING Z");

            Assert.AreEqual(tokens[1].Type, TokenType.GeometryStartSeparator);

        }

        [Test]
        public void Test_Lexer_Scientific_Notation()
        {
            var ewkt = "POINT(3.0445e-07 19)";

            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);

            var tokens = lexer.Tokenize().ToList();

            Assert.IsTrue(tokens.Count > 0);
            Assert.AreEqual(tokens[0].Type, TokenType.Word);
            Assert.AreEqual(tokens[0].RawValue, "POINT");

            Assert.AreEqual(tokens[2].Type, TokenType.Number);
            Assert.AreEqual(tokens[2].RawValue, "3.0445");

            Assert.AreEqual(tokens[3].Type, TokenType.Word);
            Assert.AreEqual(tokens[3].RawValue, "e");

            Assert.AreEqual(tokens[4].Type, TokenType.Number);
            Assert.AreEqual(tokens[4].RawValue, "-07");
        }
    }
}
