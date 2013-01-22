<!-- #include file="../../_portal.asp" -->

<%
'Make sure the page is not cached by IIS
Response.Expires = 0

'Initialize the page variables
Call InitPortalPage()

Dim ReturnUrl
ReturnUrl = Request.QueryString("ReturnUrl")

Dim ReturnUrlParam
ReturnUrlParam = Server.URLEncode(ReturnUrl)
'Double-encode to work around failure to reencode in /signon/RedirectAndRefresh.asp
ReturnUrlParam = Server.URLEncode(ReturnUrlParam)
%>

<html>
    <head>
        <link rel="stylesheet" type="text/css" href="<%=gstrAppStylesheet%>" />
        <title>Panopto Connector Single Sign-On Page</title>
    </head>

    <!--call the pageScroll function to scroll if necessary -->
    <body class="bodyDefault">
        <div class="normalDiv">
            <span class="normalSpan">
                <h2 class="pageTitleSpan">
                    Angel Single Sign-On Page for Panopto
                </h2>

                <!--------------- Inlined LogonForm Nugget ------------------->
                <div menutype="nugget" class="nugget nugstate_F" id="nugLogonForm" style="position: relative; width: 500px">
                    <span class="nuggetIcon">
                        <img border="0" alt="" src="/images/misc/whitespace.gif"></span>
                    <h2 class="nugTitle">
                        <div class="topborder" style="display: none"></div>
                        <span class="left"><span class="right"><span class="center"><a id="lnkLogonForm"
                            target="_self">Log On</a> </span></span></span>
                        <div class="controls">
                            <img hspace="1" border="0" align="middle" vspace="0" class="nuggetRemoveIcon" title="Remove: Log On"
                                alt="Remove: Log On" src="/AngelThemes/icons/portal/remove.png"></div>
                    </h2>
                    <div class="nugBody">
                        <div class="nugBodyLogo">
                            <form name="frmLogon" id="frmLogon" target="_top" action="/signon/authenticate.asp?REDIR=<%= ReturnUrlParam %>" method="POST" style="display: inline;">
                                <input type="hidden" value="/" name="COOKIEPATH">
                                <div>
                                    <table cellspacing="0" cellpadding="1" width="100%" summary="">
                                        <tbody>
                                            <tr>
                                                <td align="right" class="normalDiv">
                                                    <span style="font-size: 8pt;" class="descriptionSpan"><strong>
                                                        <label for="username">
                                                            Username</label></strong></span>
                                                </td>
                                                <td align="left">
                                                    <input type="text" value="" id="username" name="username" size="8" style="width: 70px;
                                                        font-size: 8pt;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="normalDiv">
                                                    <span style="font-size: 8pt;" class="descriptionSpan"><strong>
                                                        <label for="password">
                                                            Password</label></strong></span>
                                                </td>
                                                <td align="left">
                                                    <input type="password" value="" id="password" name="password" size="8" style="width: 70px;
                                                        font-size: 8pt;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <input type="submit" value=" Log On " class="button">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div align="center">
                                    <small><a href="/signon/RequestPasswordReset.aspx">I forgot my password</a>&nbsp;</small></div>
                            </form>
                            <!--NUGGET_TEXT-->
                            <div class="clear">
                                &nbsp;</div>
                        </div>
                    </div>
                </div>
                <!--------------- End Inlined LogonForm Nugget ------------------->

            </span>
        </div>
    </body>
</html>