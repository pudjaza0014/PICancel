function getformatDefault() {
//    var strEvent = sessionStorage.getItem("ssEvent");
//    if (strEvent != "LotCancel") { 
//        $("#txtNeedCancelQty").attr("disabled", "disabled");
//    }
//    if (strEvent == "OrderCancel") {
//        $("#divLotno").css("display", "none"); 
//        $("#divInputQty").css("display", "none");
//        $("#txtOrderNo").removeAttr("disabled", "disabled");
//        //$("#txtNeedCancelQty").attr("disabled", "disabled"); 
//    } if (strEvent == "LotCancelReflow") {


//    }

    var strEvent = sessionStorage.getItem("ssEvent");
    if (strEvent != "LotCancel") {
        $("#txtNeedCancelQty").attr("disabled", "disabled");

    }
    if (strEvent == "OrderCancel") {
        $("#divLotno").css("display", "none");
        $("#divInputQty").css("display", "none");
        $("#txtOrderNo").removeAttr("disabled", "disabled");
        $("#lblOutputQty").html("Order QTY");
        $("#lblCancelQty").html("Instruction QTY");
        $("#lblCompQty").html("Cancel QTY");
        $("#txtOrderNo").focus();
        $("#emailHelp1").css("display", "block");
        //$("#txtNeedCancelQty").attr("disabled", "disabled"); 
    } else {

        $("#txtLotNo").focus();
    }
    if (strEvent == "UndoCancel") {

        $("#txtUndoLotNo").focus();
    }
}