using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.SqlServer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlServerRepoAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
