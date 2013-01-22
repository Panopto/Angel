using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;
using CLL.S3.Framework.Exceptions;

namespace Panopto.External.Angel
{

    public partial class AspNetShim : CLL.S3.Angel.Web.Page
    {
        private static Guid CoursePublicID
        {
            get
            {
                Guid CoursePublicID = Guid.Empty;

                try
                {
                    Course currentCourse = AngelSession.CurrentAngelSession.Section;

                    string panoptoCourseID = ConfigurationVariable.GetValue("PanoptoCourseID", null, null, null, currentCourse.CourseId, null);

                    CoursePublicID = new Guid(panoptoCourseID);
                }
                catch (Exception ex)
                {
                    // Log
                    new FrameworkException("Error getting Panopto ID for course.", ex, ExceptionSeverity.Warning);
                }

                return CoursePublicID;
            }
        }

        [WebMethod]
        public static CourseInfo GetCourse()
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.GetCourse(CoursePublicID, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        public static CourseInfo[] GetCourses()
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.GetCourses(Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        public static CourseInfo ProvisionCourse(CourseProvisioningInfo ProvisioningInfo, string ApiUserKey, string AuthCode)
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.ProvisionCourse(ProvisioningInfo, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        CourseInfo[] ProvisionCourses(CourseProvisioningInfo[] ProvisioningInfo, string ApiUserKey, string AuthCode)
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.ProvisionCourses(ProvisioningInfo, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        Guid CreateCourse(string ShortName, string LongName, string ExternalCourseID, string ApiUserKey, string AuthCode)
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.CreateCourse(ShortName, LongName, ExternalCourseID, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        void AddUsersToCourse(Guid CoursePublicID, AccessLevel Role, string[] UserKeys, string ApiUserKey, string AuthCode)
        {
            using (var clientData = new ClientDataProxy())
            {
                clientData.AddUsersToCourse(CoursePublicID, Role, UserKeys, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        void SetContactInfo(string ContactUserKey, string FirstName, string LastName, string Email, bool MailLectureNotifications, string ApiUserKey, string AuthCode)
        {
            using (var clientData = new ClientDataProxy())
            {
                clientData.SetContactInfo(ContactUserKey, FirstName, LastName, Email, MailLectureNotifications, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        public static SessionInfo[] GetLiveSessions()
        {
            using (var clientData = new ClientDataProxy())
            {
                SessionInfo[] info = clientData.GetLiveSessions(CoursePublicID, Util.GetUserKey(), Util.GetAuthCode());

                return info;
            }
        }

        [WebMethod]
        public static DeliveryInfo[] GetCompletedDeliveries()
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.GetCompletedDeliveries(CoursePublicID, Util.GetUserKey(), Util.GetAuthCode());
            }
        }

        [WebMethod]
        public static SystemInfo GetSystemInfo()
        {
            using (var clientData = new ClientDataProxy())
            {
                return clientData.GetSystemInfo();
            }
        }
    }

}