using System;
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

            /*var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var loadedAssemblies = new HashSet<string>();

            foreach (var item in allAssemblies)
            {
                loadedAssemblies.Add(item.FullName!);
            }

            var assembliesToCheck = new Queue<Assembly>();
            assembliesToCheck.Enqueue(Assembly.GetEntryAssembly()!);

            while (assembliesToCheck.Any())
            {
                var assemblyToCheck = assembliesToCheck.Dequeue();
                foreach (var reference in assemblyToCheck!.GetReferencedAssemblies())
                {
                    if (!loadedAssemblies.Contains(reference.FullName))
                    {
                        var assembly = Assembly.Load(reference);

                        assembliesToCheck.Enqueue(assembly);

                        loadedAssemblies.Add(reference.FullName);

                        allAssemblies.Add(assembly);
                    }
                }
            }
            */

            assemblyNameStart ??= "";

            var allAssemblies = Directory
                        .GetFiles(
                            AppDomain.CurrentDomain.BaseDirectory, 
                            "*.dll")
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
                        .Where(x => x.FullName.StartsWith(assemblyNameStart))
                        .ToArray();

            return allAssemblies;
        }
    }
}
