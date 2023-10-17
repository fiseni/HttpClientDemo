using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpClientDemo.WebForms.HttpHandlers
{
    public class DemoHttpHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var response = new ClientNetFX6.Service63().GetStatusCode();
            context.Response.Write(response);
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
