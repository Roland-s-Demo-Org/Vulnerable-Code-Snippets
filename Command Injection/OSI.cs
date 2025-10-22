
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace WebFox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsInjection : ControllerBase
    {
        [HttpGet("{binFile}")]
        public string os(string binFile)
        {
            Process p = new Process();
            string allowedPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "allowed_binaries", binFile));
            if (!allowedPath.StartsWith(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "allowed_binaries"))))
                throw new UnauthorizedAccessException("Access to the specified binary is not allowed.");
            if (!File.Exists(allowedPath))
                throw new FileNotFoundException("Binary not found.");
            p.StartInfo.FileName = allowedPath;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}