using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CUSTIS.NetCore.EF.MigrationGenerationExtensions.Generation.Contracts;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    internal class PostgreSqlDropScriptGenerator : ISqlDropScriptGenerator
    {
        private readonly IReadOnlyList<IDatabaseObjectParser> _parsers =
        [
            new ViewParser(),
            new FunctionParser(),
            new ProcedureParser(),
            new TriggerParser(),
        ];

        /// <summary>
        /// Генерирует скрипт DROP для всех найденных объектов.
        /// </summary>
        public string GenerateDropScript(string createScript)
        {
            if (string.IsNullOrWhiteSpace(createScript))
            {
                return string.Empty;
            }

            var statements = _parsers
                .SelectMany(parser => parser.Parse(createScript))
                .Where(s => s != null)
                .OrderByDescending(s => s.StartPosition)
                .ToList();

            var sb = new StringBuilder();
            foreach (var statement in statements)
            {
                sb.AppendLine(statement.ToSql());
            }

            return sb.ToString().Trim();
        }
    }
}