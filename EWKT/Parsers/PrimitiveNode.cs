using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers
{
    public class PrimitiveNode
    {
        private readonly List<PrimitiveCoordinate> coordinates = new List<PrimitiveCoordinate>();
        private readonly List<PrimitiveNode> children = new List<PrimitiveNode>();

        public PrimitiveNode(string name)
        {
            Name = name;
        }
        public string Name { get; set; }

        public PrimitiveNode Parent { get; set; }

        public IEnumerable<PrimitiveCoordinate> Coordinates { get { return coordinates; } }

        public IEnumerable<PrimitiveNode> Children { get { return children; } }

        public void AddCoordinate(PrimitiveCoordinate coordinate)
        {
            coordinates.Add(coordinate);
        }

        public void AddChild(PrimitiveNode child)
        {
            child.Parent = this;
            children.Add(child);
        }
    }
}
