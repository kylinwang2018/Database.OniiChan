using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.PostgreSQL
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PostgreRepoAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
