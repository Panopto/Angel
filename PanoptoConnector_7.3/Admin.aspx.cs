using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Web;

namespace Panopto.External.Angel
{

    public partial class Admin : PanoptoConnectorPage
    {
        /// <summary>
        /// Override GetAuthenticationStatus for complex access check.
        /// </summary>
        protected override AuthenticationStatus GetAuthenticationStatus()
        {
            RequireSystemAdminRights = true;

            return base.GetAuthenticationStatus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load global settings from environment vars.
                InstanceNameTextBox.Text = Util.GetInstanceName();
                ServerTextBox.Text = Util.GetServerName();
                AppKeyTextBox.Text = Util.GetAppKey();
                NotificationsCheckBox.Checked = Util.GetNotify();
            }
        }

        protected void SaveSettings(object sender, EventArgs e)
        {
            // Update global environment vars from text boxen
            Util.SetInstanceName(InstanceNameTextBox.Text);
            Util.SetServerName(ServerTextBox.Text);
            Util.SetAppKey(AppKeyTextBox.Text);
            Util.SetNotify(NotificationsCheckBox.Checked);

            // Show confirmation text
            SaveConfirmationMessage.Visible = true;
        }
    }

}