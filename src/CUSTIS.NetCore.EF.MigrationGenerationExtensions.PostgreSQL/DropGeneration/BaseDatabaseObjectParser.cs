using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    /// <summary>
    /// Базовый класс для парсеров DDL-с шаблоном Template Method.
    /// </summary>
    internal abstract class BaseDatabaseObjectParser : IDatabaseObjectParser
    {
        protected abstract Regex Pattern { get; }

        public virtual IEnumerable<DropStatement> Parse(string sql)
        {
            return Pattern.Matches(sql)
                .Cast<Match>()
                .Select(match => GetStatement(sql, match))
                .Where(statement => statement != null);
        }

        protected abstract DropStatement GetStatement(string sql, Match match);
    }
}
