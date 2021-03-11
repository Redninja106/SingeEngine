using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Singe
{
    public interface ISaveable
    {
        public void WriteToStream(Stream stream);

        public void ReadFromStream(Stream stream);
    }
}
