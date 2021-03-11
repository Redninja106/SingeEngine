using System;
using System.Collections.Generic;
using System.Text;

namespace Singe
{
    public sealed class Node
    {
        public bool IsRoot => Parent == null;

        public string Name { get; private set; }

        public Node Parent { get; private set; }

        public List<Node> Children { get; private set; }

        public List<Component> Components { get; private set; }

        public Node()
        {
            Children = new List<Node>();
            Components = new List<Component>();
        }
    }
}
