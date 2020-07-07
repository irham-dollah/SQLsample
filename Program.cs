using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQLServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            String sql;
            StringBuilder sb = new StringBuilder();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";
                builder.UserID = "sa";
                builder.Password = "pa$$word";

                Console.Write("Connecting to SQL server . . .");
                using (SqlConnection con = new SqlConnection(builder.ConnectionString))
//                using (SqlConnection con = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Am\Documents\sample.mdf; Integrated Security = True; Connect Timeout = 30"))//localhost
                {
                    con.Open();
                    Console.WriteLine("Connected");

                    Console.Write("\nDropping and creating database 'SampleDB' ... ");
                    sql = "DROP DATABASE IF EXISTS [sampleDB]; CREATE DATABASE [sampleDB]";
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        com.ExecuteNonQuery();
                        Console.WriteLine("Database Created");
                    }

                    Console.WriteLine("\nCreating table . . .");
                    starting();
                    
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        com.ExecuteNonQuery();
                        Console.WriteLine("Table Created");
                    }
                    ending();

                    int choice=0;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1-New 2-Update 3-Delete 4-Display 5-Exit");
                        Console.Write("Choose one number (1-5): ");
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1: insert();break;
                            case 2: update();break;
                            case 3: delete();break;
                            case 4: display();break;
                            case 5: break;
                            default:choice = 0; continue;
                        }
                    } while (choice <5 && choice >= 0);
                    
                    void insert()
                    {
                        Console.WriteLine("\nInsert row ...");
                        Console.Write("Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Location: ");
                        string loc = Console.ReadLine();
                        Console.WriteLine("...");
                        sb.Clear();
                        sb.Append("INSERT Employees (Name, Location) ");
                        sb.Append("VALUES (@name, @location);");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@location", loc);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected + " row(s) inserted");
                        }
                        ending();
                    }
                    void update()
                    {
                        Console.WriteLine("\nUpdate row ...");
                        Console.Write("Enter id: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Updating 'Location' for user  '" + id + "'");
                        Console.Write("Enter location: ");
                        string loc = Console.ReadLine();
                        sb.Clear();
                        sb.Append("UPDATE Employees SET Location = @location WHERE id = @id");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@location", loc);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected + " row(s) updated");
                        }
                        ending();
                    }
                    void delete()
                    {
                        Console.WriteLine("\nDelete row ...");
                        Console.Write("Enter id: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.WriteLine("Deleting user  '" + id + "'");
                        sb.Clear();
                        sb.Append("DELETE FROM Employees WHERE id = @id;");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected + " row(s) deleted");
                        }
                        ending();
                    }
                    void display()
                    {
                        Console.WriteLine("\nDisplay All ...");
                        Console.WriteLine("Reading data from table...");
                        sql = "SELECT id, Name, Location FROM Employees;";
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                                }
                            }
                        }
                        ending();
                    }
                    void starting()
                    {
                        sb.Append("USE SampleDB;");
                        sb.Append("CREATE TABLE Employees(");
                        sb.Append("id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
                        sb.Append("Name NVARCHAR(50), ");
                        sb.Append("Location NVARCHAR(50));");
                        Console.Write("... ");
                        sb.Append("INSERT INTO Employees(Name,Location) VALUES ");
                        sb.Append("(N'Irham',N'Damansara'),");
                        sb.Append("(N'Abu',N'Gombak');");
                        sql = sb.ToString();
                    }
                    void ending()
                    {
                        Console.WriteLine("Done.");
                        Console.WriteLine("\nPress any key to continue.");
                        Console.ReadKey();
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }

            Console.WriteLine("\nAll done. Thank you.");
        }
    }
}
