using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml;

namespace WebFox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XxeTest1 : ControllerBase
    {

        [HttpGet("{xmlString}")]
        public void DoXxe(String xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            
            // XXE Mitigation: Configure secure XML reader settings
            // DtdProcessing.Ignore prevents processing of external entities that could lead to XXE attacks
            var settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            
            // Load XML through a secure XmlReader instead of using LoadXml directly
            // This ensures the secure settings are applied during parsing
            using (var stringReader = new System.IO.StringReader(xmlString))
            using (var xmlReader = XmlReader.Create(stringReader, settings))
            {
                xmlDoc.Load(xmlReader);
            }
        }
    }
}