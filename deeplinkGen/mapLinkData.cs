using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeplinkUtil
{
    public class mapLinkData
    {
        public string locationLong { get; set; }
        public string locLat { get; set; }
        public string locLon { get; set; }
        public string locZ { get; set; }
        public string linkRAW { get; set; }
        public string linkTitle { get; set; }
        public string APPLMAPscheme = "http://maps.apple.com/";
        public string APPLmapLink { get; set; }
        public string APPLscript { get; set; }
        public string ANDRDscript { get; set; }
        public string ANDRDMAPscheme = "geo:";
        public string ANDRDgoogMapLink { get; set; }
        public string UNVRSLmapScript { get; set; }
        public string linkClass { get; set; }
    }
}
