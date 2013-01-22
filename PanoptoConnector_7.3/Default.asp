<!-- #include file="../../_portal.asp" -->

<!-- Server-side JS include -->
<script language="javascript" src="PanoptoData.js" runat="server"></script>

<%
Response.Expires = 0

Call InitNugget("PanoptoConnector")

gsNugTitle = "Panopto Content"

Dim strCourseID
strCourseID = Session("SECTION_ID")

'Refresh environment variables to get changes made within the current login / course session.
Session("VARIABLES") = GetVariables("" & Session("USER_ID"), "" & Session("SECTION_ID"))
Set dctVar = GetVarDictionary(False)

Dim instanceName
instanceName = EnvVar("PANOPTO_INSTANCE", "")

'Check whether the current course is associated with a Panopto course
Dim isConfigured
If EnvVar("PanoptoCourseID", "") = "" Then
    isConfigured = "false"
Else
    isConfigured = "true"
End If

' Constant of 8+ means Instructor+
Dim isInstructor
isInstructor = (UserRights(strCourseID) >= 8)

'Enable edit icon for instructors
If isInstructor Then
    gsNugEditLink = "/Portal/Nuggets/PanoptoConnector/CourseSettings.aspx?id=" & strCourseID
End If

Response.Write PanoptoDataObject.getNuggetMarkup()
%>

<script type="text/javascript">
    Panopto =
    {
        IsCourseConfigured: <%= isConfigured %>
    };
</script>

<!-- JQuery library hosted at Google -->
<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
<!-- MSAJAX library hosted at Microsoft -->
<script type="text/javascript" src="//ajax.microsoft.com/ajax/beta/0911/Start.js"></script>
<!-- JS code behind for Panopto nugget -->
<script type="text/javascript" src="/Portal/Nuggets/PanoptoConnector/PanoptoClient.js"></script>

<link rel="stylesheet" type="text/css" href="/Portal/Nuggets/PanoptoConnector/main.css">

<!-- Add POST parameter to links into Panopto to activate SSO auto-login -->
<form name="SSO" method="post">
    <input type="hidden" name="instance" value="<%= instanceName %>" />
</form>

<!-- MSAJAX Templates for nugget content. -->
<div id="panoptoTemplates" xmlns:sys="javascript:Sys">
    <div id="noCourseTemplate" class="sys-template">
        No course selected.
    </div>
    <div id="contentTemplate" class="sys-template" style="display:none">

<!--        <a href="/Portal/Nuggets/PanoptoConnector/CourseSettings.aspx">Configure...</a>-->

        <div class="sectionHeader">Live Sessions</div>
        <div id="liveSessionsLoading" class="liveLecture">Loading...</div>
        <div id="noLiveSessions" class="liveLecture" style="display:none">No live sessions.</div>
        <div id="liveSessions" class="sys-template">
            <div class="liveLecture">
	            {{Name}}
                <a sys:href="{{BroadcastViewerURL}}"
                   sys:if="BroadcastViewerURL"
                   onclick="return startSSO(this)">
                    watch live
                </a>
                <a sys:href="{{LiveNotesURL}}" sys:onclick="{{ 'launchNotes(\'' + LiveNotesURL + '\'); return false;' }}">
                    take notes
                </a>
            </div>
        </div>
        
        <div class="sectionHeader">Completed Recordings</div>
        <div id="completedRecordingsLoading" class="completedRecording">Loading...</div>
        <div id="noCompletedRecordings" class="completedRecording" style="display:none">No completed recordings.</div>
        <div id="completedRecordings" class="sys-template">
            <div class="completedRecording">
                <a sys:href="{{ViewerURL}}" onclick="return startSSO(this)">
	                {{DisplayName}}
                </a>
            </div>
        </div>
        
        <div class="sectionHeader">Podcast Feeds</div>
        <div id="podcastsLoading" class="courseLink">Loading...</div>
        <div id="noPodcasts" class="courseLink" style="display:none">No podcast feeds available.</div>
        <div id="podcastFeeds" class="sys-template">
            <div class="courseLink"
                 sys:if="AudioPodcastURL">
                <img src="/Portal/Nuggets/PanoptoConnector/images/feed_icon.gif" />
                <a sys:href="#" sys:onclick="{{'open(\'' + AudioPodcastURL + '\', \'panoptoPodcastWindow\'); return false;'}}">
                    Audio Podcast
                </a>
                <span class="rssLink">(<a
                    sys:href="{{AudioRssURL}}" target="_blank">RSS</a
                >)</span>
            </div>
            <div class="courseLink"
                 sys:if="VideoPodcastURL">
		        <img src="/Portal/Nuggets/PanoptoConnector/images/feed_icon.gif" />
		        <a sys:href="#" sys:onclick="{{'open(\'' + VideoPodcastURL + '\', \'panoptoPodcastWindow\'); return false;'}}">
                    Video Podcast
                </a>
		        <span class="rssLink">(<a
			        sys:href="{{VideoRssURL}}" target="_blank">RSS</a
		        >)</span>
            </div>
        </div>

<% if isInstructor then %>
        <div class="sectionHeader">Links</div>
        <div id="systemInfoLoading" class="courseLink">Loading...</div>
		<div id="systemLinks" class="sys-template">
		    <div class="courseLink">
			    <a id="courseSettingsLink" href="#" onclick="return startSSO(this)">Panopto Course Settings</a>
       	    </div>
            <div class="courseLink">
                Download Recorder
                (<a sys:href="{{RecorderDownloadUrl}}">Windows</a>
                | <a sys:href="{{MacRecorderDownloadUrl}}">Mac</a>)
            </div>
        </div>
<% end if %>

    </div>
</div>