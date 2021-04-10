using Singe.Debugging;
using Singe.Messaging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Singe.Content
{
    [MessageListener]
    public static class ContentManager
    {
        static List<IContentImporter<object>> importers = new List<IContentImporter<object>>();
        
        static ContentManager()
        {
            importers = new List<IContentImporter<object>>();

            AssemblySearch.GetTypesWithBase(typeof(IContentImporter<object>));
        }
        
        public static void Init()
        {
        }

        public static void Destroy()
        {
            
        }
    }
}
