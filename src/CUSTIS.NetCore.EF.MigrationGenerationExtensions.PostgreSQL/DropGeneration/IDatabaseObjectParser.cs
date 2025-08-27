using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    /// <summary>
    /// Стратегия парсинга объекта БД для генерации DROP.
    /// Порядок удаления определяется порядком в списке парсеров.
    /// </summary>
    internal interface IDatabaseObjectParser
    {
        /// <summary>
        /// Находит все объекты данного типа в SQL и возвращает DROP-инструкции.
        /// </summary>
        IEnumerable<DropStatement> Parse(string sql);
    }
}
