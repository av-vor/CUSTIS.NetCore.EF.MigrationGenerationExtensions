using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    internal class TriggerParser : BaseDatabaseObjectParser
    {
        protected override Regex Pattern => new Regex(
            @"CREATE\s+TRIGGER\s+(?:""?(?<name>\w+)""?).+\s+ON\s+(?:""?(?<tableSchema>\w+)""?\.)?(?:""?(?<tableName>\w+)""?)",
            options: RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        protected override DropStatement GetStatement(string sql, Match match)
        {
            var name = match.Groups["name"].Value;
            var tableSchema = match.Groups["tableSchema"].Success ? match.Groups["tableSchema"].Value : string.Empty;
            var tableName = match.Groups["tableName"].Value;
            var extraData = new Dictionary<string, string>
            {
                ["TableSchema"] = tableSchema,
                ["TableName"] = tableName
            };

            return new DropStatement.Builder("TRIGGER", name)
                .SetStartPosition(match.Index)
                .SetExtraData(extraData)
                .Build();
        }
    }
}
