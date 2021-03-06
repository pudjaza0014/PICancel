﻿using System;
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
        public string strRunNo1 { get; set; }
        public string strRunNo2 { get; set; }
        public string CategType { get; set; }
    }

    public class RohmInquiryControl
    {
        public string CategType { get; set; }
        public string DocNo { get; set; }
        public string Type { get; set; }
        public string OrderNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class RohmOrderInquiry
    {
        public string No { get; set; }
        public string CategType { get; set; }
        public string DocNo { get; set; }
        public string OrderNo { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public string RohmLine { get; set; }
        public string InputDate { get; set; }
        public string Message { get; set; }
        //public string View { get; set; } 
    }

    public class RohmInqGrp
    {
        public Boolean blStatus { get; set; }
        public string strresult { get; set; }
        public List<RohmOrderInquiry> DataList { get; set; }
    }



    public class ProductPlan
    {
        public string Type { get; set; }
        public string TypeCode { get; set; }
        public int DeliveryPlanQty { get; set; }
        public string InputDate { get; set; }
        public string UserName { get; set; }
    }


    public class OrderPlanSummary
    {
        public string InputDate { get; set; }
        public string TypeGroup { get; set; }
        public string Seq { get; set; }
        public string TypeCode { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DeliveryPlanQty { get; set; }
        public string OrderPlanQty { get; set; }
        public string DifQty { get; set; }
    }

    public class OrderPlanControl
    {
        public string InputDateForm { get; set; }
        public string InputDateTo { get; set; }
        public string TypeCode { get; set; } 
    }
    public class OrderPlanInqGrp
    {
        public Boolean blStatus { get; set; }
        public string strresult { get; set; }
        public List<OrderPlanSummary> DataList { get; set; }
    }

    public class OrderPlanDetail
    {
        public string OrderNo { get; set; }
        public string TypeCode { get; set; }
        public string InputDate { get; set; }
        public string OrderQty { get; set; }
        public string RohmProductCode { get; set; }
        public string ProductCode { get; set; }
        public string TRNo { get; set; }
        public string Type { get; set; }
        public string SLine { get; set; } 
    }
    public class OrderPlanDetailGrp
    {
        public Boolean blStatus { get; set; }
        public string strresult { get; set; }
        public List<OrderPlanSummary> DataList { get; set; }
    }
}