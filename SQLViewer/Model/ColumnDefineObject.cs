using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLViewer.Model
{
    public partial class ColumnDefineObject
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }

        public string DLLTypeName { get; set; }


        public int MaxLengthInByte { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool PrimaryKey { get; set; }

        public bool IsValid()
        {
            return
                ColumnName != null && ColumnName.Length != 0
                && DataType != null && DataType.Length != 0
                && DLLTypeName != null && DataType.Length != 0
                && MaxLengthInByte >= 0;
        }
        public string ToCSharpLanguageProperty()
        {
            return ToCSharpLanguageProperty(ColumnName);
        }
        public string ToCSharpLanguageProperty(string _columnName)
        {
            string result = "            public {0} {1} {{ get; set; }}";
            return string.Format(result, Util.Util.ConvertDDLNameToType(DataType), _columnName);
        }

    }
}
