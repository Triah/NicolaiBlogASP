using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace nicoblogproject.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleCreationTime { get; set; }
        public string ArticleAuthor { get; set; }
        public string ArticleContent { get; set; }

        public int SaveDetails()
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "INSERT INTO Article(ArticleID, ArticleTitle, ArticleCreationTime, ArticleAuthor, ArticleContent) values ('" + ArticleID + "','" + ArticleTitle + "','" + ArticleCreationTime + "','" + ArticleAuthor + "','" + ArticleContent + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
    }
}
