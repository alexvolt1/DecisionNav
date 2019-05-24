using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using DeciwebProperties;
using System.Net;
using System.Deployment.Application;
using System.Reflection;
namespace Relational
{
    public partial class Login : System.Web.UI.Page
    {
        public string AssemblyProductVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
           string UserName = uid.Text.Length >= 50 ? (string)(uid.Text).Substring(0, 50) : (string)(uid.Text);
            DeciwebSession.Current.UserName = UserName;
            Response.Redirect("Default.aspx");
        }

        private bool isValidUserName(string username)
        {
            if (username == "")
            {
                return false;
            }
            return true;
        }
        private bool isValidPassword(string password)
        {
            if (password == "")
            {
                return false;
            }
            return true;
        }
    }
}