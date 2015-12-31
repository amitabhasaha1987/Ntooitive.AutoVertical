$(function () {
    debugger;
    //This .change jquery function is the selectedchange event of the Master Level dropdownlist.
    $('#advanceSearch_VehicleType').change(function () {
        var id = $('#advanceSearch_VehicleType option:selected').val();// Variable id is storing the selected value of the Master Level dropdownlist
        var makeid = $('#advanceSearch_Make option:selected').val();
        jQuery.ajax(
        {
            type: "get",
            url: "/Home/ModelList/",// Going to the BranchList Action in Branch controller with selected id of the Master Level dropdown
            data: {ID: id,MakeId: makeid},
            dataType: "json",
            success: function (data) {//data is json result set that is returning from the BranchList Action method
                $('#advanceSearch_Model').empty();// first remove the current options if any in the Parent Level dropdown
                var optionhtml1 = '<option value="' +
                 0 + '">' + "--All Models--" + '</option>';
                $("#advanceSearch_Model").append(optionhtml1);// Binding the first option for Parent Level dropdown
                $.each(data, function (index, optiondata) {// next iterate through JSON (result set) object adding each option to the Parent Level dropdown
                    $("#advanceSearch_Model").append("<option value='" + optiondata.ModelsName + "'>" + optiondata.ModelsName + "</option>");
                });
            }
        });
    });
});

//$(function () {
//    //This .change jquery function is the selectedchange event of the Master Level dropdownlist.
   
//    $('#advanceSearch_Make').change(function () {
//        var id = $('#advanceSearch_Make option:selected').val();// Variable id is storing the selected value of the Master Level dropdownlist
//        jQuery.ajax(
//        {
//            type: "get",
//            url: "/Home/ModelListByMake/?MakeId=" + id,// Going to the BranchList Action in Branch controller with selected id of the Master Level dropdown
//            dataType: "json",
//            success: function (data) {//data is json result set that is returning from the BranchList Action method
//                $('#advanceSearch_Model').empty();// first remove the current options if any in the Parent Level dropdown
//                var optionhtml1 = '<option value="' +
//                 0 + '">' + "--All Models--" + '</option>';
//                $("#advanceSearch_Model").append(optionhtml1);// Binding the first option for Parent Level dropdown
//                $.each(data, function (index, optiondata) {// next iterate through JSON (result set) object adding each option to the Parent Level dropdown
//                    $("#advanceSearch_Model").append("<option value='" + optiondata.ModelsName + "'>" + optiondata.ModelsName + "</option>");
//                });
//            }
//        });
//    });
//});