using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUSTIS.NetCore.EF.MigrationGenerationExtensions.PostgreSQL.DropGeneration
{
    internal record DropStatement
    {
        public const string DefaultSchema = "public";

        public string Type { get; }

        public string Schema { get; }

        public string Name { get; }

        public IReadOnlyList<string> ParameterTypes { get; }

        public IReadOnlyDictionary<string, string> ExtraData { get; }

        public int StartPosition { get; }

        private DropStatement(string type, string schema, string name,
               List<string> parameterTypes, Dictionary<string, string> extraData, int startPosition)
        {
            Type = type;
            Schema = string.IsNullOrWhiteSpace(schema) ? DefaultSchema : schema;
            Name = name;
            ParameterTypes = parameterTypes.AsReadOnly();
            ExtraData = extraData.AsReadOnly();
            StartPosition = startPosition;
        }

        public string ToSql()
        {
            return Type.ToUpper() switch
            {
                "VIEW" => DropViewSql(),
                "FUNCTION" => DropFunctionSql(),
                "PROCEDURE" => DropProcedureSql(),
                "TRIGGER" => DropTriggerSql(),
                "TYPE" => DropTypeSql(),
                _ => string.Empty
            };
        }

        private string DropTypeSql() => $"DROP TYPE IF EXISTS \"{Schema}\".\"{Name}\";";

        private string DropTriggerSql()
        {
            if (!ExtraData.TryGetValue("TableName", out var tableName))
            {
                return string.Empty;
            }

            ExtraData.TryGetValue("TableSchema", out var tableSchema);
            if (string.IsNullOrEmpty(tableSchema))
            {
                tableSchema = DefaultSchema;
            }

            return $"DROP TRIGGER IF EXISTS \"{Name}\" ON \"{tableSchema}\".\"{tableName}\";";
        }

        private string DropFunctionSql()
        {
            var parameterList = string.Join(", ", ParameterTypes);
            return $"DROP FUNCTION IF EXISTS \"{Schema}\".\"{Name}\"({parameterList});";
        }

        private string DropViewSql() => $"DROP VIEW IF EXISTS \"{Schema}\".\"{Name}\";";

        private string DropProcedureSql()
        {
            var parameterList = string.Join(", ", ParameterTypes);
            return $"DROP PROCEDURE IF EXISTS \"{Schema}\".\"{Name}\"({parameterList});";
        }

        public class Builder(string type, string name)
        {
            private readonly string _type = type ?? throw new ArgumentNullException(nameof(type));
            private readonly string _name = name ?? throw new ArgumentNullException(nameof(name));
            private readonly List<string> _parameterTypes = new();
            private readonly Dictionary<string, string> _extraData = new();
            private string _schema = string.Empty;
            private int _startPosition = int.MaxValue; // по умолчанию — в конец

            public Builder SetParameterTypes(List<string> parameterTypes)
            {
                _parameterTypes.Clear();
                if (parameterTypes != null)
                {
                    _parameterTypes.AddRange(parameterTypes);
                }

                return this;
            }

            public Builder SetStartPosition(int position)
            {
                _startPosition = position;
                return this;
            }

            public Builder SetSchema(string schema)
            {
                _schema = schema;
                return this;
            }

            public Builder SetExtraData(Dictionary<string, string> extraData)
            {
                _extraData.Clear();
                if (extraData != null)
                {
                    foreach (var kv in extraData)
                    {
                        _extraData[kv.Key] = kv.Value;
                    }
                }

                return this;
            }

            public DropStatement Build() =>
                new(_type, _schema, _name, _parameterTypes, _extraData, _startPosition);
        }
    }
}
