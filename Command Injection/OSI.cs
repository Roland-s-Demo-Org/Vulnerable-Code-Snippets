
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
            // Restrict to safe binaries in specific directory
            string safeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            string safeName = Path.GetFileName(binFile); // Strip any path components
            string fullPath = Path.Combine(safeDir, safeName);
            
            // Whitelist of allowed executables
            string[] allowedBinaries = new[] { "safe1.exe", "safe2.exe" };
            if (!allowedBinaries.Contains(safeName))
            {
                throw new UnauthorizedAccessException("Binary not in whitelist");
            }
            
            Process p = new Process();
            p.StartInfo.FileName = fullPath;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}