<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Panopto.External.Angel.Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Panopto Connector: Global Config</title>
    <link rel="stylesheet" type="text/css" href="main.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="configPage">
            <asp:Panel DefaultButton="SubmitButton" CssClass="configContent" runat="server">
                <h2>Panopto Connector Config Page</h2>

                <table>
                    <tr>
                        <td class="globalConfigSettingName pluginVersionCell">
                            Plugin version:
                        </td>
                        <td class="pluginVersionCell">
                            4.3.5552
                        </td>
                    </tr>
                    <tr>
                        <td class="globalConfigSettingName">
                            <asp:Label AssociatedControlID="InstanceNameTextBox" runat="server">
                                Instance name:
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="InstanceNameTextBox" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class='globalConfigSettingName'>
                            <asp:Label AssociatedControlID="ServerTextBox" runat="server">
                                Panopto Server:
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ServerTextBox" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class='globalConfigSettingName'>
                            <asp:Label AssociatedControlID="AppKeyTextBox" runat="server">
                                Application Key:
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="AppKeyTextBox" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class='globalConfigSettingName'>
                            <asp:CheckBox ID="NotificationsCheckBox" runat="server" />
                        </td>
                        <td>
                            <asp:Label AssociatedControlID="NotificationsCheckBox" runat="server">
                                Email users when recorded sessions are ready to view.
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div class="submitButtonDiv">
                                <asp:Button ID="SubmitButton" Text="Submit" OnClick="SaveSettings" runat="server" />
                                <asp:Label ID="SaveConfirmationMessage" Text="Settings saved." CssClass="settingsConfirmationMessage" Visible="false" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>

                <div class="batchProvisionLink">
                    <a href="ProvisionCourse.aspx">Add Angel courses to Panopto Focus</a>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
