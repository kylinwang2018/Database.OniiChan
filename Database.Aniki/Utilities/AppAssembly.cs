using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Database.Aniki.Utilities
{
    /// <summary>
    /// The helper class that get all assembly you want.
    /// </summary>
    public static class AppAssembly
    {
        /// <summary>
        /// Get All assemblys which the dll file starts with the spcific word.
        /// </summary>
        /// <param name="assemblyNameStart">The name start with</param>
        /// <returns>All <see cref="Assembly"/> from the folder</returns>
        public static Assembly[] GetAll(string? assemblyNameStart)
        {
            assemblyNameStart ??= "";
            var allAssemblies = Directory
                        .GetFiles(
                            AppDomain.CurrentDomain.BaseDirectory,
                            $"{assemblyNameStart}*.dll");

            var loadedAssemblies = new List<Assembly>();
            foreach (var assemblie in allAssemblies)
            {
                try
                {
                    loadedAssemblies.Add(Assembly.Load(AssemblyName.GetAssemblyName(assemblie)));
                }
                catch { }
            }
            var assemblies = loadedAssemblies
                .ToArray();
            return assemblies;
        }
    }
}
