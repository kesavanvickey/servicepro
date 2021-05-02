function Build() {
    var obj = new Object();
    obj.CompanyName = $('#CompanyName').val();
    obj.CompanyType = $('#CompanyType').val();
    obj.TinNo = $('#TinNo').val();
    obj.UserName = $('#UserName').val();
    obj.Password = $('#Password').val();
    obj.Recovery_Mobile = $('#Mobile').val();
    obj.Recovery_Email = $('#Email').val();
    obj.Recovery_Question = $('#Question').val();
    obj.Recovery_Answer = $('#Answer').val();
    obj.ActivationMaster_Key = $('#ProductKey').val();
    return obj;
}
function Save() {
    var build = Build();
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
                toastr.error('Sorry! Product Key Already Used :(');
            }
            else if (data.ReturnValue == 2) {
                toastr.success('Company Registered Successfully :)');
                window.location.replace("/");
            }
            else if(data.ReturnValue == 5)
            {
                toastr.info('Please Activate your Product Key First');
            }
            else if (data.ReturnValue == 6) {
                toastr.error('Product Key Is Invalid');
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
function Delete() {
    var build = Build();
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
                toastr.success('Deleted Successfully');
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