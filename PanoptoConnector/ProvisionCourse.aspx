<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvisionCourse.aspx.cs" Inherits="Panopto.External.Angel.ProvisionCourse" %>
<%@ Register Src="~/UserControls/EnterpriseHierarchyDirectorySelector.ascx" TagPrefix="Angel" TagName="OrgUnit" %>
<%@ Import Namespace="Panopto.External.Angel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panopto Connector: Add Course(s) to Panopto</title>
    <link rel="stylesheet" type="text/css" href="main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <div class="configContent">
            <asp:Panel ID="CourseSelectPanel" runat="server">
                <h3>Batch Add Courses to Panopto:</h3>

                <asp:Panel DefaultButton="CourseSearchButton" runat="server">
                
                    <br />
                    <b>Organizational Unit</b>
                    <Angel:OrgUnit ID="CourseOrgUnit" runat="server" />
                
                    <p>
                        <b>Title</b> (optional)<br />
                        <asp:TextBox ID="CourseFilter" runat="server" />
                        <asp:Button ID="CourseSearchButton" Text="Search" OnClick="SearchCourses" runat="server" />
                    </p>
                </asp:Panel>
                
                <asp:Label ID="CourseCount" runat="server" />
                <asp:Panel DefaultButton="BatchProvisionButton" runat="server">
                    <asp:ListBox ID="CourseList" SelectionMode="Multiple" CssClass="courseList" runat="server" />
                    <br />

                    <asp:Button ID="BatchProvisionButton" Text="Add Courses to Panopto" OnClick="ProvisionCourses" runat="server" />
                </asp:Panel>
            </asp:Panel>

            <asp:Panel ID="ProvisioningResultsPanel" Visible="false" runat="server">
                <h3>
                    Provisioning Results
                </h3>

                <asp:Label ID="PageError" CssClass="error" runat="server" />

<%  // This code will not execute until ProvisioningResultsPanel.Visible is true.
    // Iterate within the page so we can send the response down progressively (per provision call),
    // instead of waiting for the entire control tree to render.
    foreach (string courseID in CourseIDs)
    {
        CourseProvisioningInfo courseInfo = GetProvisioningInfo(courseID);
        if (courseInfo != null)
        {
%>
                <div class="courseProvisionResult">
                    <div class="attribute">Course Name</div>
                    <div class="value">
                        <%= courseInfo.LongName %>
                    </div>

                    <div class="attribute">Instructors</div>
                    <div class="value">
                        <%= FormatInstructorList(courseInfo.Instructors) %>
                    </div>

                    <div class="attribute">Students</div>
                    <div class="value">
                        <%= FormatStudentList(courseInfo.Students) %>
<%
            // Flush progress so far in case provisioning SOAP call takes a while.
            Response.Flush();
%>
                    </div>

                    <div class="attribute">Result</div>
                    <div class="value">
<%
            Guid provisionedID = Provision(courseInfo);
            if (provisionedID != Guid.Empty)
            {
%>
                        <span class="successMessage">
                            <%= String.Format("Successfully provisioned course {{{0}}}", provisionedID) %>
                        </span>
<%
            }
            else
            {
%>
                        <span class="errorMessage">Error provisioning course.</span>
<%
            }
%>
                    </div>
                </div>
<%
        }
        else
        {
%>
                <div class="courseProvisionResult">
                    <span class="errorMessage">Error retrieving course info.</span>
                </div>
<%
            // Skip course.
            continue;
        }
    }
%>
            </asp:Panel>

            <div class="backLink">
                <asp:HyperLink ID="BackLink" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>
