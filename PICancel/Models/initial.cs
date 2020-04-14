using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PICancel.Models
{

    public class PICancelMenu
    {
        public string SystemID { get; set; }
        public string SystemName { get; set; }
        public string ManuID { get; set; }
        public string ManuName { get; set; }
        public string ManuCateg { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Param { get; set; }
        public string PC { get; set; }
        public string QC { get; set; }
        public string OGI { get; set; }

    }

    public class ListPICancelMenu
    {
        public List<PICancelMenu> dtResult { get; set; }
    }

    public class initial
    {
        public string strResult { get; set; }
        public string strErrMsg { get; set; }
        public List<ListPICancelMenu> dtResult { get; set; }
    }
}