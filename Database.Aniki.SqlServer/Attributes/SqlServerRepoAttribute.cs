﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Database.Aniki.SqlServer
{
    /// <summary>
    /// Add this attribute to any class with its interface will be automatically dependency injected to
    /// <see cref="IServiceCollection"/> after use RegisterSqlServerRepositories method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlServerRepoAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
