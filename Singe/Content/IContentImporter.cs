using Singe.Debugging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Singe.Content
{
    public interface IContentImporter<out T>
    {
        string FileExtension { get; }

        T Import(Stream asset);
    }
}
