using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    /// <summary>
    /// Утилита для извлечения параметров и их типов из сигнатуры функции/процедуры.
    /// </summary>
    internal static class ParameterExtractor
    {
        /// <summary> Извлекает текст параметров между скобками </summary>
        public static string ExtractParameters(string sql, int startIndex)
        {
            var openPos = sql.IndexOf('(', startIndex);
            if (openPos == -1)
            {
                return string.Empty;
            }

            var closePos = sql.IndexOf(')', startIndex);
            return sql.Substring(openPos, closePos - openPos).TrimStart('(').TrimEnd(')');
        }

        /// <summary> Извлекает типы параметров (например, "integer", "text[]") </summary>
        public static List<string> ExtractParameterTypes(string paramsText)
        {
            if (string.IsNullOrWhiteSpace(paramsText))
            {
                return new List<string>();
            }

            var tokens = paramsText.Split(',');
            var result = new List<string>();

            foreach (var token in tokens)
            {
                var clean = Regex.Replace(token, @"\b(IN|OUT|INOUT|VARIADIC|DEFAULT[^,\)]*)\b", string.Empty,
                    RegexOptions.IgnoreCase).Trim();

                if (string.IsNullOrEmpty(clean))
                {
                    continue;
                }

                var parts = clean.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                var type = string.Join(' ', parts.Skip(1).ToArray());

                if (!string.IsNullOrEmpty(type))
                {
                    result.Add(type);
                }
            }

            return result;
        }
    }
}
