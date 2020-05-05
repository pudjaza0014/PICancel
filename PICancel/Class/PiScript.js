function GetTypeCode(url, myTables, GetDataurl) {   
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (citys) {
            if (citys.length != 0) {
                $.each(citys, function (i, city) {
                    $("#ddlTypeCode").append('<option value="' + city.Value + '">' + city.Text + '</option>');
                   $("#ddlTypeCodeInq").append('<option value="' + city.Value + '">' + city.Text + '</option>');
                    
                });
            }
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }, 
    }); 
    
};

//function gettempdata(myTables, GetDataurl) { 
//    var strControl = [];
//    strControl = [{
//        Type: "",
//        TypeCode: $("#ddlTypeCode").val(),
//        DeliveryPlanQty: $("#txtPlanQty").val(),
//        InputDate: $("#txtInputDate").val(),
//        UserName: ""
//    }];

//    $.ajax({
//        type: 'POST',
//        url: GetDataurl,
//        data: { ArrData: strControl[0] },
//        dataType: 'json',
//        success: function (Data) {
//            myTables.clear().draw();
//            if (Data.strboolbel == true && Data.strResult == "OK") {

//                myTables.rows.add(Data.data).draw();
//            };
//        },
//        error: function (ex) {
//            alert('Failed to retrieve states.' + ex);
//        },
//    });
//    return
//};
