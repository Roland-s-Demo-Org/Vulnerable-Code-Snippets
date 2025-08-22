
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
            if (!IsAllowedExecutable(binFile)) throw new ArgumentException("Invalid executable specified");
            p.StartInfo.FileName = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), binFile));
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}