using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLViewer.Model
{
    public partial class TableDefineObject
    {
        public string TableName { get; set; }

        public TableDefineObject(string _tableName)
        {
            TableName = _tableName;
        }

        List<ColumnDefineObject> Columns = new List<ColumnDefineObject>();
        public void AddColumn(ColumnDefineObject column)
        {
            Columns.Add(column);
        }

        public string ToCSharpModelClass(string _namespace)
        {
            return ToCSharpModelClass(_namespace, TableName);
        }
        public string ToCSharpModelClass(string _namespace, string className)
        {
            string result = @"using System;
using System.Collections.Generic;

namespace {0}
    {{
        public partial class {1}
        {{
{2}
        }}
    }}
";
            string properties = "";
            foreach (var column in Columns)
            {
                properties += column.ToCSharpLanguageProperty() + Environment.NewLine;
            }
            return string.Format(result, _namespace, className, properties);
        }
    }
}
