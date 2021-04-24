using EWKT.Parsers.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EWKT.Parsers
{
    public class EWKTParser
    {
        private readonly EWKTLexer lexer;

        public EWKTParser(EWKTLexer lexer)
        {
            this.lexer = lexer;
        }

        public static EWKTParser CreateParser(string ewkt)
        {
            var reader = new StringReader(ewkt);
            var lexer = new EWKTLexer(reader);
            var parser = new EWKTParser(lexer);

            return parser;
        }

        public static IGeometry Convert(string ewkt)
        {
            if (string.IsNullOrWhiteSpace(ewkt))
            {
                return null;
            }

            var parser = CreateParser(ewkt);

            return BuildGeometryTree(parser.Parse());
        }

        public PrimitiveNode Parse()
        {
            var tokens = new LinkedList<Token>(lexer.Tokenize());
            //todo: syntactical analyzer
            //todo: semantical analyzer
            var token = tokens.First;

            var rootNode = new PrimitiveNode(token.Value.RawValue);
            token = token.Next;
            var current = rootNode;
            while (token != null)
            {
                if (token.Value.Type == TokenType.Word)
                {
                    //add child
                    current = CreateChild(token.Value.RawValue, current);
                }
                else if ((token.Value.Type == TokenType.GeometryStartSeparator ||
                    token.Value.Type == TokenType.GeometryChildSeparator) &&
                    token.Next.Value.Type == TokenType.GeometryStartSeparator)
                {
                    if (current.Name.StartsWith(GeometryTypes.MULTIPOLYGON, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        current = CreateChild(GeometryTypes.POLYGON, current);
                    }
                    else
                    {
                        //add child (implicit linestring)
                        current = CreateChild(GeometryTypes.LINESTRING, current);
                    }
                }
                else if (token.Value.Type == TokenType.GeometryEndSeparator)
                {
                    //back to parent node
                    current = current.Parent;
                }
                else if (token.Value.Type == TokenType.GeometryStartSeparator &&
                    (token.Next.Value.Type == TokenType.Minus ||
                    token.Next.Value.Type == TokenType.Number))
                {
                    var coordinates = ParseCoordinates(ref token);

                    current.AddCoordinate(new PrimitiveCoordinate { Set = coordinates });
                }


                token = token.Next;
            }

            return rootNode;
        }

        private string ParseCoordinates(ref LinkedListNode<Token> token)
        {
            var coordinates = new StringBuilder();
            foreach (var node in YieldUntilClosingSeparator(token.Next))
            {
                AppendCoordinate(coordinates, node);

                //update naar volgende node
                token = node;
            }

            return coordinates.ToString().Trim();
        }

        private static void AppendCoordinate(StringBuilder coordinates, LinkedListNode<Token> node)
        {
            var currentNodeValuePartOfCoordinate = new[] { TokenType.DecimalSeparator, TokenType.Word }; //serperator or exponent
            if (coordinates.Length > 0 && //current node is not first node in stringbuilder
                node.Value.Type == TokenType.Number && //current node is number
                !currentNodeValuePartOfCoordinate.Contains(node.Previous.Value.Type)) // and current number is not part of decimal part of coordinate or containts exponent (e)

            {
                coordinates.Append(' '); //append space before the current no
            }

            coordinates.Append(node.Value.RawValue);
        }

        private PrimitiveNode CreateChild(string name, PrimitiveNode parent)
        {
            var child = new PrimitiveNode(name);
            parent.AddChild(child);
            return child;
        }

        protected IEnumerable<LinkedListNode<Token>> YieldUntilClosingSeparator(LinkedListNode<Token> node)
        {
            while (node.Value.Type != TokenType.GeometryEndSeparator)
            {
                yield return node;
                node = node.Next;
            }
        }

        private static IGeometry BuildGeometryTree(PrimitiveNode node)
        {
            var root = GeometryFactory.FromPrimitive(node);

            return root.Build();
        }
    }
}
