using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;
using CLL.S3.Angel.Web;
using CLL.S3.Framework;
using CLL.S3.Framework.Exceptions;

namespace Panopto.External.Angel
{

    public partial class CourseSettings : PanoptoConnectorPage
    {
        const string cNuggetRoot = "/Portal/Nuggets/PanoptoConnector/";

        /// <summary>
        /// Override GetAuthenticationStatus for complex access check.
        /// </summary>
        protected override AuthenticationStatus GetAuthenticationStatus()
        {
            RequireCourseEditorRights = true;
        
            return base.GetAuthenticationStatus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Course currentCourse = AngelSession.CurrentAngelSession.Section;

                ProvisionLink.NavigateUrl = String.Format("{0}ProvisionCourse.aspx?id={1}", cNuggetRoot, currentCourse.CourseId);

                using (var clientData = new ClientDataProxy())
                {
                    List<CourseInfo> courses = new List<CourseInfo>(clientData.GetCourses(Util.GetUserKey(), Util.GetAuthCode()));

                    string panoptoCourseID = ConfigurationVariable.GetValue("PanoptoCourseID", null, null, null, currentCourse.CourseId, null);

                    if (String.IsNullOrEmpty(panoptoCourseID))
                    {
                        courses.Insert(0, new CourseInfo() { DisplayName = "-- Select an Existing Course --" });
                    }

                    bool isProvisioned = false;
                    bool canSync = false;

                    // Populate course drop down.
                    CoursesDropDown.Items.Clear();
                    foreach (CourseInfo course in courses)
                    {
                        ListItem item = new ListItem(course.DisplayName, course.PublicID.ToString());

                        // Select course if it matches the current Panopto course setting
                        if (course.PublicID.ToString() == panoptoCourseID)
                        {
                            item.Selected = true;
                        }

                        CoursesDropDown.Items.Add(item);

                        // Check for matching external ID indicating the course has already been provisioned
                        if (course.ExternalCourseID == Util.GetExternalCourseID(currentCourse.CourseId))
                        {
                            isProvisioned = true;

                            // If provisioned course is already selected, show "Sync users" link
                            if (course.PublicID.ToString() == panoptoCourseID)
                            {
                                canSync = true;
                            }
                        }
                    }

                    ProvisionPanel.Visible = (!isProvisioned || canSync);
                    ProvisionLink.Text =
                        canSync
                            ? "Sync Users"
                            : "Add this course to Panopto";
                }
            }
        }

        protected void Save(object sender, EventArgs args)
        {
            try
            {
                Course currentCourse = AngelSession.CurrentAngelSession.Section;

                Guid panoptoCourseID = new Guid(CoursesDropDown.SelectedValue);
                
                if (panoptoCourseID != Guid.Empty)
                {
                    Util.SetPanoptoCourseID(currentCourse.CourseId, panoptoCourseID);
                }
            }
            catch (Exception ex)
            {
                new FrameworkException("Error saving course setting.", ex, ExceptionSeverity.Warning);
            }

            Response.Redirect("~/section/home/default.asp");
        }
    }

}