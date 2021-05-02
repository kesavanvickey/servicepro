function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}

function New() {
    window.location.replace("/ServiceItemMaster/Index/0");
}

function List() {
    $("#ServiceItemMasterList").css("display", "block");
    $("#myModal").modal("show");
    $("#ServiceItemMasterList").appendTo("#modalList");
    $("#myModal").show();
}


function Build() {
    var obj = new Object();
    obj.ServiceItemMaster_ID = $('#hdServiceItemMaster_ID').val();
    obj.ServiceCodeTemplate = $('#ServiceCodeTemplate').val();
    obj.CustomerMaster_ID = $('#CustomerMaster_ID').val();
    obj.Brand = $('#Brand').val();
    obj.Model = $('#Model').val();
    obj.ItemOrderDate = $('#ItemOrderDate').val();
    obj.ItemExpectedDeliverDate = $('#ItemExpectedDeliverDate').val();
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
    if (build.CustomerMaster_ID == "" || build.CustomerMaster_ID == null) {
        toastr.error("Please Choose Customer");
        $('#CustomerMaster_ID').focus();
        return false;
    }
    else if (build.ItemOrderDate == "" || build.ItemOrderDate == null) {
        toastr.error("Please Choose ItemOrderDate");
        $('#ItemOrderDate').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/ServiceItemMaster/Save',
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
                    window.location.replace("/ServiceItemMaster/Index/" + data.ServiceItemMaster_ID);
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
    if (build.ServiceItemMaster_ID == "0") {
        toastr.error('ServiceItemMaster Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/ServiceItemMaster/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/ServiceItemMaster/Index/0");
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

    if ($('#hdServiceItemMaster_ID').val() > 0)
    {
        if ($('#hdItemReceivedHandler').val() > 0)
        {
            $('#showItemReceived').css("display", "block");
        }

        flatpickr("#ItemOrderDate", {
            defaultDate: Date.parse($('#hdItemOrderDate').val())
        });

        flatpickr("#ItemExpectedDeliverDate", {
            defaultDate: Date.parse($('#hdItemExpectedDeliverDate').val())
        });
    }
    else
    {
        flatpickr("#ItemOrderDate", {

        });

        flatpickr("#ItemExpectedDeliverDate", {

        });
    }

    $('#LoadingImg').show();
    $.ajax({
        type: "POST",
        url: '/ServiceItemMaster/GetServiceCodeTemplate',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#ServiceCodeTemplate").select2({
                data: data
            });
            //$("#Gender").val($('#hdGender').val()).trigger("change");
        },
        error: function (data) {
        }
    });


    $.ajax({
        type: "POST",
        url: '/ServiceItemMaster/GetCustomer',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#CustomerMaster_ID").select2({
                data: data
            });
            if ($('#hdCustomerMaster_ID').val() > 0)
            {
                $("#CustomerMaster_ID").val($('#hdCustomerMaster_ID').val()).trigger("change");
            }
        },
        error: function (data) {
        }
    });

    $.ajax({
        type: "POST",
        url: '/ServiceItemMaster/GetStatusType',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            $("#StatusType").select2({
                data: data
            });
        },
        error: function (data) {
        }
    });



    //Grid starts here...........

    var ServiceItemMasterGrid;

    ServiceItemMasterGrid = $("#ServiceItemMasterGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "ServiceCodeTemplateName", title: 'TemplateCode' , sortable: true, width: '100%' },
            { field: "ServiceItemMaster_ID", title: "ID", hidden: false, width: '100%' },
            { field: "Customer", title: 'Customer', sortable: true, width: '100%' },
            { field: "Brand", sortable: true, width: '100%' },
            { field: "Model", sortable: true, width: '100%' },
            { field: "ItemOrderDate", title:"OrderDate", sortable: true, width: '100%' },
            { field: "ItemExpectedDeliverDate", title:"DeliverDate", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive",  sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchServiceItemMaster").on("click", SearchServiceItemMaster);

    if ($('#hdServiceItemMaster_ID').val() > 0)
    {
        $('#ServiceItemDetailGridList').css('display', 'block');

        var ServiceItemDetailGrid;

        ServiceItemDetailGrid = $("#ServiceItemDetailGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "ServiceItemDetail_ID", title: "ID", hidden: true, width: '100%' },
                { field: "PaymentTotal_ID", title: "ID", hidden: true, width: '100%' },
                { field: "StatusType", hidden: true, width: '100%' },
                { field: "Comments", sortable: true, width: '100%' },
                { field: "Amount", sortable: true, width: '100%' },
                { field: "Status", title: 'Status', sortable: true, width: '100%' },
                { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
                { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditServiceItemDetail } },
                { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveServiceItemDetail } }
            ]
        });
        $("#btnAddItemDetailGrid").on("click", AddServiceItemDetail);
        $("#btnSaveServiceItemDetail").on("click", SaveServiceItemDetail);
        ServiceItemDetailGrid.reload({ ServiceItemMaster_ID: $('#hdServiceItemMaster_ID').val() });
    }

});

