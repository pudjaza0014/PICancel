using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PICancel.Models
{
    public class RohmOrderModel
    {
        public string strResult { get; set; }
        public string strDocNo { get; set; }
        public Boolean blResult { get; set; }
        public  List<RohmOrderNOChip> ListOrder { get; set; }
    }

    public class RohmOrderControl
    {
        public string Type { get; set; }
        public string OrderNo { get; set; }
    }

    public class RohmOrderNOChip
    {
        public string No { get; set; }
        public string OrderNo { get; set; }
        public string ProductCode { get; set; }
        public string TRNo { get; set; }
        public string Spec { get; set; }
        public string Pack { get; set; }
        public string hFERank { get; set; }
        public string RohmLine { get; set; }
        public string Type { get; set; }
        public string InputDate { get; set; }
        public string OrderQtyOrg { get; set; }
        public string OrderQty { get; set; }
        public string Message { get; set; }
    }
    public class RohmOrderError
    {
        public string No { get; set; }
        public string OrderNo { get; set; }
        public string ProductCode { get; set; }
        public string TRNo { get; set; }
        public string Spec { get; set; }
        public string Pack { get; set; }
        public string hFERank { get; set; }
        public string RohmLine { get; set; }
        public string Type { get; set; }
        public string OrderQty { get; set; }
        public string InputDate { get; set; }
        public string Message { get; set; }

    }
    public class RohmOrderQty
    {
        public string OrderNo { get; set; }
        public int OrderQty { get; set; }
        public int OrderQtyOrg { get; set; }
    }


}