using Nito.AsyncEx.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace HttpClientDemo.WebForms.WebServices
{
    /// <summary>
    /// Summary description for DemoWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DemoWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string RunOption63()
        {
            var response = new ClientNetFX6.Service63().GetStatusCode();
            return response;
        }

        [WebMethod]
        public IAsyncResult BeginRunAsync(string option, AsyncCallback callback, object state)
        {
            return ApmAsyncFactory.ToBegin<string>(ClientNetFX.Program.GetResponseAsync(option), callback, state);

        }
        [WebMethod]
        public string EndRunAsync(IAsyncResult result)
        {
            return ApmAsyncFactory.ToEnd<string>(result);
        }
    }
}
