
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
            string[] allowedBinaries = new string[] { "/usr/bin/whoami", "/bin/hostname" }; // Whitelist of allowed binaries
            if (!allowedBinaries.Contains(binFile))
                throw new UnauthorizedAccessException("Binary execution not allowed");
            Process p = new Process();
            p.StartInfo.FileName = binFile;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}