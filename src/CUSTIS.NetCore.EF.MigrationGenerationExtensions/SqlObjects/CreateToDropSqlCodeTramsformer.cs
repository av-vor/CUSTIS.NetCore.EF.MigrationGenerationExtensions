using CUSTIS.NetCore.EF.MigrationGenerationExtensions.Configuration;
using CUSTIS.NetCore.EF.MigrationGenerationExtensions.Generation.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.SqlObjects
{
    /// <inheritdoc/>
    public class CreateToDropSqlCodeTramsformer : ISqlDropScriptGenerator
    {
        /// <inheritdoc/>
        public string GenerateDropScript(string sqlCreateCode)
        {
            throw new System.NotImplementedException();
        }
    }
}