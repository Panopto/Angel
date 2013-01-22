using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CLL.S3.Angel;
using CLL.S3.Angel.Configuration;
using CLL.S3.Angel.Web;
using CLL.S3.Framework;
using CLL.S3.Framework.Exceptions;

namespace Panopto.External.Angel
{
    public class PanoptoConnectorPage : CLL.S3.Angel.Web.Page
    {
        // Pages inheriting from PanoptoConnectorPage can set these fields to true
        // in GetAuthenticationStatus override to require the specified permissions.
        protected bool RequireSystemAdminRights = false;
        protected bool RequireCourseEditorRights = false;

        /// <summary>
        /// Viewing a course in one window will clobber session context in other windows.
        /// This will ensure session context matches the course ID param we pass to course pages.
        /// </summary>
        protected override void OnPreInit(EventArgs e)
        {
            // Course according to Angel Session
            Course currentCourse = AngelSession.CurrentAngelSession.Section;

            // Course according to our param
            string courseID = Request.QueryString["id"];

            // Check session course against id param to ensure we don't cross streams with separate pages
            if (!String.IsNullOrEmpty(courseID)
                && ((currentCourse == null) || !courseID.Equals(currentCourse.CourseId)))
            {
                AngelSession.CurrentAngelSession.LoadSection(courseID);
                currentCourse = AngelSession.CurrentAngelSession.Section;

                // Failed to get current course
                if ((currentCourse == null) || (currentCourse.CourseId != courseID))
                {
                    // Log
                    new FrameworkException("Error syncing course with id param.", ExceptionSeverity.Warning);
                }
            }

            base.OnPreInit(e);
        }

        /// <summary>
        /// Override GetAuthenticationStatus for complex access check.
        /// </summary>
        protected override AuthenticationStatus GetAuthenticationStatus()
        {
            // Make sure we don't fail basic auth checks
            AuthenticationStatus status = base.GetAuthenticationStatus();
            if (status != AuthenticationStatus.Authenticated)
            {
                return status;
            }

            // Default to not authorized
            status = AuthenticationStatus.InsufficientRights;

            // Admin
            if (AngelSession.CurrentAngelSession.SystemRights >= SystemRights.SystemEditor)
            {
                // Admins have access to everything
                status = AuthenticationStatus.Authenticated;
            }
            // If non-admin, and admin rights are not required, check course editor rights requirement
            else if (!RequireSystemAdminRights)
            {
                // Course editor rights not required
                if (!RequireCourseEditorRights)
                {
                    status = AuthenticationStatus.Authenticated;
                }
                // Check course editor rights
                else
                {
                    // Check roster entry for course rights
                    CourseRoster currentCourseRosterEntry = AngelSession.CurrentAngelSession.RosterEntry;
                    if (currentCourseRosterEntry != null)
                    {
                        if (currentCourseRosterEntry.UserRights >= CourseRights.CourseEditor)
                        {
                            // Has course editor rights
                            status = AuthenticationStatus.Authenticated;
                        }
                    }
                    // Failed to get roster entry
                    else
                    {
                        // Log
                        new FrameworkException("Error getting roster entry for course.", ExceptionSeverity.Warning);
                    }
                }
            }

            return status;
        }
    }
}