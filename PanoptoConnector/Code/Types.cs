using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.ServiceModel;

namespace Panopto.External.Angel
{

    [DataContract(Namespace = "http://services.panopto.com")]
    public class SystemInfo
    {
        [DataMember]
        public String RecorderDownloadUrl;
        [DataMember]
        public String MacRecorderDownloadUrl;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class CourseProvisioningInfo
    {
        [DataMember]
        public string ShortName;
        [DataMember]
        public string LongName;
        [DataMember]
        public string ExternalCourseID;
        [DataMember]
        public UserProvisioningInfo[] Instructors;
        [DataMember]
        public UserProvisioningInfo[] Students;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class UserProvisioningInfo
    {
        [DataMember]
        public string UserKey;
        [DataMember]
        public string FirstName;
        [DataMember]
        public string LastName;
        [DataMember]
        public string Email;
        [DataMember]
        public bool MailLectureNotifications;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class CourseInfo
    {
        [DataMember]
        public Guid PublicID;
        [DataMember]
        public string ExternalCourseID;
        [DataMember]
        public string DisplayName;
        [DataMember]
        public AccessLevel Access;
        [DataMember]
        public string AudioPodcastURL;
        [DataMember]
        public string AudioRssURL;
        [DataMember]
        public string VideoPodcastURL;
        [DataMember]
        public string VideoRssURL;
        [DataMember]
        public string CourseSettingsURL;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public enum AccessLevel
    {
        [EnumMember]
        Error = 0,
        [EnumMember]
        Creator = 1,
        [EnumMember]
        Viewer = 2,
        [EnumMember]
        Public = 3
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class SessionInfo
    {
        [DataMember]
        public Guid PublicID;
        [DataMember]
        public string Name;
        [DataMember]
        public string LiveNotesURL;
        [DataMember]
        public string BroadcastViewerURL;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class DeliveryInfo
    {
        [DataMember]
        public Guid PublicID;
        [DataMember]
        public string DisplayName;
        [DataMember]
        public string ViewerURL;
    }

    [DataContract(Namespace = "http://services.panopto.com")]
    public class UserInfo
    {
        [DataMember]
        public string UserKey;
        [DataMember]
        public string FirstName;
        [DataMember]
        public string LastName;
        [DataMember]
        public string Email;
        [DataMember]
        public bool MailLectureNotifications;
    }

    [ServiceContract(Namespace = "http://services.panopto.com")]
    public interface IClientDataService
    {
        [OperationContract]
        SystemInfo GetSystemInfo();
        [OperationContract]
        CourseInfo ProvisionCourse(CourseProvisioningInfo ProvisioningInfo, string ApiUserKey, string AuthCode);
        [OperationContract]
        CourseInfo[] ProvisionCourses(CourseProvisioningInfo[] ProvisioningInfo, string ApiUserKey, string AuthCode);
        [OperationContract]
        Guid CreateCourse(string ShortName, string LongName, string ExternalCourseID, string ApiUserKey, string AuthCode);
        [OperationContract]
        void AddUsersToCourse(Guid CoursePublicID, AccessLevel Role, string[] UserKeys, string ApiUserKey, string AuthCode);
        [OperationContract]
        void SetContactInfo(string ContactUserKey, string FirstName, string LastName, string Email, bool MailLectureNotifications, string ApiUserKey, string AuthCode);
        [OperationContract]
        CourseInfo[] GetCourses(string ApiUserKey, string AuthCode);
        [OperationContract]
        CourseInfo GetCourse(Guid CoursePublicID, string ApiUserKey, string AuthCode);
        [OperationContract]
        SessionInfo[] GetLiveSessions(Guid CoursePublicID, string ApiUserKey, string AuthCode);
        [OperationContract]
        DeliveryInfo[] GetCompletedDeliveries(Guid CoursePublicID, string ApiUserKey, string AuthCode);
    }

}
