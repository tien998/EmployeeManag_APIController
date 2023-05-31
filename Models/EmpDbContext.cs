using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace EmpManagement_APIController.Models;

public class EmployDb
{
    static string str = "data source= 127.0.0.1,1433; initial catalog=Management; uid=sa; pwd=Password123; TrustServerCertificate=true";
    internal static SqlConnection? connection;
    internal static SqlDataAdapter? adapter;
    internal static DataSet? dataSet;
    internal static DataTable? empTable;
    public static bool rowNull { get; set; }

    static void InitADO_Object()
    {
        connection = new SqlConnection(str);
        adapter = new SqlDataAdapter();
        dataSet = new DataSet();
    }
    static void MappingAndFill()
    {
        adapter!.TableMappings.Add("Employees", "EmployeeT");
        adapter!.Fill(dataSet!, "Employees");
        empTable = dataSet!.Tables["EmployeeT"];
    }
    public static async Task<List<Employee>> Get()
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select * from Employees", connection);
        MappingAndFill();
        connection!.Open();
        empTable!.WriteXml("Xml/EmpTable.xml");
        List<Employee> employees = new List<Employee>();

        // Sử dụng vòng lặp để truyền giá trị từ 'DataRowCollection' sang 'List<Employee>'
        foreach (DataRow r in empTable!.Rows)
        {
            // Bảng Employee trên CSDL có: NvID, Name, DateOfBirth
            var em = new Employee
            {
                ID = (int)r["ID"],
                Name = r["Names"].ToString(),
                DateOfBirth = DateTime.Parse(r["DateOfBirth"].ToString()!),
            };

            employees.Add(em);
        }
        connection.Close();
        Dispose.ADO_Objects();
        await Task.CompletedTask;
        return employees;
    }
    public static async Task<Employee> Get(int id)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select * from Employees where ID = @nvID", connection);
        var nvID = adapter!.SelectCommand.Parameters.Add("nvID", SqlDbType.Int);
        nvID.Value = id;
        MappingAndFill();
        adapter.Fill(dataSet!, "Employee");
        dataSet!.WriteXml("Xml/dataSet.xml");

        var e = new Employee();

        if (empTable!.Rows.Count != 0)
        {
            var r = empTable!.Rows[0];
            e.ID = (int)r!["ID"];
            e.Name = r["Names"].ToString()!;
            e.DateOfBirth = DateTime.Parse(r["DateOfBirth"].ToString()!);
        }


        Dispose.ADO_Objects();
        await Task.CompletedTask;
        return e;
    }
    public static void Create(Employee e)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand("Select Names, DateOfBirth from Employees", connection);
        MappingAndFill();
        adapter!.InsertCommand = new SqlCommand("Insert into Employees(names, DateOfBirth) values (@name, @dateOfBirth)", connection);

        var first = adapter!.InsertCommand.Parameters.AddWithValue("name", e.Name);
        var last = adapter!.InsertCommand.Parameters.AddWithValue("dateOfBirth", e.DateOfBirth);
        var nRow = empTable!.Rows.Add();
        adapter.Update(dataSet!, "Employees");
        Dispose.ADO_Objects();
    }
    public static void Update(Employee e)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand($"Select * from Employees Where ID={e.ID}", connection);
        MappingAndFill();

        adapter!.UpdateCommand = new SqlCommand("Update Employees Set Names=@Name, DateOfBirth=@DateOfBirth Where ID=@ID", connection);
        adapter!.UpdateCommand.Parameters.AddRange(
            new SqlParameter[]{
                new SqlParameter("Name", SqlDbType.NVarChar, 255, "names"),
                new SqlParameter("DateOfBirth", SqlDbType.Date) {SourceColumn="DateOfBirth"},
                new SqlParameter("ID", SqlDbType.Int) { SourceColumn = "ID"}
            }
        );
        DataRow r = empTable!.Rows[0];
        r[1] = e.Name;
        r[2] = e.DateOfBirth;
        adapter!.Update(dataSet!, "Employees");

        Dispose.ADO_Objects();
    }
    public static void Delete(int id)
    {
        InitADO_Object();
        adapter!.SelectCommand = new SqlCommand($"Select ID from Employees Where ID={id}", connection);
        MappingAndFill();
        adapter!.DeleteCommand = new SqlCommand($"Delete Employees Where ID={id}", connection);
        if (empTable!.Rows.Count > 0)
        {
            empTable!.Rows[0].Delete();
        }
        adapter.Update(dataSet!, "Employees");
        Dispose.ADO_Objects();
    }

    // static string UpdateCmdBuilder()
    // {
    //     StringBuilder cmd = new StringBuilder("Update Employee Set ");
    //     List<string> colNames = new List<string>();
    //     List<string> cmdParams = new List<string>();
    //     // empTable!.PrimaryKey = new DataColumn[] { empTable.Columns[0]};
    //     foreach (DataColumn col in empTable!.Columns)
    //     {
    //         if (!col.Unique)
    //         {
    //             colNames.Add(col.ColumnName);
    //             cmdParams.Add($"@{col.ColumnName}");
    //         }
    //     }
    //     for (int i = 1; i < colNames.Count(); i++)
    //     {
    //         cmd.Append(colNames[i] + "=" + cmdParams[i] + " ");
    //     }
    //     cmd.Append($"Where {colNames[0]}={cmdParams[0]}");

    //     return cmd.ToString();
    // }

}
public class Dispose
{
    public static void ADO_Objects()
    {
        Debug.WriteLine("Tien hanh dispose: SqlConnection, SqlDataAdapter, DataSet, DataTable");
        EmployDb.connection!.Dispose();
        EmployDb.adapter!.Dispose();
        EmployDb.dataSet!.Dispose();
        EmployDb.empTable!.Dispose();
    }
}