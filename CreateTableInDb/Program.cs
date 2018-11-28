using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CreateTableInDb
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder cnStrBuilder = new SqlConnectionStringBuilder();
            Console.Write("Введите SQl Server: ");
            cnStrBuilder.DataSource = Console.ReadLine();
            Console.Write("Введите название базы данных: ");
            cnStrBuilder.InitialCatalog = Console.ReadLine();
            cnStrBuilder.IntegratedSecurity = true;

            using (SqlConnection cnSql = new SqlConnection(cnStrBuilder.ConnectionString))
            {
                try
                {
                    cnSql.Open();
                }
                catch(SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

                string createTb = "Create table gruppa(id int not null primary key, name nvarchar(30) not null);";

                StringBuilder insert = new StringBuilder("Insert into gruppa values(1, 'Дамир')");
                insert.Append("Insert into gruppa values(2, 'Куат')");
                insert.Append("Insert into gruppa values(3, 'Талгат')");
                insert.Append("Insert into gruppa values(4, 'Тимур')");

                string select = "Select * from gruppa";

                SqlCommand cmdCreate = new SqlCommand(createTb, cnSql);
                SqlCommand cmdInsert = new SqlCommand(insert.ToString(), cnSql);
                SqlCommand cmdSelect = new SqlCommand(select, cnSql);

                try
                {
                    cmdCreate.ExecuteNonQuery();
                    cmdInsert.ExecuteNonQuery();

                    using (SqlDataReader dr = cmdSelect.ExecuteReader())
                    {
                        Console.WriteLine("\n{0}\t{1}", dr.GetName(0), dr.GetName(1));
                        Console.WriteLine("----------------");
                        while (dr.Read())
                        {
                            Console.WriteLine("{0}\t{1}", dr["id"], dr["name"]);
                        }
                    }

                    Console.WriteLine("\nТаблица успешно создана!!!");
                }
                catch(SqlException ex)
                {
                    if(ex.Number == 2714)
                    {
                        using (SqlDataReader dr = cmdSelect.ExecuteReader())
                        {
                            Console.WriteLine("\n{0}\t{1}", dr.GetName(0), dr.GetName(1));
                            Console.WriteLine("----------------");
                            while (dr.Read())
                            {
                                Console.WriteLine("{0}\t{1}", dr["id"], dr["name"]);
                            }
                        }
                    }
                    else
                        Console.WriteLine(ex.Message);
                }                             
            }
        }
    }
}
