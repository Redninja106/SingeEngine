using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Singe.Content
{
    public static class ContentLoader
    {
        static IReadOnlyList<Assembly> assemblies = AssemblySearch.GetAssemblies();

        static Dictionary<string, Assembly> nameAssemblyMap = new Dictionary<string, Assembly>();

        static ContentLoader()
        {
            // index every new registered assembly
            AssemblySearch.AssemblyAdded += (sender, e) => MapAssembly(e.Assembly);

            // index every existing registered assembly
            foreach (var assembly in assemblies)
            {
                MapAssembly(assembly);
            }
        }

        /// <summary>
        /// Adds all of the assets within an assembly to the map.
        /// </summary>
        /// <param name="assembly"></param>
        private static void MapAssembly(Assembly assembly)
        {
            var names = assembly.GetManifestResourceNames();
            
            // map every asset name to the assembly in which it is located, ignoring duplicates
            foreach (var name in names)
            {
                if (nameAssemblyMap.ContainsKey(name))
                    continue;

                var shortName = name.Substring(name.IndexOf('.') + 1);
                shortName = shortName.Substring(shortName.IndexOf('.') + 1);
                nameAssemblyMap.Add(shortName, assembly);
            }
        }

        /// <summary>
        /// Gets the content of an asset as an array bytes.
        /// </summary>
        /// <param name="path">The path to the asset</param>
        /// <returns></returns>
        public static byte[] GetAssetBytes(string path)
        {
            var stream = GetAssetStream(path);

            // copy the asset's content into a byte array
            byte[] asset = new byte[stream.Length];
            stream.Read(asset, 0, asset.Length);

            return asset;
        }
        
        /// <summary>
        /// Gets the content of an asset as a <see cref="Stream"/>.
        /// </summary>
        /// <param name="path">The path to the asset</param>
        /// <returns></returns>
        public static Stream GetAssetStream(string path)
        {
            if (!nameAssemblyMap.ContainsKey(path))
                return null;

            // get the asset's assembly
            var assembly = nameAssemblyMap[path];

            // replace slashes
            path.Replace('/', '.');
            path.Replace('\\', '.');

            var fullPath = assembly.GetName().Name + ".Assets." + path;
            // get the stream
            return assembly.GetManifestResourceStream(fullPath);
        }
        
        /// <summary>
        /// Gets the content of an asset as a string.
        /// </summary>
        /// <param name="path">The path to the asset</param>
        /// <returns></returns>
        public static string GetAssetString(string path)
        {
            Stream stream = GetAssetStream(path);

            if(stream == null)
            {
                return null;
            }

            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Gets the path to of every asset available to load.
        /// </summary>
        /// <returns></returns>
        public static string[] GetAvailableAssets()
        {
            return nameAssemblyMap.Keys.ToArray();
        }
    }
}
