using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinqToXmlDemo
{
    /// <summary>
    /// Summary description for AcceptForm
    /// </summary>
    public class AcceptForm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            string action = context.Request.Form["btnSubmit"];
            if (action == "Submit Name")
            {
                context.Response.Write(context.Request.Form["Name"]);
            }
            else if (action == "Submit Age")
            {
                context.Response.Write(context.Request.Form["Age"]);
            }
            else
            {
                context.Response.Write("Hello World");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}