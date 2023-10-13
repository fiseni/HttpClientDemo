using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HttpClientDemo.WebForms
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["test"] = "test";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var service = new ClientNetFX6.Service64();
            var result = service.GetEntries();
            TextBox1.Text = result;
        }
    }
}