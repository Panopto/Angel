using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;
using CLL.S3.Angel.Web;
using CLL.S3.Framework;

namespace Panopto.External.Angel
{
    public partial class SSO : CLL.S3.Angel.Web.Page
    {
        // Allow unauthenticated users.
        protected override AuthenticationStatus GetAuthenticationStatus()
        {
            if (AngelSession.CurrentAngelSession == null)
            {
                Response.Redirect("~/Portal/Nuggets/PanoptoConnector/Login.asp?ReturnUrl=" + Server.UrlEncode(Request.RawUrl));
            }

            return AuthenticationStatus.Authenticated;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AfterAuthentication(AuthenticationStatus.Authenticated);

            string serverName = Request.QueryString["serverName"];
            string callbackUrl = Request.QueryString["callbackURL"];
            string expiration = Request.QueryString["expiration"];
            string authCode = Request.QueryString["authCode"];
            string action = Request.QueryString["action"];

            bool relogin = (action == "relogin");

            if(relogin)
            {
                //BUGBUG: Implement relogin
            }

            string userKey = Util.GetUserKey();

        	// Generate canonically-ordered auth payload string
            string responseParams = String.Format("serverName={0}&externalUserKey={1}&expiration={2}", serverName, userKey, expiration);

            // Sign payload with shared key and hash.
            string responseAuthCode = Util.GetAuthCode(responseParams);
	
        	string separator = callbackUrl.Contains("?") ? "&" : "?";
            string redirectUrl = callbackUrl + separator + responseParams + "&authCode=" + responseAuthCode;  
	
        	// Redirect to Panopto login page.
	        Response.Redirect(redirectUrl);
        }
    }
}
