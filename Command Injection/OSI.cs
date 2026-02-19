
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
            if (!System.IO.File.Exists(binFile)) throw new ArgumentException("File does not exist");
            p.StartInfo.FileName = System.IO.Path.GetFullPath(binFile);
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}