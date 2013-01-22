var gPanoptoDataProvider = "/Portal/Nuggets/PanoptoConnector/AspNetShim.aspx";

// Function to pop up Panopto live note taker.
function launchNotes(url)
{
    // Open empty notes window, then POST SSO form to it.
    var notesWindow = window.open("", "PanoptoNotes", "width=500,height=800,resizable=1,scrollbars=0,status=0,location=0");
    document.SSO.action = url;
    document.SSO.target = "PanoptoNotes";
    document.SSO.submit();

    // Ensure the new window is brought to the front of the z-order.
    notesWindow.focus();
}

function startSSO(linkElem)
{
    document.SSO.action = linkElem.href;
    document.SSO.target = "_blank";
    document.SSO.submit();

    // Cancel default link navigation.
    return false;
}

// Make async request to populate panoptoInfo DIV with info from Panopto web service.
Sys.require([Sys.components.dataView, Sys.scripts.WebServices, Sys.components.dataContext, Sys.scripts.Templates], function ()
{
    if (!Panopto.IsCourseConfigured)
    {
        Sys.create.dataView("#panoptoContent",
            {
                data: {},
                itemPlaceholder: "#contentPlaceholder",
                itemTemplate: "#noCourseTemplate",
                rendered: function ()
                {
                    $("#panoptoContent").show();
                    $("#loadingMessage").hide();
                }
            });
    }
    else
    {
        Sys.create.dataView("#panoptoContent",
            {
                data: {},
                itemPlaceholder: "#contentPlaceholder",
                itemTemplate: "#contentTemplate",
                rendered: function ()
                {
                    $("#panoptoContent").show();
                    $("#loadingMessage").hide();
                }
            });

        Sys.create.dataView("#liveSessions0",
            {
                autoFetch: "true",
                dataProvider: gPanoptoDataProvider,
                fetchOperation: "GetLiveSessions",
                rendered: function (sender, args)
                {
                    $("#liveSessionsLoading0").hide();

                    if (!args.get_data().length)
                    {
                        $("#noLiveSessions0").show();
                    }
                }
            });

        Sys.create.dataView("#completedRecordings0",
            {
                autoFetch: "true",
                dataProvider: gPanoptoDataProvider,
                fetchOperation: "GetCompletedDeliveries",
                rendered: function (sender, args)
                {
                    $("#completedRecordingsLoading0").hide();

                    if (!args.get_data().length)
                    {
                        $("#noCompletedRecordings0").show();
                    }
                }
            });


        var courseSettingsLink;

        Sys.create.dataView("#podcastFeeds0",
            {
                autoFetch: "true",
                dataProvider: gPanoptoDataProvider,
                fetchOperation: "GetCourse",
                itemRendering: function (sender, args)
                {
                    $("#podcastsLoading0").hide();

                    var dataItem = args.get_dataItem();
                
                    if (!dataItem.AudioPodcastURL)
                    {
                        $("#noPodcasts0").show();
                    }

                    courseSettingsLink = args.get_dataItem().CourseSettingsURL;
                }
            });

        if ($get("systemLinks0"))
        {
            Sys.create.dataView("#systemLinks0",
                {
                    autoFetch: "true",
                    dataProvider: gPanoptoDataProvider,
                    fetchOperation: "GetSystemInfo",
                    rendered: function (sender, args)
                    {
                        $("#systemInfoLoading0").hide();

                        $("#courseSettingsLink0").attr("href", courseSettingsLink);
                    }
                });
        }
    }
});
