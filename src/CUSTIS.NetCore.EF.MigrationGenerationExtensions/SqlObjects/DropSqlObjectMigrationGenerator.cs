using CUSTIS.NetCore.EF.MigrationGenerationExtensions.Generation;
using CUSTIS.NetCore.EF.MigrationGenerationExtensions.Generation.Contracts;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.SqlObjects
{
    internal sealed class DropSqlObjectMigrationGenerator : CustomMigrationOperationGeneratorBase<DropSqlObjectOperation>
    {
        private readonly ICSharpHelper _code;
        private readonly ISqlDropScriptGenerator _dropScriptGenerator;

        public DropSqlObjectMigrationGenerator(ICSharpHelper code, ISqlDropScriptGenerator dropScriptGenerator)
        {
            _code = code;
            _dropScriptGenerator = dropScriptGenerator;
        }

        /// <summary>
        ///     Generates code for an <see cref="DropSqlObjectOperation" />.
        /// </summary>
        protected override void Generate(DropSqlObjectOperation operation, IndentedStringBuilder builder)
        {
            builder.AppendLine($".{nameof(MigrationBuilderSqlObjectsExtensions.DropSqlObject)}(");
            using (builder.Indent())
            {
                builder.Append("name: ").Append(_code.Literal(operation.Name)).AppendLine(",");
                builder.Append("sqlCode: ").Append(_code.Literal(_dropScriptGenerator.GenerateDropScript(operation.SqlCode))).AppendLine(",");
                //builder.Append("sqlCode: ").Append($"DROP {operation.Name}").AppendLine(", // write code to drop object");
                builder.Append("order: ").Append(_code.Literal(operation.Order)).Append(")");
            }
        }
    }
}