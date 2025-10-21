
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WebFox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsInjection : ControllerBase
    {
        [HttpGet("{binFile}")]
        public string os(string binFile)
        {
            string[] allowedExecutables = new[] { "notepad.exe", "calc.exe" };
            string safeFileName = Path.GetFileName(binFile);
            if (!allowedExecutables.Contains(safeFileName.ToLower()))
                return "Access denied: Executable not allowed";
            string executablePath = Path.Combine(@"C:\Windows\System32", safeFileName);
            if (!File.Exists(executablePath))
                return "Executable not found";
            Process p = new Process();
            p.StartInfo.FileName = executablePath;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.Dispose();
            return output;
        }
    }
}