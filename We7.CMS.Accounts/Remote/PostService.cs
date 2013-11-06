﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace We7.CMS.Accounts
{
    public class PostService
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            string html = string.Empty;
            html = "<html><head>";
            html += string.Format("</head><body onload=\"document.{0}.submit()\">正在连接服务器验证您的身份...", FormName);
            html += string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\">", FormName, Method, Url);
            try
            {
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    html += string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]);
                }
                html += "</form></head>";
                html += "</html>";
                System.Web.HttpContext.Current.Response.Write(html);
                System.Web.HttpContext.Current.Response.End();
            }
            catch
            { }
        }
    }
}
