using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Aniki.PostgreSQL
{
    /// <summary>
    /// Add this attribute to any class with its interface will be automatically dependency injected to
    /// <see cref="IServiceCollection"/> after use RegisterPostgreRepositories method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PostgreRepoAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
