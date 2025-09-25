using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    internal class TypeParser : BaseDatabaseObjectParser
    {
        protected override Regex Pattern => new Regex(
            @"CREATE\s+TYPE\s+(?:""?(?<schema>\w+)""?\.)?""?(?<name>\w+)""?",
            options: RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        protected override DropStatement GetStatement(string sql, Match match)
        {
            var schema = match.Groups["schema"].Success ? match.Groups["schema"].Value : string.Empty;
            var name = match.Groups["name"].Value;

            return new DropStatement.Builder("TYPE", name)
                .SetSchema(schema)
                .SetStartPosition(match.Index)
                .Build();
        }
    }
}
