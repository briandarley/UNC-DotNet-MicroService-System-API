using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Api.Domain.Models.AppPools
{
    public class ApplicationModel
    {
        public string Path { get; set; }
        public AppPoolStateModel AppPool { get; set; }
    }
}
