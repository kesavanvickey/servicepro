function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}

function New() {
    window.location.replace("/ItemReceivedHandler/Index/0");
}

function List() {
    $("#ItemReceivedHandlerList").css("display", "block");
    $("#myModal").modal("show");
    $("#ItemReceivedHandlerList").appendTo("#modalList");
    $("#myModal").show();
}


function Build() {
    var obj = new Object();
    obj.ItemReceivedHandler_ID = $('#hdItemReceivedHandler_ID').val();
    obj.ServiceItemMaster_ID = $('#ServiceItem').val();
    obj.EmployeeMaster_ID = $('#Employee').val();
    obj.CustomerMaster_ID = $('#Customer').val();
    obj.Comments = $('#Comments').val();
    obj.ReceivedDateTime = $('#ReceivedDateTime').val();
    var Active = $('#IsActive').prop("checked");
    if (Active == true) {
        obj.IsActive = 1;
    }
    else {
        obj.IsActive = 0;
    }
    return obj;
}

function Save() {
    var build = Build();
    if (build.ServiceItemMaster_ID == "" || build.ServiceItemMaster_ID == null) {
        toastr.error("Please Choose ServiceItem");
        $('#ServiceItem').focus();
        return false;
    }
    else if (build.EmployeeMaster_ID == "" || build.EmployeeMaster_ID == null) {
        toastr.error("Please Choose Employee");
        $('#Employee').focus();
        return false;
    }
    else if (build.ReceivedDateTime == "" || build.ReceivedDateTime == null) {
        toastr.error("Please Choose ReceivedDateTime");
        $('#ReceivedDateTime').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/ItemReceivedHandler/Save',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "jsonBuild": jsonBuild }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                if (data.ReturnValue == 1) {
                    toastr.error('Record Already Exist');
                }
                else if (data.ReturnValue == 2) {
                    alert('Saved Successfully');
                    window.location.replace("/ItemReceivedHandler/Index/" + data.ItemReceivedHandler_ID + "");
                }
                else if (data.ReturnValue == 3) {
                    toastr.error('Sorry! Please Try Again');
                }
                else if (data.ReturnValue == 4) {
                    toastr.error('Error! Please Refresh & Try Again');
                }
            },
            error: function (data) {
                $('#LoadingImg').hide();
                alert('Error');
            }
        });
    }
}
function Delete() {

    if ($('#hdItemReceivedHandler_ID').val() == "0") {
        toastr.error('ItemReceivedHandler Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/ItemReceivedHandler/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "Id": $('#hdItemReceivedHandler_ID').val() }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/ItemReceivedHandler/Index/0");
                    }
                    else if (data.ReturnValue == 2) {
                        toastr.error('Sorry! Please Try Again');
                    }
                },
                error: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 3) {
                        toastr.error('Sorry! Please Try Again');
                    }
                    else {
                        toastr.error('Error! Please Refresh & Try Again');
                    }
                }
            });
        }
    }
}


$(document).ready(function () {

    var IsActive = $('#hdIsActive').val();
    if (IsActive == "1") {
        $('#IsActive').attr("checked", true);
    }
    else {
        $('#IsActive').attr("checked", false);
    }

    if ($('#hdItemReceivedHandler_ID').val() > 0) {
        flatpickr("#ReceivedDateTime", {
            enableTime: true,
            defaultDate: Date.parse($('#hdReceivedDateTime').val())
        });
    }
    else
    {
        flatpickr("#ReceivedDateTime", {
            enableTime: true
        });
    }

    $('#LoadingImg').show();
    $.ajax({
        type: "POST",
        url: '/ItemReceivedHandler/GetServiceItem',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#ServiceItem").select2({
                data: data
            });
            $("#ServiceItem").val($('#hdServiceItemMaster_ID').val()).trigger("change");
        },
        error: function (data) {
        }
    });

    $.ajax({
        type: "POST",
        url: '/ItemReceivedHandler/GetEmployee',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#Employee").select2({
                data: data
            });
            $("#Employee").val($('#hdEmployeeMaster_ID').val()).trigger("change");
        },
        error: function (data) {
        }
    });

    $.ajax({
        type: "POST",
        url: '/ItemReceivedHandler/GetCustomer',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            $("#Customer").select2({
                data: data
            });
            $("#Customer").val($('#hdCustomerMaster_ID').val()).trigger("change");
        },
        error: function (data) {
        }
    });

    var ItemReceivedHandlerGrid;

    ItemReceivedHandlerGrid = $("#ItemReceivedHandlerGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "ItemReceivedHandler_ID", hidden: true, width: '100%' },
            { field: "ServiceItem", sortable: true, width: '100%' },
            { field: "EmployeeName", sortable: true, width: '100%' },
            { field: "CustomerName", sortable: true, width: '100%' },
            { field: "ReceivedDateTime", sortable: true, width: '100%' },
            { field: "Comments", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchItemReceivedHandler").on("click", SearchItemReceivedHandler);

});

function View(e) {
    var id = $('#ItemReceivedHandlerGrid').grid().getById(e.data.id).ItemReceivedHandler_ID;
    if (id != null && id != 0) {
        window.location.replace("/ItemReceivedHandler/Index/" + id + "");
    }
}

function SearchItemReceivedHandler() {
    $('#ItemReceivedHandlerGrid').grid().reload({ searchString: $("#searchItemReceivedHandler").val() });
}
