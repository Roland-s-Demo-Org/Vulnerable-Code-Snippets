using Microsoft.AspNetCore.Mvc;

namespace WebFox.Controllers.PathTraversal
{
    public class PathTraversalTest1 : ControllerBase
    {
        [HttpGet("{path}")]
        public void Test(string path)
        {
            if (path == null || path.Contains("../") || path.Contains(@"..\"))
            {
                throw new ArgumentException("Invalid file path");
            }
            System.IO.File.Delete(path);
        }


    }
}