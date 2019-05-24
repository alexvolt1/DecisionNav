using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Relational
{
    public partial class DeciwebPageBase : System.Web.UI.Page
    {
        public bool HasAccess { get; set; }
        private void InitializeComponent()
        {

            this.Init += new System.EventHandler(this.Page_Init);

        }

        public void Page_Init(object sender, EventArgs e)
        {
            if (DeciwebProperties.DeciwebSession.Current.UserName==null)
            {
                Response.Redirect("Login.aspx");
            }



           // if (DeciwebProperties.DeciwebSession.Current.HasLegal == true && DeciwebProperties.DeciwebSession.Current.LegalAccepted == false)
           // {
            //    HasAccess = false;
           //     this.Visible = false;
           //     Response.End();
          //  }
          //  else
           // {
                HasAccess = true;

           // }
        }


    }
}