using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SQLViewer.Util
{
    static class Util
    {
        private static readonly string[] DDL_NAMES = new string[]
        {


        };

        public static string ConvertToPascalCase(string str)
        {
            string[] arr = str.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
           return arr.Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1).ToLowerInvariant()).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string ConvertDDLNameToType(string ddlName)
        {
            string type = "";
            switch (ddlName) {
                case "bigint":
                    type = "long";
                    break;
                case "binary":
                    type = "byte[]";
                    break;
                case "bit":
                    type = "boolean";
                    break;
                case "char":
                    type = "string";
                    break;
                case "date":
                    type = "DateTime";
                    break;
                case "datetime":
                    type = "DateTime";
                    break;
                case "datetime2":
                    type = "DateTime";
                    break;
                case "datetimeoffset":
                    type = "DateTimeOffset";
                    break;
                case "decimal":
                    type = "decimal";
                    break;
                case "float":
                    type = "float";
                    break;
                case "image":
                    type = "byte[]";
                    break;
                case "int":
                    type = "int";
                    break;
                case "money":
                    type = "decimal";
                    break;
                case "nchar":
                    type = "string";
                    break;
                case "ntext":
                    type = "string";
                    break;
                case "numeric":
                    type = "decimal";
                    break;
                case "nvarchar":
                    type = "string";
                    break;
                case "real":
                    type = "double";
                    break;
                case "smalldatetime":
                    type = "DateTime";
                    break;
                case "smallint":
                    type = "short";
                    break;
                case "smallmoney":
                    type = "decimal";
                    break;
                case "text":
                    type = "string";
                    break;
                case "time":
                    type = "TimeSpan";
                    break;
                case "timestamp":
                    type = "DateTime";
                    break;
                case "tinyint":
                    type = "byte";
                    break;
                case "uniqueidentifier":
                    type = "Guid";
                    break;
                case "varbinary":
                    type = "byte[]";
                    break;
                case "varchar":
                    type = "string";
                    break;
            }
            return type;
        }
    }
}
