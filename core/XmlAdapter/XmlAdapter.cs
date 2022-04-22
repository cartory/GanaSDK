using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GanaSDK.XmlAdapter
{
    public abstract class XmlAdapter
    {
        public virtual string ToXml(string tag, object json)
        {
            if (json is string)
            {
                return this.ToXml(tag, JsonConvert.DeserializeObject<Dictionary<string, string>>((string)json));
            }

            return this.ToXml(tag, JObject.FromObject(json).ToObject<Dictionary<string, string>>());
        }

        public virtual string ToXml(string tag, Dictionary<string, string> args)
        {
            XmlDocument xml = new XmlDocument();

            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement);

            XmlElement xmlResponse = xml.CreateElement(string.Empty, tag, string.Empty);
            xml.AppendChild(xmlResponse);

            Array.ForEach(args.Keys.ToArray(), key => {
                XmlText xmlText = xml.CreateTextNode(args[key]);
                XmlElement xmlTag = xml.CreateElement(string.Empty, key, string.Empty);

                xmlTag.AppendChild(xmlText);
                xmlResponse.AppendChild(xmlTag);
            });

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xml.WriteTo(xmlWriter);
                    xmlWriter.Flush();

                    return stringWriter.GetStringBuilder().ToString();
                }
            }
        }
    }
}