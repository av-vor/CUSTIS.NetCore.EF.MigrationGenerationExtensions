using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Humanizer;

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

            var depth = 1;
            for (var i = openPos + 1; i < sql.Length; i++)
            {
                if (sql[i] == '(')
                {
                    depth++;
                }
                else if (sql[i] == ')' && --depth == 0)
                {
                    return sql.Substring(openPos + 1, i - openPos - 1).Trim();
                }
            }

            return string.Empty;
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
                var clean = Regex.Replace(token, @"\b(INOUT|IN|OUT|VARIADIC|DEFAULT[^,\)]*)\b", string.Empty,
                    RegexOptions.IgnoreCase).Trim();

                var argmode = Regex.Match(token, @"\b(INOUT|IN|OUT|VARIADIC)\b", RegexOptions.IgnoreCase).Value;

                if (string.IsNullOrEmpty(clean))
                {
                    continue;
                }

                var parts = clean.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                var type = argmode + " " + string.Join(' ', parts.Skip(1).ToArray());

                if (!string.IsNullOrEmpty(type))
                {
                    result.Add(type);
                }
            }

            return result;
        }
    }
}
