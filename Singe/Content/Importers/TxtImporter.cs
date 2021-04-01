using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Singe.Content.Importers
{
    internal sealed class TxtImporter : IContentImporter<string>
    {
        public string FileExtension => "txt";

        public string Import(Stream asset)
        {
            throw new NotImplementedException();
        }
    }
}
