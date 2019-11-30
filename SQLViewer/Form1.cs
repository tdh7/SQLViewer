using SQLViewer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SQLViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static readonly string DATA_SOURCE = "DESKTOP-5NQGFJL\\SQLEXPRESS";
        private static readonly string DATABASE = "LOP_HOC";
        private static readonly string USERNAME = "sa";
        private static readonly string PASSWORD = "12345";
        private static string connString = @"Data Source=" + DATA_SOURCE + ";Initial Catalog="
                     + DATABASE + ";Persist Security Info=True;User ID=" + USERNAME + ";Password=" + PASSWORD;
        SqlConnection sqlConnection = new SqlConnection(connString);
        SqlDataAdapter adapter;
        DataTable dataTable;

        private void ConnectToDatabase()
        {
            sqlConnection.Open();
            string query = "select * from GIAOVIEN";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.CommandType = CommandType.Text;
            adapter = new SqlDataAdapter(cmd);
            dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView.DataSource = dataTable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectToDatabase();
            DoSomething2();
        }

        private void DoSomething()
        {
            List<string> tables = new List<string>();
            DataTable dt = sqlConnection.GetSchema("Tables");
            foreach (DataRow row in dt.Rows)
            {
                string tableName = (string)row.ItemArray[2];
                foreach (string s in row.ItemArray)
                {
                    Debug.WriteLine("value: " + s);
                    string[] restrictionsColumns = new string[4];
                    restrictionsColumns[2] = tableName;
                    DataTable schemaColumns = sqlConnection.GetSchema("Columns", restrictionsColumns);

                    foreach (System.Data.DataRow rowColumn in schemaColumns.Rows)
                    {
                        string columnName = rowColumn[3].ToString();
                        Debug.WriteLine("With column: " + columnName);
                        
                    }
                }
            
                Debug.WriteLine("--- end row ---");
                tables.Add(tableName);
            }
/*
            foreach (string t in tables)
            {
                Debug.WriteLine("Found table: " + t);

            }


            foreach (DataColumn column in dt.Columns)
            {
                string tablename = (string)column.ColumnName;
                //foreach (string s in row.ItemArray)
                    Debug.WriteLine("value: " + tablename);
                Debug.WriteLine("--- end row ---");
                tables.Add(tablename);
            }

            DisplayData(dt);*/
        }


        private void DoSomething2()
        {
            List<string> tables = new List<string>();
            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    tables.Add(name);
                    Debug.WriteLine("found table: " + name);
                }
            }

            foreach(string tableName in tables)
            {
                printTableInfo2(tableName);
            }


        }

        private void printTableInfo(string tableName)
        {
            string infoQuery =
                 @"select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, 
                    NUMERIC_PRECISION, DATETIME_PRECISION, IS_NULLABLE
                    from INFORMATION_SCHEMA.COLUMNS X
                    where TABLE_NAME = '" + tableName + "'";

            SqlCommand cmd = new SqlCommand(infoQuery, sqlConnection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Debug.WriteLine("--- info of table [" + tableName + "] ---");
                while (reader.Read())
                {
                    /*for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Debug.WriteLine("value: " + reader.GetValue(i));
                    }*/
                    string columnName = reader.GetString(0);
                    string dataType = reader.GetString(1);

                    int cml;
                    if (!reader.IsDBNull(2))
                        cml = reader.GetInt32(2);
                    else cml = -1;
                    byte numeric = (!reader.IsDBNull(3)) ? reader.GetByte(3) : (byte)0;
                    string datetime = (!reader.IsDBNull(4)) ? reader.GetString(4) : "";
                    string isNullable = reader.GetString(5);
                    Debug.WriteLine("Column Name: " + columnName);
                    Debug.WriteLine("Data Type: " + dataType);
                    Debug.WriteLine("Charater maximum length: " + cml);
                    Debug.WriteLine("Numberic: " + numeric);
                    Debug.WriteLine("Datetime precision: " + datetime);
                    Debug.WriteLine("Is Nullable: " + isNullable);
                    Debug.WriteLine("  ");
                    
                }
            }
        }

        private void printTableInfo2(string tableName)
        {
            string infoQuery = @"SELECT c.name, t.name, t.name +
   CASE WHEN t.name IN ('char', 'varchar','nchar','nvarchar') THEN '('+

             CASE WHEN c.max_length=-1 THEN 'MAX'

                  ELSE CONVERT(VARCHAR(4),

                               CASE WHEN t.name IN ('nchar','nvarchar')

                               THEN  c.max_length/2 ELSE c.max_length END )

                  END +')'

          WHEN t.name IN ('decimal','numeric')

                  THEN '('+ CONVERT(VARCHAR(4),c.precision)+','

                          + CONVERT(VARCHAR(4),c.Scale)+')'

                  ELSE '' END as ""DDL name"",

   c.max_length,
   c.precision ,
   c.scale ,
   c.is_nullable,
   ISNULL(i.is_primary_key, 0)
FROM
   sys.columns c
INNER JOIN
   sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN
   sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN
   sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE
   c.object_id = OBJECT_ID('" + tableName + "')";

            SqlCommand cmd = new SqlCommand(infoQuery, sqlConnection);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Debug.WriteLine("--- info of table [" + tableName + "] ---");

                TableDefineObject tableObject = new TableDefineObject(tableName);
                while (reader.Read())
                {
                    /*for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Debug.WriteLine("value: " + reader.GetValue(i));
                    }*/
                    string columnName = reader.GetString(0);
                    string dataType = reader.GetString(1);

                    string fullDataType;
                    if (!reader.IsDBNull(2))
                        fullDataType = reader.GetString(2);
                    else fullDataType = "";

                    
                    int maxLength = (!reader.IsDBNull(3)) ? reader.GetInt16(3) : -1;
                    int precision = (!reader.IsDBNull(4)) ? reader.GetByte(4): 0;
                    int scale = (!reader.IsDBNull(5)) ? reader.GetByte(5) : 0;
                    bool isNullable = reader.GetBoolean(6);
                    Debug.WriteLine("Column Name: " + columnName);
                    bool primaryKey = (!reader.IsDBNull(7)) ? reader.GetBoolean(7) : false;


                    ColumnDefineObject columnObject = new ColumnDefineObject();
                    columnObject.ColumnName = Util.Util.ConvertToPascalCase(columnName);
                    columnObject.DataType = dataType;
                    columnObject.DLLTypeName = fullDataType;
                    columnObject.MaxLengthInByte = maxLength;
                    columnObject.Precision = precision;
                    columnObject.IsNullable = isNullable;
                    columnObject.PrimaryKey = primaryKey;
                    columnObject.Scale = scale;
                    tableObject.AddColumn(columnObject);



                    Debug.WriteLine("Data Type: " + dataType + " => " + Util.Util.ConvertDDLNameToType(dataType));
                    Debug.WriteLine("DDL Name: " + fullDataType);
                    Debug.WriteLine("Max Length In Byte: " + maxLength);
                    Debug.WriteLine("Precision: " + precision);
                    Debug.WriteLine("Is Nullable: " + isNullable);
                    Debug.WriteLine("Is Primary Key: " + primaryKey);
                    Debug.WriteLine("  ");
                }
                string classString = tableObject.ToCSharpModelClass(Util.Util.ConvertToPascalCase(DATABASE)+"Model",Util.Util.ConvertToPascalCase(tableName));
               
                using (StreamWriter writer = new StreamWriter(Util.Util.ConvertToPascalCase(tableName) + ".cs"))
                {
                    writer.Write(classString);
                    writer.Close();
                }
                    Debug.WriteLine(classString);
            }
        }

        private static void DisplayData(System.Data.DataTable table)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
                }
                Console.WriteLine("============================");
            }
        }
    }
}
