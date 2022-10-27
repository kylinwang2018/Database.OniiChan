using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Database.Aniki.Utilities
{
    public static class AppAssembly
    {
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

            if (assemblyNameStart == null)
            {
                assemblyNameStart = "";
            }

            var allAssemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                        .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)))
                        .Where(x => x.FullName.StartsWith(assemblyNameStart))
                        .ToArray();

            return allAssemblies;
        }
    }
}
