using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static MyStore2.Pages.Persons.IndexModel;

namespace MyStore2.Pages.Persons
{
    public class EditModel : PageModel
    {
        public PersonInfo personInfo = new PersonInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["userid"];
            String connectionString = "Data Source=hvdev-sql-01;Initial Catalog=dev_cosd_nadeem;Persist Security Info=True;User ID=sa;Password=Riley++";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlQuery = "SELECT * FROM Persons WHERE userid=@userid";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("userid", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                personInfo.userid = "" + reader.GetInt32(0);
                                personInfo.firstName = reader.GetString(1);
                                personInfo.lastName = reader.GetString(2);
                                personInfo.address = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

        }

        public void OnPost()
        {
            personInfo.userid = Request.Form["userid"];
            personInfo.firstName = Request.Form["firstName"];
            personInfo.lastName = Request.Form["lastName"];
            personInfo.address = Request.Form["address"];

            if (personInfo.firstName.Length == 0 || personInfo.lastName.Length == 0 || personInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //String id = Request.Query["id"];
            String connectionString = "Data Source=hvdev-sql-01;Initial Catalog=dev_cosd_nadeem;Persist Security Info=True;User ID=sa;Password=Riley++";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sqlQuery = "UPDATE persons " + "SET FirstName=@firstName, LastName=@lastName, Address=@address " +
                        "WHERE userid=@userid";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.Parameters.AddWithValue("@firstName", personInfo.firstName);
                        command.Parameters.AddWithValue("@lastName", personInfo.lastName);
                        command.Parameters.AddWithValue("@address", personInfo.address);
                        command.Parameters.AddWithValue("@userid", personInfo.userid);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            Response.Redirect("/Persons/Index");
        }
    }
}
