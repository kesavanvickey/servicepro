function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}

function New() {
    window.location.replace("/WorkOrderMaster/Index/0");
}

function List() {
    $("#WorkOrderMasterList").css("display", "block");
    $("#myModal").modal("show");
    $("#WorkOrderMasterList").appendTo("#modalList");
    $("#myModal").show();
}


function Build() {
    var obj = new Object();
    obj.WorkOrderMaster_ID = $('#hdWorkOrderMaster_ID').val();
    obj.WorkCodeTemplate = $('#hdWorkCodeTemplate').val();
    obj.ServiceItemDetail_ID = $('#hdServiceItemDetail_ID').val();
    obj.ServiceStartDate = $('#ServiceStartDate').val();
    obj.ServiceEndDate = $('#ServiceEndDate').val();
    obj.StatusType = $('#StatusType').val();
   
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
    if (build.ServiceItemDetail_ID == "" || build.ServiceItemDetail_ID == null) {
        toastr.error("Please Choose Service Item");
        $('#ServiceItemDetail_ID').focus();
        return false;
    }
    else if (build.ServiceStartDate == "" || build.ServiceStartDate == null) {
        toastr.error("Please Choose ServiceStart Date");
        $('#ServiceStartDate').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({            
            type: "POST",
            url: '/WorkOrderMaster/Save',
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
                    window.location.replace("/WorkOrderMaster/Index/" + data.WorkOrderMaster_ID);
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
    var build = Build();
    if (build.WorkOrderMaster_ID == "0") {
        toastr.error('WorkOrderMaster Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/WorkOrderMaster/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/WorkOrderMaster/Index/0");
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

function GoToItemReceived() {
    window.location.replace("/ItemReceivedHandler/Index/" + $('#hdItemReceivedHandler').val());
}

$(document).ready(function () {

    var IsActive = $('#hdIsActive').val();
    if (IsActive == "1") {
        $('#IsActive').attr("checked", true);
    }
    else {
        $('#IsActive').attr("checked", false);
    }


    if ($('#hdWorkOrderMaster_ID').val() > 0) {

        $('#ServiceItemDisplay').css('display', 'block');
        $('#ServiceItemOthers').css('display', 'block');        


        flatpickr("#ServiceStartDate", {
            enableTime: true,
            defaultDate: Date.parse($('#hdServiceStartDate').val())
        });

        flatpickr("#ServiceEndDate", {
            enableTime: true,
            defaultDate: Date.parse($('#hdServiceEndDate').val())
        });

        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/WorkOrderMaster/GetServiceItem',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "ServiceItemMaster_ID": $('#hdServiceItem').val() }),
            dataType: "json",
            success: function (data) {
                $("#ServiceItem").select2({
                    data: data
                });
                $("#ServiceItem").val($('#hdServiceItem').val()).trigger("change");
            },
            error: function (data) {
            }
        });


        $.ajax({
            type: "POST",
            url: '/WorkOrderMaster/GetStatusType',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                $("#StatusType").select2({
                    data: data
                });
                $("#StatusType").val($('#hdStatusType').val()).trigger("change");
            },
            error: function (data) {
            }
        });
    }
    else {

        flatpickr("#ServiceStartDate", {
            enableTime: true
        });

        flatpickr("#ServiceEndDate", {
            enableTime: true
        });

        $('#LoadingImg').show();

        $.ajax({
            type: "POST",
            url: '/WorkOrderMaster/GetServiceItem',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "ServiceItemMaster_ID": 0 }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                $("#ServiceItem").select2({
                    data: data
                });
                $("#ServiceItem").val($('#hdServiceItem').val()).trigger("change");
            },
            error: function (data) {
            }
        });


        if ($('#hdWorkCodeTemplate').val() == 0) {
            $.ajax({
                type: "POST",
                url: '/WorkOrderMaster/GetWorkCodeTemplate',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "searchTerm": null }),
                dataType: "json",
                success: function (data) {
                    $("#WorkCodeTemplate").select2({
                        data: data
                    });
                    $('#hdWorkCodeTemplate').val(data[0].id);
                    //$("#Gender").val($('#hdGender').val()).trigger("change");
                },
                error: function (data) {
                }
            });
        }
    }


    var WorkOrderMasterGrid;

    WorkOrderMasterGrid = $("#WorkOrderMasterGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "WorkOrderMaster_ID", title: '',hidden: true, width: '100%' },
            { field: "WorkCodeTemplate", title: "", hidden: true, width: '100%' },
            { field: "ServiceItem", title: 'ServiceItem', sortable: true, width: '100%' },
            { field: "Comments", sortable: true, width: '100%' },
            { field: "EmployeeName", sortable: true, width: '100%' },
            { field: "Status", title: "Status", sortable: true, width: '100%' },
            { field: "ServiceStartDate", title: "StartDate", sortable: true, width: '100%' },
            { field: "ServiceEndDate", title: "EndDate", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": ViewWorkOrder } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchWorkOrderMaster").on("click", SearchWorkOrderMaster);
});

function ViewWorkOrder(e) {
    var id = $('#WorkOrderMasterGrid').grid().getById(e.data.id).WorkOrderMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/WorkOrderMaster/Index/" + id + "");
    }
}

function SearchWorkOrderMaster() {
    $('#WorkOrderMasterGrid').grid().reload({ searchString: $("#searchWorkOrderMaster").val() });
}


function onChangeServiceItem(e) {

    if ($('#ServiceItem').val() > 0)
    {
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/ItemReceivedHandler/CheckItemReceived',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "ServiceItem": $('#ServiceItem').val() }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                if (data > 0) {
                    $('#hdItemReceivedHandler').val(data);
                    $('#showItemReceived').css("display", "block");
                }
                else {
                    $('#showItemReceived').css("display", "none");
                }
            },
            error: function (data) {
            }
        });

        $('#ServiceItemDisplay').css('display', 'block');

        var hidden = false;
        if ($('#hdWorkOrderMaster_ID').val() > 0) {
            hidden = true;
        }

        var ServiceItemDetailGrid;
        ServiceItemDetailGrid = $("#ServiceItemDetailGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "", title: "", width: 34, type: "checkbox", hidden: hidden, events: { "click": onChangeCheckBox } },
                { field: "ServiceItemDetail_ID", title: "ID", hidden: true, width: '100%' },
                { field: "Comments", title: 'Comments', width: '100%' },
                { field: "Status", title: 'Status', hidden: hidden, width: '100%' },
                { field: "StatusType", hidden: true }
            ]
        });
        ServiceItemDetailGrid.reload({ ServiceItemMaster_ID: parseInt($('#ServiceItem').val()), ServiceItemDetail_ID: parseInt($('#hdServiceItemDetail_ID').val()) });
    }
}
function onChangeCheckBox(e)
{

    if (e.toElement.checked == true) {
        if ($('#hdCheckBox').val() == "false")
        {
            $('#hdServiceItemDetail_ID').val(e.data.record.ServiceItemDetail_ID);
            $('#hdCheckBox').val("true");
            $('#ServiceItemOthers').css('display', 'block');

            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/WorkOrderMaster/GetStatusType',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "searchTerm": null }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    $("#StatusType").select2({
                        data: data
                    });
                    $("#StatusType").val(e.data.record.StatusType).trigger("change");
                },
                error: function (data) {
                }
            });
        }
        else
        {
            e.toElement.checked = false;
            toastr.error('Please choose one checkbox');
        }
    }
    else
    {
        $('#hdCheckBox').val("false");

        if ($('#hdCheckBox').val() == "true")
            $('#hdCheckBox').val("true");
        $('#ServiceItemOthers').css('display', 'none');
    }
}
