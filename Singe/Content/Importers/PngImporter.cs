using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Singe.Content.Importers
{
    internal sealed class PngImporter : IContentImporter<Texture>
    {
        public string FileExtension => "PNG";

        public Texture Import(Stream asset)
        {
            throw new NotImplementedException();
        }
    }
}
