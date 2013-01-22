using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;
using CLL.S3.Permissions.Angel;

namespace Panopto.External.Angel
{

    public partial class Admin : CLL.S3.Angel.Web.Page
    {
        protected override void SetupPrivileges()
        {
            AddRequiredPrivileges(AngelPrivileges.AdministratorConsole);
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