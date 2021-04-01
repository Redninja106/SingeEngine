using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Singe
{
    public sealed class Node
    {

        public bool IsRootNode { get; }
        public string Name { get; private set; }

        public Node Parent { get; private set; }

        private List<Node> children;
        private List<Component> components;

        public Node() : this(false)
        {
        }

        private Node(bool isRoot)
        {
            this.IsRootNode = isRoot;
            this.children = new List<Node>();
            this.components = new List<Component>();
        }

        public void AddChild(Node childNode)
        {
            this.children.Add(childNode);
            childNode.Parent = null;
        }

        internal static Node CreateRootNode()
        {
            return new Node(true);
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)GetComponent(typeof(T));
        }

        public Component GetComponent(Type type)
        {
            return components.FirstOrDefault(c => type == c.GetType());
        }

        public T[] GetComponents<T>() where T : Component
        {
            return GetComponents(typeof(T)) as T[];
            //return Array.ConvertAll(GetComponents(typeof(T)), c => (T)c);
        }

        public Component[] GetComponents(Type type)
        {
            return components.Where(c => type == c.GetType()).ToArray();
        }

    }
}
