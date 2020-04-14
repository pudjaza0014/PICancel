using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PICancel.Models
{
    


    public class OGICancelInquiryModel
    {
        public string No { get; set; }
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string ProdFamily { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public string RBCTGR { get; set; }
        public string CancelQty { get; set; }
        public string CancelCode { get; set; }
        public string CancelReason { get; set; }
        public string cancelDate { get; set; }
        public string OperatorName { get; set; }
    }

    public class OGIInquiryCondition
    {
        public string CancelReason { get; set; }
        public string LotNo { get; set; }
        public string Type { get; set; }
        public string OrderNo { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class OGIConfirmCondition
    {
        public string CancelReason { get; set; }
        public string LotNo { get; set; }
        public string Operator { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class OGIInquiryResult
    {
        public Boolean Result { get; set; }
        public string strResult { get; set; }
        public List<OGICancelInquiryModel> DataList { get; set; }
    }

    public class OGICancelResult
    {
        public Boolean Result { get; set; }
        public string strResult { get; set; }
        public DataSet DataList { get; set; }
    }



}