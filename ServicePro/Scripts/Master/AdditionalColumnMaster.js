function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}
function New() {
    window.location.replace("/AdditionalColumn/Index/0");
}
function List() {
    $("#AdditionalColumnGridList").css("display", "block");
    $("#myModal").modal("show");
    $("#AdditionalColumnGridList").appendTo("#modalList");
    $("#myModal").show();
}

function Build() {
    var obj = new Object();
    obj.AdditionalColumnMaster_ID = $('#hdAdditionalColumnMaster_ID').val();
    obj.TableName = $('#cboTableName').val();
    obj.AdditionalColumnName = $('#cboAdditionalColumnName').val();
    obj.DisplayName = $('#DisplayName').val();
    obj.DataType = $('#cboDataType').val();
    obj.CompanyMaster_ID = $('#hdCompanyMaster_ID').val();
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
    if (build.TableName == "" || build.TableName == null || build.TableName == "0") {
        toastr.error("Please Choose TableName");
        $('#cboTableName').focus();
        return false;
    }
    else if (build.AdditionalColumnName == "" || build.AdditionalColumnName == null || build.AdditionalColumnName == "0")
    {
        toastr.error("Please Choose Additional ColumnName");
        $('#cboAdditionalColumnName').focus();
        return false;
    }
    else if (build.DisplayName == "" || build.DisplayName == null) {
        toastr.error("Display Name Cannot be empty");
        $('#DisplayName').focus();
        return false;
    }
    else if (build.DataType == "" || build.DataType == null || build.DataType == "0") {
        toastr.error("Please Choose DataType");
        $('#DataType').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/AdditionalColumn/Save',
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
                    window.location.replace("/AdditionalColumn/Index/" + data.AdditionalColumnMaster_ID);
                }
            },
            error: function (data) {
                $('#LoadingImg').hide();
                if (data.ReturnValue == 3) {
                    toastr.error('Sorry! Please Try Again');
                }
                else if (data.ReturnValue == 4) {
                    toastr.error('Error! Please Refresh & Try Again');
                }
            }
        });
    }
}
function Delete() {
    var build = Build();
    if (build.AdditionalColumnMaster_ID == "0") {
        toastr.error('Additional Column Master Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/AdditionalColumn/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/AdditionalColumn/Index/0");
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
    $('#LoadingImg').show();
    $.ajax({
        type: "POST",
        url: '/Additionalcolumn/GetTableName',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#cboTableName").select2({
                data: data
            });
            $("#cboTableName").val($('#hdTableName').val()).trigger("change");
        },
        error: function (data) {
        }
    });

    $.ajax({
        type: "POST",
        url: '/Additionalcolumn/GetAdditionalColumnName',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#cboAdditionalColumnName").select2({
                data: data
            });
            $("#cboAdditionalColumnName").val($('#hdAdditionalColumnName').val()).trigger("change");
        },
        error: function (data) {
        }
    });
    $.ajax({
        type: "POST",
        url: '/Additionalcolumn/GetDataType',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $("#cboDataType").select2({
                data: data
            });
            $("#cboDataType").val($('#hdDataType').val()).trigger("change");
            $('#LoadingImg').hide();
        },
        error: function (data) {
        }
    });

    


    //Grid starts here...........

    var AdditionalColumnGrid;

    AdditionalColumnGrid = $("#AdditionalColumnGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "AdditionalColumnMaster_ID", title: "ID", hidden: true, width: '100%' },
            { field: "TableName", sortable: true, width: '100%' },
            { field: "AdditionalColumnName", title: "Additional Column", sortable: true, width: '100%' },
            { field: "DataType", sortable: true, width: '100%' },
            { field: "DisplayName", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchAdditionalColumn").on("click", SearchAdditionalColumn);
});

function View(e) {
    var id = $('#AdditionalColumnGrid').grid().getById(e.data.id).AdditionalColumnMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/AdditionalColumn/Index/" + id + "");
    }
}
function SearchAdditionalColumn() {
    $('#AdditionalColumnGrid').grid().reload({ searchString: $("#searchAdditionalColumn").val() });
}
