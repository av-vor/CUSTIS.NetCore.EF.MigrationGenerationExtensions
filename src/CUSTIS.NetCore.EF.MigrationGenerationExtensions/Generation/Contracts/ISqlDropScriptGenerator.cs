using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.Generation.Contracts
{
    /// <summary> Генератор SQL-кода для удаления объекта </summary>
    public interface ISqlDropScriptGenerator
    {
        /// <summary> Генерирует SQL-скрипт удаления объекта PostgreSQL на основе DDL-скрипта создания объекта </summary>
        /// <param name="ddlScript">DDL объекта (create)</param>
        string GenerateDropScript(string ddlScript);
    }
}
