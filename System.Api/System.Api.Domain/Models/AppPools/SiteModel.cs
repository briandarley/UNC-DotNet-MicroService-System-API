using System.Collections.Generic;

namespace System.Api.Domain.Models.AppPools
{
    public class SiteModel
    {
        public string Name { get; set; }

        public List<ApplicationModel> Applications { get; set; }
        public string State { get; set; }
    }
}
