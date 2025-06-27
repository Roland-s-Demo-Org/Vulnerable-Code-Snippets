
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
            if (!Path.GetFullPath(binFile).StartsWith(Path.GetFullPath("/allowed/bin/"), StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Access to the specified file is not allowed.");
            p.StartInfo.FileName = Path.GetFullPath(binFile);
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}