using nicoblogproject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicoblogproject.Data
{
    public class DbInitializer
    {
        public static void Initialize(UserContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new ApplicationUser[]
            {
                new ApplicationUser{ApplicationUserID=1, Username="Nicolai", Email="nicolaihedegaardjensen93@gmail.com",Password="000000"},
                new ApplicationUser{ApplicationUserID=2, Username="Nicolai Jensen", Email="nicolai@domainname.com",Password="000000"}
            };

            foreach(ApplicationUser u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
