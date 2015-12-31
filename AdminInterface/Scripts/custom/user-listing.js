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
        var oTable = $('#userlisting').dataTable();
        oTable.fnFilter(search_value);
    }
});
var userstable;
function create_datatable() {

    userstable = $('#userlisting').dataTable({
        "bServerSide": true,
        "sAjaxSource": "/dealer/dealer-listing-ajax-handler",
        "bProcessing": true,
        "order": [
            [1, "asc"]
        ],
        "aoColumns": [

            {
                "sName": "ParticipantId",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "FirstName",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "LastName",
                "bSearchable": true,
                "bSortable": true,
            }, {
                "sName": "Email",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "PrimaryContactPhone",
                "bSearchable": true,
                "bSortable": true
            }, {
                "sName": "OfficePhone",
                "bSearchable": true,
                "bSortable": true
            },
            {
                "sName": "Roles",
                "bSearchable": false,
                "bSortable": false
            },

         {
             "bSortable": false,
             "bSearchable": false,
             "sWidth": "15%",
             "mRender": function (data, type, full) {
                 return "<a class=\"btn btn-primary\" href=\"javascript:ManageUser('" + full[0] + "');\">Manage</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:DeleteUser('" + full[0] + "');\">Delete</a>";
                 //return "<a class=\"btn btn-primary\" href=\"javascript:ManageUser('" + full[0] + "');\">Manage</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:EditUser('" + full[0] + "');\">Edit</a>&nbsp<a class=\"btn btn-primary\" href=\"javascript:DeleteUser('" + full[0] + "');\">Delete</a>";
             }
         }
            
        ]
    });

}

function ManageUser(uniqueid) {
    window.location.href = "/dealer/manage/" + uniqueid;
}

function EditUser(uniqueid) {
    window.location.href = "/dealer/edit/" + uniqueid + '/' + true;
}

//function DeleteUser(uniqueid) {
//    window.location.href = "/dealer/delete/" + uniqueid + '/' + true;
//}

function DeleteUser(uniqueid) {
    debugger;
    // $(btn).button('loading').queue(function () {
    //$(btn).button('loading').queue(function () {
    $.ajax({
        url: "/dealer/delete/" + uniqueid + '/' + true,
        type: "POST",
        contentType: 'application/json',
        dataType: "json",
        data: null,
        success: function (data, textStatus, jqXHR) {
            if (data.success) {
                $("#sucessalert").show();
                $('#sucessalert').delay(5000).fadeOut(400);
                var newuserlisting = $("#userlisting").dataTable({ bRetrieve: true });
                newuserlisting.fnDraw(true);

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


