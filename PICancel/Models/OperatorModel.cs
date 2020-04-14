using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PICancel.Models
{
    public class OperatorModel
    {
        public string OPID { get; set; }
        public string Password { get; set; }
        public string OPName { get; set; }
        public string OPLevel { get; set; }
        public string OPType { get; set; }
        public string AuthorityLV { get; set; } 
    }

    public class OPeratorLogin
    {
        public Boolean strResult { get; set; }
        public OperatorModel OPeratordetail { get; set; }
    }
}