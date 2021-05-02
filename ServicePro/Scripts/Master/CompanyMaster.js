function ToolBarAction(e) {
    if (e.id == "SaveAction") Save();
    if (e.id == "DeleteAction") Delete();
    if (e.id == "NewAction") New();
    if (e.id == "ListAction") List();
}
function New() {
    window.location.replace("/Company/Index/0");
}
function List() {
    $("#CompanyMasterGridList").css("display", "block");
    $("#myModal").modal("show");
    $("#CompanyMasterGridList").appendTo("#modalList");
    $("#myModal").show();
}
function Build() {
    var obj = new Object();
    obj.CompanyMaster_ID = $('#hdCompanyMaster_ID').val();
    obj.CompanyName = $('#CompanyName').val();
    obj.CompanyType = $('#CompanyType').val();
    obj.CompanyLogo = $('#CompanyLogo').val();
    obj.ReportSignature = $('#ReportSignature').val();
    obj.ReportBottom = $('#ReportBottom').val();
    obj.TinNo = $('#TinNo').val();
    obj.UserName = $('#UserName').val();
    obj.Password = $('#Password').val();
    obj.Recovery_Mobile = $('#Recovery_Mobile').val();
    obj.Recovery_Email = $('#Recovery_Email').val();
    obj.Recovery_Question = $('#Recovery_Question').val();
    obj.Recovery_Answer = $('#Recovery_Answer').val();
    obj.ActivationMaster_Key = $('#hdActivationMaster_Key').val();
    obj.IsActive = 1;
    //var Active = $('#IsActive').prop("checked");
    //if (Active == true) {
    //    obj.IsActive = 1;
    //}
    //else {
    //    obj.IsActive = 0;
    //}
    return obj;
}
function Save() {
    var build = Build();
    if (build.CompanyName == "" || build.CompanyName == null) {
        toastr.error("Company Name Cannot be empty");
        $('#CompanyName').focus();
        return false;
    }
    else if (build.CompanyType == "" || build.CompanyType == null) {
        toastr.error("Company Type Cannot be empty");
        $('#CompanyType').focus();
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
    else if (build.Recovery_Mobile == "" || build.Recovery_Mobile == null) {
        toastr.error("Mobile No Cannot be empty");
        $('#Recovery_Mobile').focus();
        return false;
    }
    else if (build.Recovery_Email == "" || build.Recovery_Email == null) {
        toastr.error("Email Cannot be empty");
        $('#Recovery_Email').focus();
        return false;
    }
    else if (build.Recovery_Question == "" || build.Recovery_Question == null) {
        toastr.error("Recovery Question Cannot be empty");
        $('#Recovery_Question').focus();
        return false;
    }
    else if (build.Recovery_Answer == "" || build.Recovery_Answer == null) {
        toastr.error("Recovery Answer Cannot be empty");
        $('#Recovery_Answer').focus();
        return false;
    }
    else {
        jsonBuild = JSON.stringify(build);
        if (!jsonBuild) return null;
        $('#LoadingImg').show();
        $.ajax({
            type: "POST",
            url: '/Company/Save',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "jsonBuild": jsonBuild }),
            dataType: "json",
            success: function (data) {
                $('#LoadingImg').hide();
                if (data.ReturnValue == 1) {
                    toastr.error('Sorry! Product Key Already Used');
                }
                else if (data.ReturnValue == 2) {
                    alert('Saved Successfully');
                    window.location.replace("/Company/Index/" + data.CompanyMaster_ID);
                }
                else if (data.ReturnValue == 5) {
                    toastr.info('Please Activate your Product Key First');
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
    if (build.CompanyMaster_ID == "0") {
        toastr.error("Company Master cannot be empty");
        return false;
    }
    else {
        if (confirm("Are you sure you want to delete?")) {
            jsonBuild = JSON.stringify(build);
            if (!jsonBuild) return null;
            $('#LoadingImg').show();
            $.ajax({
                type: "POST",
                url: '/Company/Delete',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ "jsonBuild": jsonBuild }),
                dataType: "json",
                success: function (data) {
                    $('#LoadingImg').hide();
                    if (data.ReturnValue == 1) {
                        alert('Deleted Successfully');
                        window.location.replace("/Company/Index/0");
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

    //Compnay List Grid
    var CompanyMasterGrid;
    CompanyMasterGrid = $("#CompanyMasterGrid").grid({
        dataKey: "ID",
        uiLibrary: "bootstrap",
        columns: [
            { field: "CompanyMaster_ID", title: "ID", hidden: true, width: '100%' },
            { field: "CompanyName", sortable: true, width: '100%' },
            { field: "TinNo", title: "TinNo", sortable: true, width: '100%' },
            { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
            { title: "", field: "View", width: 50, type: "icon", icon: "glyphicon-search", tooltip: "View", events: { "click": ComapanyGridView } }
        ],
        pager: { enable: true, limit: 5, sizes: [2, 5, 10, 20] }
    });





    //Address Grid
    if ($('#hdCompanyMaster_ID').val() > 0) {
        var AddressGrid;
        $('#AddressGridList').css('display', 'block');
        AddressGrid = $("#AddressGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "AddressMaster_ID", title: "ID", hidden: true, width: '100%' },
                { field: "AddressType", hidden: true, width: '100%' },
                { field: "Created_UserID", hidden: true, width: '100%' },
                { field: "Created_DateTime", hidden: true, width: '100%' },
                { field: "City", hidden: true, width: '100%' },
                { field: "State", hidden: true, width: '100%' },
                { field: "Country", hidden: true, width: '100%' },
                { field: "AddressTypeName", title: "AddressType", sortable: true, width: '100%' },
                { field: "Address1", title: "Address1", sortable: true, width: '100%' },
                { field: "Address2", title: "Address2", sortable: true, width: '100%' },
                { field: "Address3", title: "Address3", sortable: true, width: '100%' },
                { field: "CityName", title: "City", sortable: true, width: '100%' },
                { field: "StateName", title: "State", sortable: true, width: '100%' },
                { field: "CountryName", hidden: true, title: "Country", sortable: true, width: '100%' },
                { field: "Pincode", title: "Pincode", sortable: true, width: '100%' },
                { field: "ContactNo1", title: "ContactNo1", sortable: true, width: '100%' },
                { field: "ContactNo2", hidden: true, title: "ContactNo2", sortable: true, width: '100%' },
                { field: "Email", title: "Email", sortable: true, width: '100%' },
                { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
                { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditAddress } },
                { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveAddress } }
            ]
            //pager: { enable: false, limit: 5, sizes: [2, 5, 10, 20] }
        });

        var IDProofGrid;
        $('#IDProofGridList').css('display', 'block');
        IDProofGrid = $("#IDProofGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "IDProofMaster_ID", title: "ID", hidden: true, width: '100%' },
                { field: "CodeTemplate", hidden: true, width: '100%' },
                { field: "Ref_ID", hidden: true, width: '100%' },
                { field: "IDProofType", hidden: true, width: '100%' },
                { field: "Created_UserID", hidden: true, width: '100%' },
                { field: "Created_DateTime", hidden: true, width: '100%' },
                { field: "IDProofTypeName", title: 'IDProofType', width: '100%' },
                { field: "IDProofData", title: 'IDProofData', width: '100%' },
                { field: "IsActive", title: "IsActive", sortable: true, width: '100%' },
                { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditIDProof } },
                { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveIDProof } }
            ]
            //pager: { enable: false, limit: 5, sizes: [2, 5, 10, 20] }
        });

        var StorageMasterGrid;
        $('#StorageMasterGridList').css('display', 'block');
        StorageMasterGrid = $("#StorageMasterGrid").grid({
            dataKey: "ID",
            uiLibrary: "bootstrap",
            columns: [
                { field: "StorageMaster_ID", title: "ID", hidden: true, width: '100%' },
                { field: "CodeTemplate", hidden: true, width: '100%' },
                { field: "Ref_ID", hidden: true, width: '100%' },
                { field: "FileName", title: 'FileName', width: '100%' },
                { field: "StorageType", hidden: true, width: '100%' },
                { field: "StorageTypeName", title: 'StorageType', hidden: false, width: '100%' },
                { field: "Extension", width: '100%' },
                { field: "ContentType", hidden: true, width: '100%' },
                { field: "StorageMaster_Data", hidden: true, width: '100%' },
                { field: "FileSize", width: '100%' },
                { field: "IsActive", sortable: true, width: '100%' },
                { title: "Download", width: '100%', type: "icon", icon: "glyphicon-download", tooltip: "Download", events: { "click": DownloadStorageMaster } },
                { title: "", field: "Edit", width: 34, type: "icon", icon: "glyphicon-pencil", tooltip: "Edit", events: { "click": EditStorageMaster } },
                { title: "", field: "Delete", width: 34, type: "icon", icon: "glyphicon-remove", tooltip: "Delete", events: { "click": RemoveStorageMaster } }
            ]
        });


        //Address
        AddressGrid.reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
        $("#btnAddAddress").on("click", AddAddress);
        $("#btnAddressSave").on("click", SaveAddress);
        //$("#btnCompanyAddressSearch").on("click", CompanyAddressSearch);

        //IDProof
        IDProofGrid.reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
        $("#btnAddIDProof").on("click", AddIDProof);
        $("#btnIDProofSave").on("click", SaveIDProof);

        //StorageMaster
        StorageMasterGrid.reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
        $("#btnAddStorageMaster").on("click", AddStorageMaster);
        $("#btnStorageMasterSave").on("click", SaveStorageMaster);
    }


    $("#btnCompanySearch").on("click", ComapanyGridSearch);
});


//View CompanyGrid
function ComapanyGridView(e) {
    var id = $('#CompanyMasterGrid').grid().getById(e.data.id).CompanyMaster_ID;
    if (id != null && id != 0) {
        window.location.replace("/Company/Index/" + id + "");
    }
}


//Company Grid Search
function ComapanyGridSearch() {
    $('#CompanyMasterGrid').grid().reload({ searchString: $("#searchCompanyMaster").val() });
}







//Address Grid Search

//function CompanyAddressSearch() {
//    $('#CompanyAddressGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null, searchString: $("#searchCompanyAddress").val() });
//}
function AddAddress() {
    $("#AddressMaster_ID").val("");
    $("#AddressType").val(0).trigger('change');
    $("#Address1").val("");
    $("#Address2").val("");
    $("#Address3").val("");
    $("#City").val(0).trigger('change');
    $("#State").val(0).trigger('change');
    $("#Country").val(0).trigger('change');
    $("#Pincode").val("");
    $("#ContactNo1").val("");
    $("#ContactNo2").val("");
    $("#Email").val("");
    $("#IsActiveAddress").attr("checked", true);
    $("#AddressModal").modal("show");
}
function EditAddress(e) {

    $("#AddressMaster_ID").val(e.data.record.AddressMaster_ID);
    $("#AddressType").val(e.data.record.AddressType).trigger("change");
    $("#Address1").val(e.data.record.Address1);
    $("#Address2").val(e.data.record.Address2);
    $("#Address3").val(e.data.record.Address3);
    $("#City").val(e.data.record.City).trigger("change");
    $("#State").val(e.data.record.State).trigger("change");
    $("#Country").val(e.data.record.Country).trigger("change");
    $("#Pincode").val(e.data.record.Pincode);
    $("#ContactNo1").val(e.data.record.ContactNo1);
    $("#ContactNo2").val(e.data.record.ContactNo2);
    $("#Email").val(e.data.record.Email);
    $("#AddressMaster_Created_UserID").val(e.data.record.Created_UserID);
    $("#AddressMaster_Created_DateTime").val(e.data.record.Created_DateTime);

    var active = e.data.record.IsActive;
    if (active == true) {
        $("#IsActiveAddress").attr("checked", true);
    }
    else {
        $("#IsActiveAddress").attr("checked", false);
    }
    $("#AddressModal").modal("show");
}
function SaveAddress() {
    var addressMaster = {
        AddressMaster_ID: $("#AddressMaster_ID").val(),
        AddressType: $("#AddressType").val(),
        Address1: $("#Address1").val(),
        Address2: $('#Address2').val(),
        Address3: $('#Address3').val(),
        City: $('#City').val(),
        State: $('#State').val(),
        Country: $('#Country').val(),
        Pincode: $('#Pincode').val(),
        ContactNo1: $('#ContactNo1').val(),
        ContactNo2: $('#ContactNo2').val(),
        Email: $('#Email').val(),
        Ref_ID: $('#hdCompanyMaster_ID').val(),
        CodeTemplate: null,
        Created_UserID: $('#AddressMaster_Created_UserID').val(),
        Created_DateTime: $('#AddressMaster_Created_DateTime').val()
    };
    if ($("#IsActiveAddress")[0].checked == true) {
        addressMaster.IsActive = 1;
    }
    if (addressMaster.AddressType == null) {
        toastr.info("Please Choose Address Type");
        return null;
    }
    else if (addressMaster.ContactNo1 == null) {
        toastr.info("ContactNo1 Cannot be empty");
        return null;
    }
    else {
        $('#LoadingImg').show();
        $.ajax({ url: "/Home/SaveAddress", type: "POST", data: { addressMaster: addressMaster } })
        .done(function (data) {
            $('#LoadingImg').hide();
            $('#AddressGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
            $("#AddressModal").modal("hide");
            if (data.ReturnValue == 1) {
                toastr.success("Saved Successfully");
            }
            else if (data.ReturnValue == 2) {
                toastr.error("ContactNo Already Exist");
            }
        })
        .fail(function () {
            $('#LoadingImg').hide();
            toastr.info('Unable To Save');
        });
    }
}

function RemoveAddress(e) {
    if (confirm("Are you sure you want to delete?")) {
        $('#LoadingImg').show();
        $.ajax({ url: "/Home/DeleteAddress", type: "POST", data: { id: e.data.record.AddressMaster_ID } })
        .done(function (data) {
            $('#LoadingImg').hide();
            if (data == true) {
                toastr.success('Deleted Successfully');
                $('#AddressGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
            }
        })
        .fail(function (data) {
            $('#LoadingImg').hide();
            toastr.info('Unable To Delete');
        });
    }
}
//Address Grid End



//IDProof
function AddIDProof() {
    $("#IDProofMaster_ID").val("");
    $("#CodeTemplate").val(0).trigger('change');
    $("#Ref_ID").val("");
    $("#IDProofType").val(0).trigger('change');
    $("#IDProofData").val("");
    $("#IsActiveIDProof").attr("checked", true);
    $("#IDProofModal").modal("show");
}
function EditIDProof(e) {

    $("#IDProofMaster_ID").val(e.data.record.IDProofMaster_ID);
    $("#CodeTemplate").val(e.data.record.CodeTemplate).trigger("change");
    $("#Ref_ID").val(e.data.record.Ref_ID);
    $("#IDProofType").val(e.data.record.IDProofType).trigger("change");
    $("#IDProofData").val(e.data.record.IDProofData);

    $("#IDProof_Created_UserID").val(e.data.record.Created_UserID);
    $("#IDProof_Created_DateTime").val(e.data.record.Created_DateTime);

    var active = e.data.record.IsActive;
    if (active == true) {
        $("#IsActiveIDProof").attr("checked", true);
    }
    else {
        $("#IsActiveIDProof").attr("checked", false);
    }
    $("#IDProofModal").modal("show");
}
function SaveIDProof() {
    var idProofMaster = {
        IDProofMaster_ID: $("#IDProofMaster_ID").val(),
        IDProofType: $("#IDProofType").val(),
        IDProofData: $('#IDProofData').val(),
        Ref_ID: $('#hdCompanyMaster_ID').val(),
        CodeTemplate: null,
        Created_UserID: $('#IDProof_Created_UserID').val(),
        Created_DateTime: $('#IDProof_Created_DateTime').val()
    };
    if ($("#IsActiveIDProof")[0].checked == true) {
        idProofMaster.IsActive = 1;
    }
    if (idProofMaster.IDProofType == null) {
        toastr.error("Please Choose IDProof Type");
        return null;
    }
    else {
        $('#LoadingImg').show();
        $.ajax({ url: "/Home/SaveIDProof", type: "POST", data: { idProofMaster: idProofMaster } })
        .done(function (data) {
            $('#LoadingImg').hide();
            $('#IDProofGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
            $("#IDProofModal").modal("hide");
            if (data.ReturnValue == 1) {
                toastr.success("Saved Successfully");
            }
            else if (data.ReturnValue == 2) {
                toastr.error("ContactNo Already Exist");
            }
        })
        .fail(function () {
            $('#LoadingImg').hide();
            toastr.info('Unable To Save');
        });
    }
}

function RemoveIDProof(e) {
    if (confirm("Are you sure you want to delete?")) {
        $('#LoadingImg').show();
        $.ajax({ url: "/Home/DeleteIDProof", type: "POST", data: { id: e.data.record.IDProofMaster_ID } })
        .done(function (data) {
            $('#LoadingImg').hide();
            if (data == true) {
                toastr.success('Deleted Successfully');
                $('#IDProofGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
            }
        })
        .fail(function (data) {
            $('#LoadingImg').hide();
            toastr.info('Unable To Delete');
        });
    }
}
//IDProof Grid End



//StoarageMaster

function DownloadStorageMaster(e) {
    window.location = "/Home/Download?Id=" + e.data.record.StorageMaster_ID;
}

function AddStorageMaster() {
    $("#StorageMasterDataLabel").css("display", "none");
    $("#StorageMasterData").css("display", "block");
    $("#StorageMaster_ID").val("");
    $("#CodeTemplate").val(0).trigger('change');
    $("#Ref_ID").val("");
    $("#FileName").val("");
    $("#StorageType").val(0).trigger('change');
    $("#StorageMasterData").val("");
    $("#IsActiveStorageMaster").attr("checked", true);
    $("#StorageMasterModal").modal("show");
}
function EditStorageMaster(e) {

    $("#StorageMaster_ID").val(e.data.record.StorageMaster_ID);
    $("#CodeTemplate").val(e.data.record.CodeTemplate).trigger("change");
    $("#Ref_ID").val(e.data.record.Ref_ID);
    $("#StorageType").val(e.data.record.StorageType).trigger("change");
    $("#FileName").val(e.data.record.FileName);
    $("#StorageMasterData").css("display", "none");
    $("#StorageMasterDataLabel").css("display", "block");
    document.getElementById("StorageMasterDataLabel").innerHTML = e.data.record.FileName + e.data.record.Extension;


    var active = e.data.record.IsActive;
    if (active == true) {
        $("#IsActiveStorageMaster").attr("checked", true);
    }
    else {
        $("#IsActiveStorageMaster").attr("checked", false);
    }
    $("#StorageMasterModal").modal("show");
}
function SaveStorageMaster() {
    var storageMaster = {
        StorageMaster_ID: $("#StorageMaster_ID").val(),
        StorageType: $("#StorageType").val(),
        FileName: $('#FileName').val(),
        Ref_ID: $('#hdCompanyMaster_ID').val(),
        CodeTemplate: null
    };

    if ($("#IsActiveStorageMaster")[0].checked == true) {
        storageMaster.IsActive = 1;
    }
    if (storageMaster.StorageType == null) {
        toastr.error("Please Choose Storage Type");
        return null;
    }
    else if ($("#StorageMasterData").val() == null) {
        toastr.error("Please Choose Storage Master Data");
        return null;
    }
    else {

        var fileUpload = $("#StorageMasterData").get(0);
        var files = fileUpload.files;

        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        if (storageMaster.StorageMaster_ID == "")
            storageMaster.StorageMaster_ID = 0;
        if (storageMaster.CodeTemplate == "")
            storageMaster.CodeTemplate = null;

        fileData.append('StorageMaster_ID', storageMaster.StorageMaster_ID);
        fileData.append('StorageType', storageMaster.StorageType);
        fileData.append('CodeTemplate', storageMaster.CodeTemplate);
        fileData.append('Ref_ID', storageMaster.Ref_ID);
        fileData.append('FileName', storageMaster.FileName);
        fileData.append('IsActive', storageMaster.IsActive);

        $('#LoadingImg').show();
        $.ajax({
            url: '/Home/SaveStorageMaster',
            type: "POST",
            cache: false,
            dataType: 'json',
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            success: function (data) {
                $('#LoadingImg').hide();
                $('#StorageMasterGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
                $("#StorageMasterModal").modal("hide");
                if (data == 1) {
                    toastr.success("Saved Successfully");
                }
                else if (data == 2) {
                    toastr.error("ContactNo Already Exist");
                }
            },
            error: function (err) {
                $('#LoadingImg').hide();
                alert(err.statusText);
            }
        });
    }
}

function RemoveStorageMaster(e) {
    if (confirm("Are you sure you want to delete?")) {
        $('#LoadingImg').show();
        $.ajax({ url: "/Home/DeleteStorageMaster", type: "POST", data: { id: e.data.record.StorageMaster_ID } })
        .done(function (data) {
            $('#LoadingImg').hide();
            if (data == true) {
                toastr.success('Deleted Successfully');
                $('#StorageMasterGrid').grid().reload({ Ref_ID: $('#hdCompanyMaster_ID').val(), CodeTempate: null });
            }
        })
        .fail(function (data) {
            $('#LoadingImg').hide();
            toastr.info('Unable To Delete');
        });
    }
}
//Storage Master Grid End






$(document).ready(function () {

    //ComboBox binding for Address Modal

    if ($('#hdCompanyMaster_ID').val() > 0) {

        $.ajax({
            type: "POST",
            url: '/Home/GetAddressType',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#AddressType").select2({
                    data: data
                });
            },
            error: function (data) {
            }
        });

        $.ajax({
            type: "POST",
            url: '/Home/GetState',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#State").select2({
                    data: data
                });
                //$("#State").val($('#hdTableName').val()).trigger("change");
            },
            error: function (data) {
            }
        });

        $.ajax({
            type: "POST",
            url: '/Home/GetCountry',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#Country").select2({
                    data: data
                });
                //$("#Country").val($('#hdTableName').val()).trigger("change");
            },
            error: function (data) {
            }
        });

        $.ajax({
            type: "POST",
            url: '/Home/GetCity',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#City").select2({
                    data: data
                });
                //$("#City").val($('#hdTableName').val()).trigger("change");
            },
            error: function (data) {
            }
        });

        $.ajax({
            type: "POST",
            url: '/Home/GetIDProofType',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#IDProofType").select2({
                    data: data
                });
                //$("#City").val($('#hdTableName').val()).trigger("change");
            },
            error: function (data) {
            }
        });

        $.ajax({
            type: "POST",
            url: '/Home/GetStorageType',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ "searchTerm": null }),
            dataType: "json",
            success: function (data) {
                $("#StorageType").select2({
                    data: data
                });
                //$("#City").val($('#hdTableName').val()).trigger("change");
            },
            error: function (data) {
            }
        });
    }

});