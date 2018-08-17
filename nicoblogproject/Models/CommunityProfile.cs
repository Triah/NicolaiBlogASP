using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace nicoblogproject.Models
{
    public class CommunityProfile
    {
        public int CommunityProfileID { get; set; }
        public string CommunityProfileUsername { get; set; }
        public string CommunityProfileSummary { get; set; }
        public string CommunityProfileSummaryAdded { get; set; }
        public string CommunityProfileEmail { get; set; }
        public string CommunityProfileEmailAdded { get; set; }
        public string CommunityProfileCountry { get; set; }
        public string CommunityProfileCountryAdded { get; set; }
        public int CommunityProfileAge { get; set; }
        public string CommunityProfileAgeAdded { get; set; }
        public string CommunityProfileOccupation { get; set; }
        public string CommunityProfileOccupationAdded { get; set; }
        public string CommunityProfileDisplayImage { get; set; }

        public int SaveDetails()
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "INSERT INTO CommunityProfile(CommunityProfileID," +
                "CommunityProfileUsername," +
                "CommunityProfileSummary," +
                "CommunityProfileSummaryAdded," +
                "CommunityProfileEmail," +
                "CommunityProfileEmailAdded," +
                "CommunityProfileCountry," +
                "CommunityProfileCountryAdded," +
                "CommunityProfileAge," +
                "CommunityProfileAgeAdded," +
                "CommunityProfileOccupation," +
                "CommunityProfileOccupationAdded," +
                "CommunityProfileDisplayImage) values ('" + 
                CommunityProfileID + "','" + 
                CommunityProfileUsername + "','" + 
                CommunityProfileSummary + "','" + 
                CommunityProfileSummaryAdded + "','" +
                CommunityProfileEmail + "','" + 
                CommunityProfileEmailAdded+ "','" +
                CommunityProfileCountry + "','" +
                CommunityProfileCountryAdded + "','" +
                CommunityProfileAge + "','" +
                CommunityProfileAgeAdded + "','" +
                CommunityProfileOccupation + "','" +
                CommunityProfileOccupationAdded + "','"+
                CommunityProfileDisplayImage + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateSummary(string Summary, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileSummary = '" + Summary + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateDisplayImage(string URL, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileDisplayImage = '" + URL + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateSummaryAdded(string SummaryAdded, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileSummaryAdded = '" + SummaryAdded + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateEmail(string Email, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileEmail = '" + Email + "' " + 
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateEmailAdded(string EmailAdded, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileEmailAdded ='" + EmailAdded + "' " +
                "WHERE CommunityProfileUsername = '" + Username +"';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateCountry(string Country, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileCountry = '" + Country + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateCountryAdded(string CountryAdded, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileCountryAdded = '" + CountryAdded + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateAge(string Age, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileAge = '" + Age + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateAgeAdded(string AgeAdded, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileAgeAdded = '" + AgeAdded + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateOccupation(string Occupation, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileOccupation = '" + Occupation + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int UpdateOccupationAdded(string OccupationAdded, string Username)
        {
            SqlConnection con = new SqlConnection("Server = (localdb)\\mssqllocaldb; Database = NicolaiBlogDatabase; Trusted_Connection = True; MultipleActiveResultSets = true");
            string query = "UPDATE CommunityProfile " +
                "SET CommunityProfileOccupationAdded = '" + OccupationAdded + "' " +
                "WHERE CommunityProfileUsername = '" + Username + "';";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }


    }
}
