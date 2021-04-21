using System;
using System.DirectoryServices;

namespace AdAuthentication
{
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public UserDetails GetDetails(string domainName, string userName, string password)
        {
            UserDetails user = null;
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + domainName, userName, password);
                DirectorySearcher directorySearch = new DirectorySearcher(directoryEntry);
                directorySearch.Filter = "(samAccountName=" + userName + ")";
                SearchResult searchResult = directorySearch.FindOne();
                DirectoryEntry result = searchResult.GetDirectoryEntry();

                /*
                 * User attributes inside active directory
                 * http://www.kouti.com/tables/userattributes.htm
                 * 1. However, DISYS has only stored 33 attributes and the rest like Title, Designation, Manager, Email, Mobile
                 * are stored only in Zoho People
                 * 2. So, we must identify an alternate way to store this information in the backend of this application
                 * and retrieve when needed
                 */
                if (result != null)
                {
                    user = new UserDetails()
                    {
                        FirstName = result.Properties["givenName"].Value.ToString(),
                        LastName = result.Properties["sn"].Value.ToString(),
                        DisplayName = result.Properties["displayName"].Value.ToString(),
                    };
                }

                return user;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return user;
            }
        }
    }}