<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseSettings.aspx.cs" Inherits="Panopto.External.Angel.CourseSettings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Panopto Connector: Course Settings</title>
    <link rel="stylesheet" type="text/css" href="main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 15px;">
            <h3>Panopto Connector: Course Settings</h3>
            <div style="margin-top: 5px;">Select an existing Panopto course to connect to:</div>
            <div>
                <asp:DropDownList ID="CoursesDropDown" runat="server" />
            </div>

            <asp:Panel ID="ProvisionPanel" runat="server">
                <br />
                OR
                <br />
                <br />
            
                <asp:HyperLink ID="ProvisionLink" runat="server" />
            </asp:Panel>

            <div style="margin-top: 15px;">
                <asp:Button ID="SaveButton" Text="Save" OnClick="Save" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
