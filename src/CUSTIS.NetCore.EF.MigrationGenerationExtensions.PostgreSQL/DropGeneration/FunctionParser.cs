using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    internal class FunctionParser : BaseDatabaseObjectParser
    {
        protected override Regex Pattern => new Regex(
            @"CREATE\s+(?:OR\s+REPLACE\s+)?FUNCTION\s+(?:""?(?<schema>\w+)""?\.)?""?(?<name>\w+)""?\s*\(",
            options: RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        protected override DropStatement GetStatement(string sql, Match match)
        {
            var schema = match.Groups["schema"].Success ? match.Groups["schema"].Value : string.Empty;
            var name = match.Groups["name"].Value;

            var parametersText = ParameterExtractor.ExtractParameters(sql, match.Index);
            var parameterTypes = ParameterExtractor.ExtractParameterTypes(parametersText);

            return new DropStatement.Builder("FUNCTION", name)
                .SetSchema(schema)
                .SetParameterTypes(parameterTypes)
                .SetStartPosition(match.Index)
                .Build();
        }
    }
}
