using System.Collections.Generic;
using UnityEngine;

namespace Backrooms
{
    public abstract class Node
    {
        private List<Node> _childrenNodes;

        // Getters
        public List<Node> ChildrenNodes { get { return _childrenNodes; } }

        // Properties
        public bool WasVisited;
        public Vector2Int BottomLeftAreaCorner;
        public Vector2Int BottomRightAreaCorner;
        public Vector2Int TopRightAreaCorner;
        public Vector2Int TopLeftAreaCorner;
        public int TreeLayerIndex;
        public Node ParentNode;

        public Node(Node parentNode)
        {
            _childrenNodes = new();
            this.ParentNode = parentNode;

            if (ParentNode != null)
                ParentNode.AddChild(this);
        }

        public void AddChild(Node node)
        {
            _childrenNodes.Add(node);
        }

        public void RemoveChild(Node node)
        {
            if (_childrenNodes.Contains(node))
                _childrenNodes.Remove(node);
        }
    }
}
