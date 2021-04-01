using System;

namespace Model
{
   public class Credentials
   {
       public string Username { get; set; }
       public string Password { get; set; }
       public RoleType Role { get; set; }
       public Credentials(string user, string pass, RoleType r)
       {
           this.Username = user;
           this.Password = pass;
           this.Role = r;
       }
    }
}