using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace AdAuthentication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string domainName = "disys.in"; //this should be moved to config file
                string userName = "saravana.kumar"; //put your username here
                string password = "********"; //put your password here

                //This will validate the user
                LdapConnection connection = new LdapConnection(domainName);
                NetworkCredential credential = new NetworkCredential(userName, password);
                connection.Credential = credential;
                connection.Bind(); //this line will throw exception if credentails are not valid

                //Next step is the get additional details of the user
                UserDetails userInfo = new UserDetails();
                userInfo = userInfo.GetDetails(domainName, userName, password);

                Console.WriteLine("Logged in User: {0} \nFirst Name: {1}\nLast Name: {2}",
                    userInfo.DisplayName, userInfo.FirstName, userInfo.LastName);

                Console.ReadLine();
            }
            catch (LdapException ldapException)
            {
                String error = ldapException.ServerErrorMessage;
                Console.WriteLine(ldapException);
                //525​ user not found ​(1317)
                //52e​ invalid credentials ​(1326)
                //530​ not permitted to logon at this time​ (1328)
                //531​ not permitted to logon at this workstation​ (1329)
                //532​ password expired ​(1330)
                //533​ account disabled ​(1331)
                //701​ account expired ​(1793)
                //773​ user must reset password (1907)
                //775​ user account locked (1909)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
