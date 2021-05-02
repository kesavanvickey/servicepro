function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}

function New() {
    window.location.replace("/UserMaster/Index/0");
}

function List() {
    $("#UserMasterGridList").css("display", "block");
    $("#myModal").modal("show");
    $("#UserMasterGridList").appendTo("#modalList");
    $("#myModal").show();
}


function Build() {
    var obj = new Object();
    obj.UserMaster_ID = $('#hdUserMaster_ID').val();
    obj.EmployeeMaster_ID = $('#Employee').val();
    obj.UserName = $('#UserName').val();
    obj.Password = $('#Password').val();
    obj.RollType = $('#RollType').val();
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
    if (build.EmployeeMaster_ID == "" || build.EmployeeMaster_ID == null || build.EmployeeMaster_ID == "0") {
        toastr.error("Please Choose Employee");
        $('#Employee').focus();
        return false;
    }
    else if (build.UserName == "" || build.UserName == null) {
        toastr.error("User Name Cannot be empty");
        $('#UserName').focus();
        return false;
    }
    else if (build.Password == "" || build.Password == null) {
        toastr.error("Password Cannot be empty");
        $('#Password').focus();
        return false;
    }
    else if (build.RollType == "" || build.RollType == null || build.RollType == "0") {
        toastr.error("Please Choose Roll Type");
        $('#RollType').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/UserMaster/Save',
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
                    window.location.replace("/UserMaster/Index/" + data.UserMaster_ID);
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
    if (build.UserMaster_ID == "0") {
        toastr.error('UserMaster Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/UserMaster/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/UserMaster/Index/0");
                    }
                    else if (data.ReturnValue == 2) {
                        toastr.error('Sorry! Please Try Again');
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
        url: '/UserMaster/GetEmployee',
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
        url: '/UserMaster/GetRollType',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "searchTerm": null }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            $("#RollType").select2({
                data: data
            });
            $("#RollType").val($('#hdRollType').val()).trigger("change");
        },
        error: function (data) {
        }
    });


    //Grid starts here...........

    var UserMasterGrid;

    UserMasterGrid = $("#UserMasterGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "UserMaster_ID", title: "ID", hidden: true, width: '100%' },
            { field: "EmployeeCodeTemplate", title: 'Template', sortable: true, width: '100%' },
            { field: "EmployeeCode", sortable: true, width: '100%' },
            { field: "EmployeeName", sortable: true, width: '100%' },
            { field: "UserName", sortable: true, width: '100%' },
            { field: "RollType", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });
    $("#btnSearchUserMaster").on("click", SearchAdditionalColumn);

});

function View(e) {
    var id = $('#UserMasterGrid').grid().getById(e.data.id).UserMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/UserMaster/Index/" + id + "");
    }
}

function SearchAdditionalColumn() {
    $('#UserMasterGrid').grid().reload({ searchString: $("#searchUserMaster").val() });
}