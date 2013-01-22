using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FormsAuthentication = System.Web.Security.FormsAuthentication;

using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;

namespace Panopto.External.Angel
{
    public class Util
    {
        // Constants for Angel global environment variable names
        private static string InstanceVarName = "PANOPTO_INSTANCE";
        private static string ServerVarName = "PANOPTO_SERVER";
        private static string AppKeyVarName = "PANOPTO_APP_KEY";
        private static string NotificationsVarName = "PANOPTO_NOTIFY";

        // Constant for Angel course environment variable name
        private static string PanoptoCourseIDVarName = "PanoptoCourseID";

        /// <summary>
        /// Get Angel instance name setting from Angel environment variable.
        /// </summary>
        public static String GetInstanceName()
        {
            return ConfigurationVariable.GetValue(InstanceVarName);
        }

        /// <summary>
        /// Set Angel instance name environment variable.
        /// </summary>
        public static void SetInstanceName(string instanceName)
        {
            ConfigurationVariable.SetValue(InstanceVarName, null, null, null, null, null, VariableType.DefaultSetting, instanceName);
        }

        /// <summary>
        /// Get Panopto server name setting from Angel environment variable.
        /// </summary>
        public static String GetServerName()
        {
            return ConfigurationVariable.GetValue(ServerVarName);
        }

        /// <summary>
        /// Set Panopto server name environment variable.
        /// </summary>
        public static void SetServerName(string serverName)
        {
            ConfigurationVariable.SetValue(ServerVarName, null, null, null, null, null, VariableType.DefaultSetting, serverName);
        }

        /// <summary>
        /// Get Panopto application key setting from Angel environment variable.
        /// </summary>
        public static String GetAppKey()
        {
            return ConfigurationVariable.GetValue(AppKeyVarName);
        }

        /// <summary>
        /// Set Panopto application key environment variable.
        /// </summary>
        public static void SetAppKey(string appKey)
        {
            ConfigurationVariable.SetValue(AppKeyVarName, null, null, null, null, null, VariableType.DefaultSetting, appKey);
        }

        /// <summary>
        /// Get Panopto "notify" setting from Angel environment variable.
        /// </summary>
        public static bool GetNotify()
        {
            // Default to "true" if no setting.
            return ConfigurationVariable.GetValue(NotificationsVarName) != "false";
        }

        /// <summary>
        /// Set Panopto "notify" setting environment variable.
        /// </summary>
        public static void SetNotify(bool notify)
        {
            // Convert bool to string.
            string settingText = notify ? "true" : "false";

            ConfigurationVariable.SetValue(NotificationsVarName, null, null, null, null, null, VariableType.DefaultSetting, settingText);
        }

        /// <summary>
        /// Get Panopto user key of currently logged-in user.
        /// </summary>
        public static String GetUserKey()
        {
            return GetUserKey(AngelSession.CurrentAngelSession.User.LoginName);
        }

        /// <summary>
        /// Get Panopto user key for specified username.
        /// </summary>
        public static String GetUserKey(string userName)
        {
            string userKey = String.Format("{0}\\{1}", GetInstanceName(), userName);

            return userKey;
        }

        /// <summary>
        /// Get API auth code for currently logged-in user.
        /// </summary>
        public static String GetAuthCode()
        {
            string userKey = GetUserKey();
            string serverName = GetServerName();

            string payload = String.Format("{0}@{1}", userKey, serverName);

            return GetAuthCode(payload);
        }

        /// <summary>
        /// Get auth code for specified payload for use in SSO process.
        /// </summary>
        /// <param name="payload">Payload to sign</param>
        /// <returns>Specified payload signed with app key and hashed with SHA-1.</returns>
        public static String GetAuthCode(string payload)
        {
            // Get the app key setting.
            string applicationKey = ConfigurationVariable.GetValue(AppKeyVarName);

            String signedPayload = payload + "|" + applicationKey;

            String authCode = FormsAuthentication.HashPasswordForStoringInConfigFile(signedPayload, "SHA1").ToUpper();

            return authCode;
        }

        /// <summary>
        /// Set the ID of the Panopto course to pull content from for the specified Angel course.
        /// </summary>
        /// <param name="angelCourseID">ID of the Angel course to set the setting on.</param>
        /// <param name="panoptoCourseID">ID of the Panopto course to pull content from.</param>
        public static void SetPanoptoCourseID(string angelCourseID, Guid panoptoCourseID)
        {
            ConfigurationVariable.SetValue(PanoptoCourseIDVarName, null, null, null, angelCourseID, null, VariableType.CourseSetting, panoptoCourseID.ToString());
        }

        /// <summary>
        /// Decorate an Angel course ID with instance name for use within Panopto.
        /// </summary>
        public static string GetExternalCourseID(string courseId)
        {
            return String.Format("{0}:{1}", GetInstanceName(), courseId);
        }

        /// <summary>
        /// Get the Angel course ID from a Panopto external course ID.
        /// </summary>
        public static string GetCourseIDFromExternalCourseID(string externalCourseId)
        {
            // Strip instance name and ":" to get Angel course ID
            return externalCourseId.Substring(externalCourseId.IndexOf(':') + 1);
        }
    }
}