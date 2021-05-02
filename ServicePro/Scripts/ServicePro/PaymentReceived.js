function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}

function New() {
    window.location.replace("/PaymentReceived/Index/0");
}

function List() {
    $("#PaymentReceivedList").css("display", "block");
    $("#myModal").modal("show");
    $("#PaymentReceivedList").appendTo("#modalList");
    $("#myModal").show();
}


function Build() {
    var obj = new Object();
    obj.PaymentReceived_ID = $('#hdPaymentReceived_ID').val();
    obj.ServiceItemMaster_ID = $('#ServiceItem').val();
    obj.Amount = $('#Amount').val();
    obj.PaymentType = $('#PaymentType').val();
    obj.PaymentReferenceNo = $('#PaymentReferenceNo').val();
    obj.ReceivedDateTime = $('#ReceivedDateTime').val();
    obj.IsActive = 1;
    return obj;
}

function Save() {
    var build = Build();
    if (build.ServiceItemMaster_ID == 0 || build.ServiceItemMaster_ID == null) {
        toastr.error("Please Choose ServiceItem");
        $('#ServiceItem').focus();
        return false;
    }
    else if (build.Amount == "" || build.Amount == null || build.Amount == 0) {
        toastr.error("Please Fill Amount");
        $('#Amount').focus();
        return false;
    }
    else if (build.ReceivedDateTime == "" || build.ReceivedDateTime == null) {
        toastr.error("Please Select ReceivedDateTime");
        $('#ReceivedDateTime').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        var checkAmount = 0
        if (build.PaymentReceived_ID == 0)
        {
            checkAmount += parseFloat(document.getElementById("onLoadPaidTotal").innerHTML) + parseFloat(build.Amount);
        }
        if (parseFloat(document.getElementById("onLoadPayableTotal").innerHTML) >= checkAmount)
        {
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/PaymentReceived/Save',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        toastr.error('Amount Exceeds than Total');
                    }
                    else if (data.ReturnValue == 2) {
                        alert('Saved Successfully');
                        window.location.replace("/PaymentReceived/Index/" + data.PaymentReceived_ID);
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
        else
        {
            toastr.error('Amount Exceeds than Total');
        }
    }
}
function Delete() {

    if ($('#hdPaymentReceived_ID').val() == "0") {
        toastr.error('PaymentReceived Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/PaymentReceived/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "Id": $('#hdPaymentReceived_ID').val() }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/PaymentReceived/Index/0");
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

function GoToItemReceived()
{
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

    if ($('#hdPaymentReceived_ID').val() > 0)
    {
        $('#ServiceItem').attr("disabled", true);
        $('#Amount').attr("disabled", true);

        if ($('#hdItemReceivedHandler').val() > 0)
        {
            $('#showItemReceived').css("display", "block");
        }

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
        url: '/PaymentReceived/GetServiceItem',
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
        url: '/PaymentReceived/GetPaymentType',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            $("#PaymentType").select2({
                data: data
            });
            $("#PaymentType").val($('#hdPaymentType').val()).trigger("change");
        },
        error: function (data) {
        }
    });

    var PaymentReceivedGrid;

    PaymentReceivedGrid = $("#PaymentReceivedGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "PaymentReceived_ID", hidden: true, width: '100%' },
            { field: "Customer", sortable: true, width: '100%' },
            { field: "Item", title: 'Item', sortable: true, width: '100%' },
            { field: "Amount", sortable: true, width: '100%' },
            { field: "PaymentType", title: "PaymentType", sortable: true, width: '100%' },
            { field: "PaymentReceivedBy", title: "ReceivedBy", sortable: true, width: '100%' },
            { field: "ReceivedDateTime", title: "ReceivedDateTime", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchPaymentReceived").on("click", SearchPaymentReceived);

});

function SearchPaymentReceived() {
    $('#PaymentReceivedGrid').grid().reload({ searchString: $("#searchPaymentReceived").val() });
}

function View(e) {
    var id = $('#PaymentReceivedGrid').grid().getById(e.data.id).PaymentReceived_ID;
    if (id != null && id != 0) {
        window.location.replace("/PaymentReceived/Index/" + id + "");
    }
}

function onChangeServiceItem(e)
{

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

        document.getElementById("onLoadCustomerName").innerHTML = "";
        document.getElementById("onLoadPayableTotal").innerHTML = "";
        document.getElementById("onLoadPaidTotal").innerHTML = "";
        document.getElementById("onLoadBalance").innerHTML = "";

        $('#PaymentTableInsert tr.dynamicRow').remove();

        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/PaymentReceived/GetPaymentReceivedList',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "ServiceItemMaster_ID": $('#ServiceItem').val() }),
            dataType: "json",
            success: function (data) {
                if (data != null && data.length > 0)
                {
                    //$('#hdPaymentTotal_ID').val(data[0].PaymentTotal_ID);
                    var onLoadPayableTotal = 0;
                    var onLoadPaidTotal = 0;
                    var onLoadBalance = 0;
                    $.each(data, function (key, value) {

                        onLoadPayableTotal += parseFloat(value.ReceivableAmount);
                        onLoadPaidTotal = parseFloat(value.ReceivedAmount);

                        
                         $('#PaymentTableInsert tr:first').after('<tr class="dynamicRow"><td style="text-align:center;">' + value.Comments + '</td><td style="text-align:center;">' + value.StatusType + '</td><td style="text-align:right;">' + value.ReceivableAmount + '</td></tr>');
                        
                        //if(value.PaymentReceived_ID == $('#hdPaymentReceived_ID').val())
                        //{
                            
                        //}
                    });

                    document.getElementById("onLoadCustomerName").innerHTML = data[0].CustomerName;
                    document.getElementById("onLoadPayableTotal").innerHTML = onLoadPayableTotal;
                    document.getElementById("onLoadPaidTotal").innerHTML = onLoadPaidTotal;
                    document.getElementById("onLoadBalance").innerHTML = parseFloat(onLoadPayableTotal) - parseFloat(onLoadPaidTotal);

                    $('#hdBalance').val(parseFloat(onLoadPayableTotal) - parseFloat(onLoadPaidTotal));
                }
                $('#LoadingImg').hide();
            },
            error: function (data) {
                $('#LoadingImg').hide();
            }
        });
    }

}