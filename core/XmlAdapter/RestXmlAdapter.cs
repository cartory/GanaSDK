using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Net;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GanaSDK.XmlAdapter
{
    public class RestRequestOptions
    {
        public static RestRequestOptions DefaultOptions => new RestRequestOptions();

        public int Timeout = -1;

        public string Body = "{}";
        public string Method = "GET";
        public string XmlTag = "XmlResponse";
        public string Accept = "application/json";
        public string ContentType = "application/json; charset=UTF-8";

        public Func<string, object> Result;
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
    }

    public class RestXmlAdapter : XmlAdapter
    {
        public string Fetch(string URL, RestRequestOptions options)
        {
            try
            {
                options = options ?? RestRequestOptions.DefaultOptions;

                string json = this.CallRestAPI(URL, options);
                object result = options.Result(json) ?? json;

                return this.ToXml(options.XmlTag, result);
            }
            catch (Exception e)
            {
                throw new Exception($"Error on Fetch => {e.Message}");
            }
        }

        private string CallRestAPI(string URL, RestRequestOptions options)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(options.Body);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);

            req.Accept = options.Accept;
            req.Method = options.Method;
            req.ContentType = options.ContentType;

            req.ContentLength = jsonBytes.Length;

            foreach (KeyValuePair<string, string> header in options.Headers)
            {
                req.Headers.Add(header.Key, header.Value);
            }

            Stream reqStream = req.GetRequestStream();
            reqStream.Write(jsonBytes, 0, jsonBytes.Length);
            reqStream.Close();

            WebResponse res = req.GetResponse();
            Stream resStream = res.GetResponseStream();

            string jsonRes = new StreamReader(resStream).ReadToEnd();

            res.Close();
            resStream.Close();

            return jsonRes;
        }
    }
}