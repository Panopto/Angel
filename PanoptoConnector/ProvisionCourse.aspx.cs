using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Web;
using CLL.S3.Framework;
using CLL.S3.Framework.Exceptions;

namespace Panopto.External.Angel
{
    public partial class ProvisionCourse : PanoptoConnectorPage
    {
        /// <summary>
        /// Override GetAuthenticationStatus for complex access check.
        /// </summary>
        protected override AuthenticationStatus GetAuthenticationStatus()
        {
            string courseID = Request.QueryString["id"];

            RequireSystemAdminRights = String.IsNullOrEmpty(courseID);
            RequireCourseEditorRights = true;

            return base.GetAuthenticationStatus();
        }

        protected List<string> CourseIDs;

        protected void Page_Load(object sender, EventArgs e)
        {
            string courseID = Request.QueryString["id"];

            if (!IsPostBack)
            {
                // Single-course provision
                if (!String.IsNullOrEmpty(courseID))
                {
                    BackLink.NavigateUrl = "CourseSettings.aspx";
                    BackLink.Text = "Back to course config page";

                    // Populate list with single course
                    CourseIDs = new List<string>() { courseID };

                    // Switch to provision results panel
                    CourseSelectPanel.Visible = false;
                    ProvisioningResultsPanel.Visible = true;
                }
                // Batch provision
                else
                {
                    BackLink.NavigateUrl = "Admin.aspx";
                    BackLink.Text = "Back to global config page";
                }
            }
        }

        protected void SearchCourses(object sender, EventArgs args)
        {
            // Default to error message.
            CourseCount.Text = "Error retrieving course list.";

            // Clear course list
            CourseList.Items.Clear();

            CourseCollection courses = null;

            // Title search terms (lowered for case-insensitive search)
            string titleSearch = CourseFilter.Text.ToLower();

            // Parse org unit Guid and search courses
            try
            {
                string orgUnitStr = Request.Form["CourseOrgUnit_mapping_0"];
                Guid orgUnitGuid = new Guid(orgUnitStr);

                // Get courses in org unit
                courses = CourseCollection.GetByOrgUnit(orgUnitGuid, true, false, false);
                
                // Filter if search term specified
                if (!String.IsNullOrEmpty(titleSearch))
                {
                    courses.ApplyFilter(course => course.Title.ToLower().Contains(titleSearch));
                }

                // Sort by course title
                courses.SortProperty = "Title";
                courses.SortDirection = CLL.S3.Framework.CSLA.SortDirection.Asc;
                courses.ApplySort();
            }
            catch (Exception ex)
            {
                // Log
                new FrameworkException("Error searching courses.", ex, ExceptionSeverity.Warning);
            }

            if (courses != null)
            {
                // Display count of courses found
                CourseCount.Text = String.Format("{0:n0} courses found:", courses.Count);

                // Add course items to list)
                foreach (Course course in courses)
                {
                    CourseList.Items.Add(new ListItem(course.Title, course.CourseId));
                }
            }
        }

        protected void ProvisionCourses(object sender, EventArgs args)
        {
            CourseIDs = new List<string>();

            // Add the IDs for selected courses to the list
            foreach (int selectedIndex in CourseList.GetSelectedIndices())
            {
                CourseIDs.Add(CourseList.Items[selectedIndex].Value);
            }

            if (CourseIDs.Count == 0)
            {
                PageError.Text = "Empty course list.";
            }

            CourseSelectPanel.Visible = false;
            ProvisioningResultsPanel.Visible = true;
        }

        protected CourseProvisioningInfo GetProvisioningInfo(string courseID)
        {
            if (String.IsNullOrEmpty(courseID))
            {
                return null;
            }

            Course currentCourse = Course.Get(courseID);
            if (currentCourse == null)
            {
                return null;
            }

            var instructors = new List<UserProvisioningInfo>();
            foreach (CourseRoster rosterEntry in CourseRosterCollection.GetByCourseRights(currentCourse.CourseId, CourseRights.AllCourseFaculty))
            {
                try
                {
                    instructors.Add(
                        new UserProvisioningInfo()
                            {
                                UserKey = Util.GetUserKey(rosterEntry.LoginName),
                                FirstName = rosterEntry.ParentPerson.Fname,
                                LastName = rosterEntry.ParentPerson.Lname,
                                Email = rosterEntry.ParentPerson.Email,
                                MailLectureNotifications = Util.GetNotify(),
                            });
                }
                catch (Exception ex)
                {
                    // Log & skip null / corrupted roster entries
                    string loginName = (rosterEntry != null) ? rosterEntry.LoginName : String.Empty;
                    new FrameworkException("Error getting instructor info for: " + loginName, ex, ExceptionSeverity.Warning);
                }
            }

            var students = new List<UserProvisioningInfo>();
            foreach (CourseRoster rosterEntry in CourseRosterCollection.GetByCourseRights(currentCourse.CourseId, CourseRights.Student))
            {
                try
                {
                    students.Add(new UserProvisioningInfo()
                    {
                        UserKey = Util.GetUserKey(rosterEntry.LoginName),
                    });
                }
                catch (Exception ex)
                {
                    // Log & skip null / corrupted roster entries
                    string loginName = (rosterEntry != null) ? rosterEntry.LoginName : String.Empty;
                    new FrameworkException("Error getting student info for: " + loginName, ex, ExceptionSeverity.Warning);
                }
            }

            return new CourseProvisioningInfo()
            {
                ExternalCourseID = Util.GetExternalCourseID(currentCourse.CourseId),
                LongName = currentCourse.Title,
                Instructors = instructors.ToArray(),
                Students = students.ToArray()
            };
        }

        protected string FormatInstructorList(UserProvisioningInfo[] instructorList)
        {
            var instructorText = new StringBuilder();
            foreach (UserProvisioningInfo instructor in instructorList)
            {
                instructorText.Append(
                    String.Format("{0} ({1} {2} &lt;{3}&gt;)<br />",
                                  instructor.UserKey,
                                  instructor.FirstName,
                                  instructor.LastName,
                                  instructor.Email));
            }

            return instructorText.ToString();
        }


        protected string FormatStudentList(UserProvisioningInfo[] studentList)
        {
            var studentUserKeyList = new List<string>();
            foreach (UserProvisioningInfo student in studentList)
            {
                studentUserKeyList.Add(student.UserKey);
            }

            return String.Join(", ", studentUserKeyList.ToArray());
        }

        protected Guid Provision(CourseProvisioningInfo courseInfo)
        {
            Guid provisionedID = Guid.Empty;

            try
            {
                using (var clientData = new ClientDataProxy())
                {
                    CourseInfo provisionedCourse = clientData.ProvisionCourse(courseInfo, Util.GetUserKey(), Util.GetAuthCode());
                    if (provisionedCourse != null)
                    {
                        provisionedID = provisionedCourse.PublicID;
                    }
                }

                if (provisionedID != Guid.Empty)
                {
                    // Strip instance name and ":" to get Angel course ID
                    string courseID = Util.GetCourseIDFromExternalCourseID(courseInfo.ExternalCourseID);

                    Util.SetPanoptoCourseID(courseID, provisionedID);
                }
                else
                {
                    // Log
                    new FrameworkException("Error provisioning course: " + courseInfo.LongName, ExceptionSeverity.Warning);
                }
            }
            catch (Exception ex)
            {
                // Log
                new FrameworkException("Error provisioning course: " + courseInfo.LongName, ex, ExceptionSeverity.Warning);
            }

            return provisionedID;
        }

    }

}