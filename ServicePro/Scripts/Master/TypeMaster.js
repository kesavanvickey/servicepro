function ToolBarAction(e)
{
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}
function New()
{
    window.location.replace("/TypeMaster/Index/0");
}
function List()
{
    $("#TypeMasterGridList").css("display", "block");
    $("#myModal").modal("show");
    $("#TypeMasterGridList").appendTo("#modalList");
    $("#myModal").show();
}
function Build() {
    var obj = new Object();
    obj.TypeMaster_ID = $('#hdTypeMasterId').val();
    obj.TypeMasterName = $('#TypeMasterName').val();
    obj.ShortName = $('#ShortName').val();
    obj.Parent_ID = $('#Parent_ID').val();
    obj.Description = $('#Description').val();
    obj.CompanyMaster_ID = $('#CompanyMaster_ID').val();
    var Active = $('#IsActive').prop("checked");
    if (Active == true)
    {
        obj.IsActive = 1;
    }
    else
    {
        obj.IsActive = 0;
    }
    return obj;
}
function Save() {
    var build = Build();
    if (build.TypeMasterName == "" || build.TypeMasterName == null) {
        toastr.error("TypeMaster Name Cannot be empty");
        $('#TypeMasterName').focus();
        return false;
    }
    else
    {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/TypeMaster/Save',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "jsonBuild": jsonBuild }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                if (data.ReturnValue == 1) {
                    toastr.error('TypeMaster Name Already Exist');
                }
                else if (data.ReturnValue == 2) {
                    alert('Saved Successfully');
                    window.location.replace("/TypeMaster/Index/" + data.TypeMaster_ID);
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
    if (build.TypeMaster_ID == "0") {
        toastr.error('Type Master Cannot be empty');
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/TypeMaster/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/TypeMaster/Index/0");
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
});



//TypeDetail Master Grid
function AddTypeDetail() {
        $("#TypeDetail_ID").val("");
        $("#TypeDetail_Name").val("");
        $("#TypeDetail_Description").val("");
        $("#TypeDetail_Active").val("");
        $("#typeDetailModal").modal("show");
    }
function EditTypeDetail(e) {
        $('#TypeDetail_ID').val(e.data.id);
        $("#TypeDetail_ID").val(e.data.record.TypeDetailMaster_ID);
        $("#TypeDetail_Name").val(e.data.record.TypeName);
        $("#TypeDetail_Description").val(e.data.record.Description);
        var active = e.data.record.IsActive;
        if (active == true)
        {
            $("#TypeDetail_Active").attr("checked", true);
        }
        else
        {
            $("#TypeDetail_Active").attr("checked", false);
        }
        $("#typeDetailModal").modal("show");
    }
    function SaveTypeDetail() {
        var typeDetailMaster = {
            TypeDetailMaster_ID: $("#TypeDetail_ID").val(),
            TypeName: $("#TypeDetail_Name").val(),
            Description: $("#TypeDetail_Description").val(),
            TypeMaster_ID: $('#hdTypeMasterId').val(),
            CompanyMaster_ID: $('#hdCompanyId').val()
        };
        if ($("#TypeDetail_Active")[0].checked == true)
        {
            typeDetailMaster.IsActive = 1;
        }
        if (typeDetailMaster.TypeName == null)
        {
            toastr.info("Type Name Cannot be Empty");
            return null;
        }
        else
        {
            $('#LoadingImg').show();
            $.ajax({ url: "/TypeMaster/GridTypeDetailSave", type: "POST", data: { typeDetailMaster: typeDetailMaster } })
            .done(function (data) {
                $('#LoadingImg').hide();
                $('#TypeDetailGrid').grid().reload({ Parameter: $('#hdTypeMasterId').val() });
                $("#typeDetailModal").modal("hide");
                if (data.ReturnValue == 1) {
                    toastr.success("Saved Successfully");
                }
                else if (data.ReturnValue == 2) {
                    toastr.error("Type Name Already Exist");
                }
            })
            .fail(function () {
                $('#LoadingImg').hide();
                toastr.info('Unable To Save');
                $("#typeDetailModal").modal("hide");
            });
        }
    }
    function RemoveTypeDetail(e) {
        if (confirm("Are you sure you want to delete?"))
        {
            $('#LoadingImg').show();
            $.ajax({ url: "/TypeMaster/GridTypeDetailDelete", type: "POST", data: { id: e.data.record.TypeDetailMaster_ID } })
            .done(function (data) {
                $('#LoadingImg').hide();
                if (data == true)
                {
                    toastr.success('Deleted Successfully');
                    $('#TypeDetailGrid').grid().reload({ Parameter: $('#hdTypeMasterId').val() });
                }
            })
            .fail(function (data) {
                $('#LoadingImg').hide();
                toastr.info('Unable To Delete');
            });
        }
    }
  
    $(document).ready(function () {
        var TypeDetailGrid;
        var TypeMaterGrid;

        if ($('#hdTypeMasterId').val() > 0)
        {
            $('#TypeDetailGridHide').css('display', 'block');
            TypeDetailGrid = $("#TypeDetailGrid").grid({
                dataKey: "ID",
                uiLibrary: "bootstrap",
                columns: [
                    { field: "TypeDetailMaster_ID", title:"ID", hidden :true, width:'100%' },
                    { field: "TypeName", sortable: true, width: '100%' },
                    { field: "Description", title: "Description", sortable: true, width: '100%' },
                    { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
                    { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditTypeDetail } },
                    { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveTypeDetail } }
                ],
                pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }

            });
            TypeDetailGrid.reload({ Parameter: $('#hdTypeMasterId').val() });
            $("#btnAddTypeDetail").on("click", AddTypeDetail);
            $("#btnSave").on("click", SaveTypeDetail);
            $("#btnSearchTypeDetail").on("click", SearchTypeDetail);
        }

        TypeMaterGrid = $("#TypeMasterGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "TypeMaster_ID", title: "ID", hidden: true, width: '100%' },
                { field: "TypeMasterName", sortable: true, width: '100%' },
                { field: "Description", title: "Description", sortable: true, width: '100%' },
                { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
                { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": View } }
            ],
            pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
        }); 
        $("#btnSearchTypeMaster").on("click", SearchTypeMaster);
    });
function View(e)
{
    var id = $('#TypeMasterGrid').grid().getById(e.data.id).TypeMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/TypeMaster/Index/" + id + "");
    }
}

function SearchTypeDetail() {
    $('#TypeDetailGrid').grid().reload({ searchString: $("#searchTypeDetail").val() });
}
function SearchTypeMaster() {
    $('#TypeMasterGrid').grid().reload({ searchString: $("#searchTypeMaster").val() });
}

