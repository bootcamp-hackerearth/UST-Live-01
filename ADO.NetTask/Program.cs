using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ProductDb;User Id=Suhana;Password=suha@123;TrustServerCertificate=True";

        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{
        //    conn.Open();
        //    Console.WriteLine("Connection Successful!");
        //}
        //SqlConnection conn = new SqlConnection(connectionString);

        //try
        //{
        //    conn.Open();
        //    Console.WriteLine("Connection Successful!");
        //}
        //finally
        //{
        //    conn.Close();
        //}

        //Using block is better because it automatically
        //disposes the connection even if an exception occurs.

        //try
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        Console.WriteLine("Connection Successful!");
        //    }
        //}
        //catch (SqlException ex)
        //{
        //    Console.WriteLine("Connection failed: " + ex.Message);
        //}
        //for (int i = 1; i <= 3; i++)
        //{
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        Console.WriteLine($"Iteration {i}: Opened");
        //    }

        //    Console.WriteLine($"Iteration {i}: Closed");
        //}
        //SqlConnection conn = new SqlConnection(connectionString);

        //try
        //{
        //    using (conn)
        //    {
        //        conn.Open();
        //        Console.WriteLine("Connected");

        //        throw new Exception("Simulated error");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        //Console.WriteLine("Connection State: " + conn.State);

        SqlConnection conn = new SqlConnection(connectionString);

        using (SqlCommand cmd = new SqlCommand(
            "UPDATE Products SET Price = @Price WHERE ProductName = @Name", conn))
        {
            cmd.Parameters.AddWithValue("@Price", 999m);
            cmd.Parameters.AddWithValue("@Name", "Mouse");

            int rows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Rows updated: {rows}");
        }

    }
}



