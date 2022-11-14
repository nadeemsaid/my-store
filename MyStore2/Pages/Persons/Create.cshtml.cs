using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static MyStore2.Pages.Persons.IndexModel;

namespace MyStore2.Pages.Persons
{
    public class CreateModel : PageModel
    {
        public PersonInfo personInfo = new PersonInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            personInfo.firstName = Request.Form["firstName"];
            personInfo.lastName = Request.Form["lastName"];
            personInfo.address = Request.Form["address"];

            if (personInfo.firstName.Length == 0 || personInfo.lastName.Length == 0 || personInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=hvdev-sql-01;Initial Catalog=dev_cosd_nadeem;Persist Security Info=True;User ID=sa;Password=Riley++";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQ = "INSERT INTO Persons " + "(firstName, lastName, address) VALUES"
                        + "(@firstName, @lastName, @address);";

                    using (SqlCommand command = new SqlCommand(sqlQ, connection))
                    {
                        command.Parameters.AddWithValue("@firstName", personInfo.firstName);
                        command.Parameters.AddWithValue("@lastName", personInfo.lastName);
                        command.Parameters.AddWithValue("@address", personInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }


            personInfo.firstName = ""; personInfo.lastName = ""; personInfo.address = "";
            successMessage = "New Client Added Successfully";

            Response.Redirect("/Persons/Index");



        }

    }
}
