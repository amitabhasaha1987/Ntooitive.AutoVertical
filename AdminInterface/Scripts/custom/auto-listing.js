$(function () {
    create_datatable();

    //track search box value 
    $("input[type='search']").keydown(function () {
        $.cookie("search-data", JSON.stringify($("input[type='search']").val()));

    })
    .keypress(function (e) {  //for backspace
        $.cookie("search-data", JSON.stringify($("input[type='search']").val()));
    })
    .on('input paste', function () {
        $.cookie("search-data", JSON.stringify($("input[type='search']").val()));
        console.log($("input[type='search']").val());
    })

    //check cookie and fire search on datatable
    if (typeof $.cookie('search-data') !== 'undefined') {
        var search_value = JSON.parse($.cookie("search-data"));
        var oTable = $('#carlisting').dataTable();
        oTable.fnFilter(search_value);
    }
});
var userstable;
function create_datatable() {
    userstable = $('#carlisting').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/auto/auto-listing-ajax-handler",
        "bAutoWidth": false,
        "bProcessing": true,
        "order": [
            [0, "asc"]
        ],
        "aoColumns": [

            {
                "sName": "Vin",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "Make",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "Model",
                "bSearchable": true,
                "bSortable": true,
            }, {
                "sName": "Price",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "Condition",
                "bSearchable": true,
                "bSortable": true,
                "sWidth": "5%",
            }, {
                "sName": "Mileage",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "Stock No",
                "bSearchable": true,
                "bSortable": true
            },
            //{
            //    "sName": "Transmission",
            //    "bSearchable": true,
            //    "bSortable": true
            //},
            {
                "sName": "Interior Color",
                "bSearchable": true,
                "bSortable": true                
            },
            {
                "sName": "Exterior Color",
                "bSearchable": true,
                "bSortable": true
            },
            //},
            // {
            //     "sName": "Location",
            //     "bSearchable": true,
            //     "bSortable": true
            // },
         {
             "bSortable": false,
             "bSearchable": false,
             //"sWidth": "25%",
             "mRender": function (data, type, full) {
                 //return "<a class=\"btn btn-primary\" href=\"javascript:ManageAuto('" + full[0] + "');\">Manage</a>";
                 return "<a class=\"btn btn-primary\" href=\"javascript:ManageAuto('" + full[0] + "');\">Manage</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:DeleteAuto('" + full[0] + "');\">Delete</a>";
                 //return "<a class=\"btn btn-primary\" href=\"javascript:ManageAuto('" + full[0] + "');\">Manage</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:AddAuto('" + full[0] + "');\">Edit</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:DeleteAuto('" + full[0] + "');\">Delete</a>";
             }
         }
         //{
         //    "bSortable": false,
         //    "bSearchable": false,
         //    //"sWidth": "25%",
         //    "mRender": function (data, type, full) {
         //        return "<a class=\"btn btn-primary\" href=\"javascript:DeleteAuto('" + full[0] + "');\">Delete</a>";
         //        //return "<a class=\"btn btn-primary\" href=\"javascript:ManageAuto('" + full[0] + "');\">Manage</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:AddAuto('" + full[0] + "');\">Edit</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:DeleteAuto('" + full[0] + "');\">Delete</a>";
         //    }
         //}
        ]
    });

}

function ManageAuto(uniqueid) {
    debugger;
    window.location.href = "/auto/auto/manage/" + uniqueid;
}

function AddAuto(uniqueid) {
    debugger;
    window.location.href = "/auto/auto/edit/" + uniqueid;
}

function DeleteAuto(uniqueid) {
    debugger;
    // $(btn).button('loading').queue(function () {
    //$(btn).button('loading').queue(function () {
    $.ajax({
        url: "/auto/delete/" + uniqueid + '/' + true,
        type: "POST",
        contentType: 'application/json',
        dataType: "json",
        data: null,
        success: function (data, textStatus, jqXHR) {
            if (data.success) {
                $("#sucessalert").show();
                $('#sucessalert').delay(5000).fadeOut(400);
                var newcarlisting = $("#carlisting").dataTable({ bRetrieve: true });
                newcarlisting.fnDraw(true);
            } else {
                $("#failurealert").show();
                $('#failurealert').delay(5000).fadeOut(400);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#failurealert").show();
            $('#failurealert').delay(5000).fadeOut(400);
        },
        compete: function () {
            //$(btn).button('reset');
            //$(btn).dequeue();
        }
    });


    // });


}
