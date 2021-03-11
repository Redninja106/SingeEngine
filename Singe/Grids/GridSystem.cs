using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Singe.Grids
{
    /// <summary>
    /// Abstract base class for all types of grid systems. Supports serialization and maintaining a simple array of data;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GridSystem<T> where T : struct 
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int OriginX { get; private set; }
        public int OriginY { get; private set; }

        T[] cellData;

        public abstract T GetCell(int x, int y);

        public virtual void Resize()
        {
            cellData = new T[Width * Height];
        }
    }
}
