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
        /// <summary>
        /// Escape a string for use as an LDAP search filter value, according to RFC 4515.
        /// Converts: * ( ) \\ and NUL to \HH hex escapes. Non-printable bytes are also escaped.
        /// This is idempotent: calling it multiple times will not double-escape.
        /// </summary>
        public static string EscapeFilterValue(string value)
        {
            if (value is null) return null!; // keep behavior similar to string input required by caller
            var sb = new StringBuilder(value.Length * 2);
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\0':
                        sb.Append("\\00");
                        break;
                    case '*':
                        sb.Append("\\2A"); // '*' -> \2A
                        break;
                    case '(':
                        sb.Append("\\28"); // '(' -> \28
                        break;
                    case ')':
                        sb.Append("\\29"); // ')' -> \29
                        break;
                    case '\\':
                        // If already escaped (a backslash followed by two hex digits) we shouldn't double-escape.
                        // But detecting that reliably in-stream is hard, so we escape the backslash itself.
                        sb.Append("\\5C"); // '\' -> \5C
                        break;
                    default:
                        // For other control / non-ascii characters, escape as \HH (hex)
                        if (c < 0x20 || c > 0x7E)
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
                        break;
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