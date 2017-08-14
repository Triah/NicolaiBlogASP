using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace nicoblogproject.Models
{
    public class ApplicationUser
    {
        public int ApplicationUserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayImage { get; set; }
        public string Type { get; set; }
        public string Salt { get; set; }

        public int SaveDetails()
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "INSERT INTO ApplicationUser(ApplicationUserID, Username, Email, Password, DisplayImage, Type, Salt) values ('" + ApplicationUserID + "','" + Username + "','" + Email + "','" + Password + "'," + "null" + ",'" + Type + "','" + Salt + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
    }

}