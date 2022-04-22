using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using System.Xml.Serialization;

namespace GanaSDK.XmlAdapter
{
    public static class CodeRes
    {
        public const string COD000 = "COD000";
        public const string COD003 = "COD003";
    }

    public abstract class RestResponse : JSON
    {
        public virtual object Result() => this.ToJson();

        public static object DefaultResult(string errMsg)
        {
            string code = string.IsNullOrEmpty(errMsg) ? CodeRes.COD000 : CodeRes.COD003;
            return new { CodRes = code, CodError = code, DesError = errMsg };
        }

        public static object DefaultResult(string errCode, string errMsg)
        {
            string code = string.IsNullOrEmpty(errCode) ? CodeRes.COD000 : CodeRes.COD003;
            return new { CodRes = code, CodError = code, DesError = errMsg };
        }
    }
}