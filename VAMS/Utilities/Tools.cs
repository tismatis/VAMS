using System;

namespace VAMS.Utilities
{
    public static class Tools
    {
        public static Type SearchType(string className)
        {
            var type = Type.GetType(className);
            if (type != null)
                return type;
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(className);
                if (type != null)
                    return type;
            }
            
            return null;
        }
    }
}