function View(e) {
    var id = $('#ServiceItemMasterGrid').grid().getById(e.data.id).ServiceItemMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/ServiceItemMaster/Index/" + id + "");
    }
}

function SearchServiceItemMaster() {
    $('#ServiceItemMasterGrid').grid().reload({ searchString: $("#searchServiceItemMaster").val() });
}




//ItemDetailMaster and PaymentReceivable

function AddServiceItemDetail() {
    $("#ServiceItemDetail_ID").val("");
    $("#PaymentTotal_ID").val("");
    $("#Comments").val("");
    $("#Amount").val("");
    $("#StatusType").val(0).trigger('change');
    $("#ServiceItemDetail_Active").attr("checked", true);
    $("#ServiceItemDetailModal").modal("show");
}
function EditServiceItemDetail(e) {

    $("#ServiceItemDetail_ID").val(e.data.record.ServiceItemDetail_ID);
    $("#PaymentTotal_ID").val(e.data.record.PaymentTotal_ID);
    $("#Comments").val(e.data.record.Comments);
    $("#Amount").val(e.data.record.Amount);
    $("#StatusType").val(e.data.record.StatusType).trigger('change');
    $("#ServiceItemDetail_Active").val(e.data.record.ServiceItemDetail_Active);

    var active = e.data.record.IsActive;
    if (active == true) {
        $("#ServiceItemDetail_Active").attr("checked", true);
    }
    else {
        $("#ServiceItemDetail_Active").attr("checked", false);
    }
    if (e.data.record.ServiceItemDetail_ID > 0)
    {
        $('#Amount').attr('disabled', true);
    }
    $("#ServiceItemDetailModal").modal("show");
}
function SaveServiceItemDetail() {
    var serviceItemDetail = {
        ServiceItemDetail_ID: $("#ServiceItemDetail_ID").val(),
        ServiceItemMaster_ID: $("#hdServiceItemMaster_ID").val(),
        Comments: $("#Comments").val(),
        StatusType: $("#StatusType").val(),
        IsActive: $('#ServiceItemDetail_Active').val(),
    };

    serviceItemDetail.PaymentReceivable = new Object();
    serviceItemDetail.PaymentReceivable.PaymentTotal_ID = $("#PaymentTotal_ID").val();
    serviceItemDetail.PaymentReceivable.Amount = $("#Amount").val();

    if ($("#ServiceItemDetail_Active")[0].checked == true) {
        serviceItemDetail.IsActive = 1;
    }

    if (serviceItemDetail.Comments == null) {
        toastr.info("Comments cannot be empty");
        return null;
    }
    if (serviceItemDetail.StatusType == 0 || serviceItemDetail.StatusType == null) {
        toastr.info("Please Choose StatusType");
        return null;
    }
    if (serviceItemDetail.PaymentReceivable.Amount == null) {
        toastr.info("Amount cannot be empty");
        return null;
    }
    else {
        $('#LoadingImg').show();
        $.ajax({ url: "/ServiceItemMaster/SaveServiceItemDetail", type: "POST", data: { serviceItemDetail: serviceItemDetail } })
        .done(function (data) {
            $('#LoadingImg').hide();
            $('#ServiceItemDetailGrid').grid().reload({ ServiceItemMaster_ID: $('#hdServiceItemMaster_ID').val() });
            $("#ServiceItemDetailModal").modal("hide");
            if (data.ReturnValue == 1) {
                toastr.success("Saved Successfully");
            }
            else if (data.ReturnValue == 2) {
                toastr.error("error");
            }
        })
        .fail(function () {
            $('#LoadingImg').hide();
            toastr.info('Unable To Save');
        });
    }
}

function RemoveServiceItemDetail(e) {
    if (confirm("Are you sure you want to delete?")) {
        $('#LoadingImg').show();
        $.ajax({ url: "/ServiceItemMaster/DeleteServiceItemDetail", type: "POST", data: { id: e.data.record.ServiceItemDetail_ID } })
        .done(function (data) {
            $('#LoadingImg').hide();
            if (data == "Deleted Successfully")
            {
                toastr.success('Deleted Successfully');
            }
            else
            {
                toastr.error(data);
            }
            $('#ServiceItemDetailGrid').grid().reload({ ServiceItemMaster_ID: $('#hdServiceItemMaster_ID').val() });
            
        })
        .fail(function (data) {
            $('#LoadingImg').hide();
            toastr.info('Unable To Delete');
        });
    }
}
//ServiceItemDetail Grid End
