﻿using System.Collections.Generic;
using Escc.Umbraco.LinksManager.Models.InboundLinkChecker;

namespace Escc.Umbraco.LinksManager.Services.Interfaces
{
    interface ICsvFileService
    {
        IList<InspyderLinkModel> GetLinksByDestination(string destinationUrl);
    }
}
