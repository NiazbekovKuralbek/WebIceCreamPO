using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Web.Application.Models;

namespace Web.Application.Data;

public partial class DataBaseContext
{
    public static SqlConnection Connection
           = new SqlConnection("Data Source=ONIGIRI;Initial Catalog=IceCream;Persist Security Info=True;User ID=sa;Password=1111");

    public static SqlCommand Command = null;
    public static SqlDataReader Reader = null;
    public static SqlDataAdapter Adapter = null;

    public static void OpenConnection()
    {
        try
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                Console.WriteLine("The Connection is " + Connection.State.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection failed " + ex.Message);
        }
    }

    public static void CloseConnection()
    {
        try
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                Console.WriteLine("The Connection is " + Connection.State.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Close connection error " + ex.Message);
        }
    }
}