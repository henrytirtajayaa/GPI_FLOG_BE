using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Text;

namespace Infrastructure
{
    public static class Extensions
    {
        public static string RemoveTrailingSlash(this string value) => value.EndsWith("/") ? value.TrimEnd('/') : value;

        public static DbTransaction GetDbTransaction(this IDbContextTransaction source)
        {
            return (source as IInfrastructure<DbTransaction>).Instance;
        }
    }

    public static class StringExtensions
    {
        public static string Repeat(this string s, int n)
        {
            return new StringBuilder(s.Length * n)
                            .AppendJoin(s, new string[n + 1])
                            .ToString();
        }
    }

    public static class StringBuilderExtension
    {
        public static string ReplaceString(this string OriginalStr, int index, int length, string SubsituteStr)
        {
            return new StringBuilder(OriginalStr).Remove(index, length).Insert(index, SubsituteStr).ToString();
        }
    }
}
