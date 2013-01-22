function PanoptoData()
{
    // Break up token to avoid parsing errors.
    var m_nuggetToken = "<!" + "--NUGGET_TEXT-->";

    this.getNuggetMarkup = function()
    {
        var nuggetMarkup;

        // Get the nugget container text by calling the NuggetContainer().
        var nuggetContainer = NuggetContainer(gsNugID);

        // Check if the text contains the NUGGET_TEXT token.
        var nuggetTokenLoc = nuggetContainer.indexOf(m_nuggetToken);

        // If not, nugget is collapsed and contents should not be shown.
        if (nuggetTokenLoc == -1)
        {
            nuggetMarkup = nuggetContainer;
        }
        // If so, nugget is open and the token should be replaced with
        // nugget contents so output container text that precedes token.
        else
        {
            var nuggetContents = '<div id="loadingMessage">Loading...</div>' +
                                 '<div id="panoptoContent" class="sys-template">' +
                                    '<div id="contentPlaceholder"></div>' +
                                 '</div>';

            nuggetMarkup = nuggetContainer.replace(m_nuggetToken, nuggetContents);
        }

        return nuggetMarkup;
    }
}

PanoptoDataObject = new PanoptoData();