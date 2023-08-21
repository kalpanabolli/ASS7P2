using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ass7
{
    internal class Program
    {

        static SqlCommand cmd;
        static SqlConnection con;
        static SqlDataAdapter adapter;
        static DataSet ds;
        static string conStr = "server=DESKTOP-898SEC1;database=LibraryDB; trusted_connection=true;";
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Find out 1.view 2.Insert 3.Update \nEnter your choice 1,2 or 3");

                int choice = int.Parse(Console.ReadLine());



                switch (choice)
                {
                    case 1:
                        ViewBooks();
                        break;
                    case 2:
                        InsertBook();
                        break;
                    case 3:
                        UpdateBook();
                        break;
                    case 4:
                        con.Close();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
                Console.ReadKey();
            }


        }

        static DataSet ViewBooks()
        {
            con = new SqlConnection(conStr);
            cmd = new SqlCommand("select * from Books", con);
            adapter = new SqlDataAdapter(cmd);
            con.Open();
            ds = new DataSet();
            adapter.Fill(ds);
            con.Close();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine("BookID: " + ds.Tables[0].Rows[i]["BookId"]);
                Console.WriteLine("Title : " + ds.Tables[0].Rows[i]["Title"]);
                Console.WriteLine("Author : " + ds.Tables[0].Rows[i]["Author"]);
                Console.WriteLine("Genre : " + ds.Tables[0].Rows[i]["Genre"]);
                Console.WriteLine("Quantity : " + ds.Tables[0].Rows[i]["Quantity"]);
            }
            return ds;
        }
        static void InsertBook()
        {
            try
            {

                con = new SqlConnection(conStr);
                con.Open();
                cmd = new SqlCommand();
                cmd.CommandText = "insert into Books (Title,Author,Genre,Quantity) values (@Title,@author,@Genre,@quantity)";
                cmd.Connection = con;
                // Console.WriteLine("Enter BookID:");
                //cmd.Parameters.AddWithValue("@id", int.Parse(Console.ReadLine()));
                Console.WriteLine("Enter Title:");
                cmd.Parameters.AddWithValue("@Title", Console.ReadLine());
                Console.WriteLine("Enter Author:");
                cmd.Parameters.AddWithValue("@author", Console.ReadLine());
                Console.WriteLine("Enter Genre:");
                cmd.Parameters.AddWithValue("@Genre", Console.ReadLine());
                Console.WriteLine("Enter quantity");
                cmd.Parameters.AddWithValue("@quantity", int.Parse(Console.ReadLine()));

                cmd.ExecuteNonQuery();
                Console.WriteLine("Book Added successfully");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error!!!" + ex.Message);
            }

        }

        static void UpdateBook()
        {
            try
            {
                Console.Write("Enter Title of the Book to Update: ");
                string title = Console.ReadLine();
                Console.Write("Enter New Quantity: ");
                int newQuantity = Convert.ToInt32(Console.ReadLine());

                DataSet ds = ViewBooks();
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    if (string.Equals(row["Title"].ToString(), title, StringComparison.OrdinalIgnoreCase))
                    {
                        row["Quantity"] = newQuantity;
                        UpdateDatabase(ds);
                        Console.WriteLine("Quantity updated successfully.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating book quantity: " + ex.Message);
            }

        }
        static void UpdateDatabase(DataSet ds)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    string query = "select*from Books";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the database: " + ex.Message);
            }
        }
    }
}