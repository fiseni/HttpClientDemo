using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HttpClientDemo.WebForms.HttpHandlers
{
    public class DemoHttpHandlerAsync : HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var option = context.Request.QueryString["option"];
            var response = await ClientNetFX.Program.GetResponseAsync(option);
            context.Response.Write(response);
        }
    }
}
