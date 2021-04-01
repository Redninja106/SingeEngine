using Singe.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Singe.Content.Importers
{
    internal sealed class MatImporter : IContentImporter<Material>
    {
        public string FileExtension => "mat";

        public Material Import(Stream asset)
        {
            throw new NotImplementedException();
        }
    }
}
