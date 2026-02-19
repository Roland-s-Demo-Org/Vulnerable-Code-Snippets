using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Text;

namespace WebFox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LDAP : ControllerBase
    {
        // Escape a string for use as an LDAP search filter value, according to RFC 4515.
        public static string EscapeFilterValue(string value)
        {
            if (value is null) return null!;
            var sb = new StringBuilder(value.Length * 2);
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (c == '\0')
                {
                    sb.Append("\\00");
                }
                else if (c == '*')
                {
                    sb.Append("\\2A");
                }
                else if (c == '(')
                {
                    sb.Append("\\28");
                }
                else if (c == ')')
                {
                    sb.Append("\\29");
                }
                else if (c == '\\') {
                    sb.Append("\\5C");
                }
                else if (c < 0x20 || c > 0x7E)
                {
                    // encode as UTF-8 bytes and escape each byte
                    // This preserves non-ASCII safely for LDAP filters.
                    var bytes = Encoding.UTF8.GetBytes(new[] { c });
                    foreach (var b in bytes)
                    {
                        sb.Append('\\');
                        sb.Append(b.ToString("X2"));
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        [HttpGet("{user}")]
        public void LdapInje(string user)
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://DC=mycompany,DC=com");
            DirectorySearcher searcher = new DirectorySearcher(de);
            searcher.Filter = "(&(objectClass=user)(|(cn=" + EscapeFilterValue(user) + ")(sAMAccountName=" + EscapeFilterValue(user) + ")))"; //When I'm concatenating the user name, here I got the security flag which is below.

            SearchResult result = searcher.FindOne();
        }
    }
}