using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyStore2.Pages.Persons
{
    public class IndexModel : PageModel
    {
        public List<PersonInfo> personList = new List<PersonInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=hvdev-sql-01;Initial Catalog=dev_cosd_nadeem;Persist Security Info=True;User ID=sa;Password=Riley++";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM Persons";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var personInfo = new PersonInfo();
                                personInfo.userid = "" + reader.GetInt32(0);
                                personInfo.firstName = reader.GetString(1);
                                personInfo.lastName = reader.GetString(2);
                                personInfo.address = reader.GetString(3);
                                personInfo.created_at = reader.GetDateTime(4).ToString();

                                personList.Add(personInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public class PersonInfo
        {
            public string userid;
            public string firstName;
            public string lastName;
            public string address;
            public string created_at;

        }
    }
}