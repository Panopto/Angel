using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using CLL.S3.Angel.Configuration;

namespace Panopto.External.Angel
{

    public class ClientDataProxy : IDisposable
    {
        // Use with String.Format to build service location from serverName.
        // Note: app root is always "/Panopto".
        private const string serviceLocationFormatString = "http://{0}/Panopto/Services/ClientData.svc";

        // The ClientData service proxy.
        private ServiceProxy<IClientDataService> m_service;

        public ClientDataProxy()
        {
            string serverName = ConfigurationVariable.GetValue("PANOPTO_SERVER");

            string serviceLocation = String.Format(serviceLocationFormatString, serverName);

            m_service = new ServiceProxy<IClientDataService>(serviceLocation);
        }

        public SystemInfo GetSystemInfo()
        {
            return m_service.Channel.GetSystemInfo();
        }

        public CourseInfo ProvisionCourse(CourseProvisioningInfo ProvisioningInfo, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.ProvisionCourse(ProvisioningInfo, ApiUserKey, AuthCode);
        }

        public CourseInfo[] ProvisionCourses(CourseProvisioningInfo[] ProvisioningInfo, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.ProvisionCourses(ProvisioningInfo, ApiUserKey, AuthCode);
        }

        public System.Guid CreateCourse(string ShortName, string LongName, string ExternalCourseID, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.CreateCourse(ShortName, LongName, ExternalCourseID, ApiUserKey, AuthCode);
        }

        public void AddUsersToCourse(System.Guid CoursePublicID, AccessLevel Role, string[] UserKeys, string ApiUserKey, string AuthCode)
        {
            m_service.Channel.AddUsersToCourse(CoursePublicID, Role, UserKeys, ApiUserKey, AuthCode);
        }

        public void SetContactInfo(string ContactUserKey, string FirstName, string LastName, string Email, bool MailLectureNotifications, string ApiUserKey, string AuthCode)
        {
            m_service.Channel.SetContactInfo(ContactUserKey, FirstName, LastName, Email, MailLectureNotifications, ApiUserKey, AuthCode);
        }

        public CourseInfo[] GetCourses(string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.GetCourses(ApiUserKey, AuthCode);
        }

        public CourseInfo GetCourse(System.Guid CoursePublicID, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.GetCourse(CoursePublicID, ApiUserKey, AuthCode);
        }

        public SessionInfo[] GetLiveSessions(System.Guid CoursePublicID, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.GetLiveSessions(CoursePublicID, ApiUserKey, AuthCode);
        }

        public DeliveryInfo[] GetCompletedDeliveries(System.Guid CoursePublicID, string ApiUserKey, string AuthCode)
        {
            return m_service.Channel.GetCompletedDeliveries(CoursePublicID, ApiUserKey, AuthCode);
        }

        // Create a basic proxy for a specific endpoint address, and expose ClientBase's channel property.
        private class ServiceProxy<T> : ClientBase<T> where T : class
        {
            // Allow for non-trivial data sizes from Panopto server (e.g. many courses in GetCourses())
            static private BasicHttpBinding GetBinding()
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.MaxReceivedMessageSize = 33554432;

                // The clientdata service is implemented in an innefficient way, causing calls to GetSessions
                // to take well over a minute at Creighton.  Update the timeout on this manual binding
                // to give us a little breathing room until we can fix the service implementation.
                binding.OpenTimeout = TimeSpan.FromMinutes(5);
                binding.CloseTimeout = TimeSpan.FromMinutes(5);
                binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
                binding.SendTimeout = TimeSpan.FromMinutes(5);

                return binding;
            }

            public ServiceProxy(string sCurrentSite)
                : base (GetBinding(), new EndpointAddress(sCurrentSite)) { }

            // new keyword allows us to supercede the inherited protected member and make it public.
            public new T Channel
            {
                get { return base.Channel; }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_service.Close();
        }

        #endregion
    }

}