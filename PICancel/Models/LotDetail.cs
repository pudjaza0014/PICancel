using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PICancel.Models
{
    public class LotDetail
    {
        public string LotNo { get; set; }
        public string Type { get; set; }
        public string SLine { get; set; }
        public string ProductCode { get; set; }
        public string RohmProductCode { get; set; }
        public string TRNo { get; set; }
        public string Spec { get; set; }
        public string Pack { get; set; }
        public string hFERank { get; set; }
        public string ReceivingCateg { get; set; }
        public string ProductCateg { get; set; }
        public string OrderNo { get; set; }
        public DateTime InputDate { get; set; }
        public string OutputDate { get; set; }
        public int InputQty { get; set; }
        public int OutputQty { get; set; }
        public int PackUnitQty { get; set; }
        public string InputProcCode { get; set; }
        public string OutputProcCode { get; set; }
        public string RouteNo { get; set; }
        public string Priority { get; set; }
        public string ReInputLot { get; set; }
        public string UseOldLotNo { get; set; }
        public int LotCompQty { get; set; }
        public int LotCancelQty { get; set; }
        public DateTime LotCompDate { get; set; }
        public string WorkslipPrinted { get; set; }
        public string ProductLabelPrinted { get; set; }
        public DateTime UpdDate { get; set; }
        public string Operator { get; set; }
        public string OperatorName { get; set; }

    }



    public class LotCancelDetailTemp
    {
        public string CancelBy { get; set; }
        public string OrderNo { get; set; }
        public string LotNo { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public string CancelQty { get; set; }
        public string RBCthr { get; set; }
        public string CancelCode { get; set; }
        public string Treatment { get; set; }
        public string ResponseProcess { get; set; }
        public string ResponseMachine { get; set; }
        public string ResponseOperationDate { get; set; } 
    }
    public class LotCancelDetail
    {
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string ProductCateg { get; set; }
        public DateTime InputDate { get; set; }
        public DateTime OutputDate { get; set; }
        public int InputQTY { get; set; }
        public int OutputQTY { get; set; }
        public int CancelQTY { get; set; }
        public int CompleteQTY { get; set; }
        public string Packunit { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public int OriginalQty { get; set; }
        public int NeedCancelQty { get; set; }
        public int RemainQty { get; set; }
        public string CancelCode { get; set; }
        public string Treatment { get; set; }
        public string RespProcess { get; set; }
        public string RespMachine { get; set; }
        public string OpeDate { get; set; }

    }

    public class LotCancelTable
    {
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string ProductCateg { get; set; }
        public string InputDate { get; set; }
        public string OutputDate { get; set; }
        public int InputQTY { get; set; }
        public int OutputQTY { get; set; }
        public int CancelQTY { get; set; }
        public int CompleteQTY { get; set; }
        public string Packunit { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public int OriginalQty { get; set; }
        public int NeedCancelQty { get; set; }
        public int RemainQty { get; set; }
        public string CancelCode { get; set; }
        public string Treatment { get; set; }
        public string RespProcess { get; set; }
        public string RespMachine { get; set; }
        public string OpeDate { get; set; }

    }
    public class OrderCancleTable
    {
        public string CancelBy { get; set; }
        public string OrderNo { get; set; }
        public string LotNo { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public int CancelQty { get; set; }
        public string RBCtgr { get; set; }
        public string CancelCode { get; set; }
        public string treatmentCode { get; set; }
        public string RespProcess { get; set; }
        public string RespMachine { get; set; }
        public string RespOpeDate { get; set; }
    }


    public class UndoDetailTable
    {
        public string CancelBy { get; set; }
        public string LotNo { get; set; }
        public string OrderNo { get; set; }
        public string ProdFamily { get; set; }
        public string Type { get; set; }
        public string ProductCode { get; set; }
        public int CancelQty { get; set; }
        public string CancelCode { get; set; }
        public string CancelReason { get; set; }
        public string TreatmentCode { get; set; }
        public string TreatmentName { get; set; }
        public string RespProcess { get; set; }
        public string RespMachine { get; set; }
        public string RespOpeDate { get; set; }
        public string SentCateg { get; set; }
        public string UpdDate { get; set; }
        public string Operator { get; set; }
        public string OperatorName { get; set; }
        public string CancelSeq { get; set; }
        public string RBCTGR { get; set; } 
    }

    public class PicancelInquiryControl
    {
        public string Lotno { get; set; }
        public string Line { get; set; }
        public string type { get; set; }
        public string ProductCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CancelBy { get; set; }
        public string cancelCode { get; set; }

    }
    public class PICancelInquiryData
    {
        public string CancelDate { get; set; }
        public string CancelBy { get; set; }
        public string LotNo { get; set; }
        public string CancelQty { get; set; }
        public string OrderNo { get; set; }
        public string Type { get; set; }
        public string SLine { get; set; }
        public string ALine { get; set; }
        public string ProductCode { get; set; }
        public string CancelCode { get; set; }
        public string RBCTGR { get; set; }
        public string CancelReason { get; set; }
        public string Operator { get; set; }
        public string UpdDate { get; set; }
        public string OperatorName { get; set; }

    }
}