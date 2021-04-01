using System;
using System.Collections.Generic;
using System.Text;

namespace Singe.Rendering
{
    public struct VertexLayoutElement
    {
        public string Semantic;
        public int ComponentCount;
        public int BytesPerElement;
        public VertexElementType Type;
        public int SemanticIndex;

        public VertexLayoutElement(string semantic, int componentCount, int bytesPerElement, VertexElementType type, int semanticIndex)
        {
            Semantic = semantic;
            ComponentCount = componentCount;
            BytesPerElement = bytesPerElement;
            Type = type;
            SemanticIndex = semanticIndex;
        }
    }
}