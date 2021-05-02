function ToolBarAction(e) {
    if (e.id == "ListAction") List();
}

function List() {
    $("#InvoiceGridList").css("display", "block");
    $("#myModal").modal("show");
    $("#InvoiceGridList").appendTo("#modalList");
    $("#myModal").show();
}

function onChangeServiceItem() {
    if ($('#ServiceItem').val() > 0) {
        if (confirm('Do you want to Generate Invoice?') == true) {
            document.getElementById("callerId").href = "/Home/Invoice?Id=0&serviceItemId=" + $('#ServiceItem').val();
            document.getElementById("callerId").click();
        }
    }
}


$(document).ready(function () {

    $('#LoadingImg').show();
    $.ajax({
        type: "POST",
        url: '/ItemReceivedHandler/GetServiceItem',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            $("#ServiceItem").select2({
                data: data
            });
        },
        error: function (data) {
            $('#LoadingImg').hide();
        }
    });


    var InvoiceGrid;

    InvoiceGrid = $("#InvoiceGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "InvoiceID", hidden: false, width: '100%' },
            { field: "ServiceItemId", sortable: true, width: '100%' },
            { field: "CustomerId", sortable: true, width: '100%' },
            { field: "CustomerName", sortable: true, width: '100%' },
            { field: "ItemName", sortable: true, width: '100%' },
            { field: "PrintDateTime", sortable: true, width: '100%' },
            { field: "Created_UserId", title:'Created By', sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchInvoice").on("click", SearchInvoice);
});


function View(e) {
    var id = $('#InvoiceGrid').grid().getById(e.data.id).InvoiceID;
    if (id != null && id != 0) {
        document.getElementById("callerId").href = "/Home/Invoice?Id=" + id + "&serviceItemId=0";
        document.getElementById("callerId").click();
    }
}

function SearchInvoice() {
    $('#InvoiceGrid').grid().reload({ searchString: $("#searchInvoice").val() });
}