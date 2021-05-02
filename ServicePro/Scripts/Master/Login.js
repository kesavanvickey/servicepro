function Build() {
    var obj = new Object();
    obj.UserId = $('#UserId').val();
    obj.Password = $('#Password').val();
    obj.UserType = $('#UserType').val();
    return obj;
}
function Login() {
    var build = Build();
    jsonBuild = JSON.stringify(build);
    if (!jsonBuild) return null;
    $('#LoadingImg').show();
    $.ajax({
        type: "POST",
        url: '/Login/Validate',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ "jsonBuild": jsonBuild }),
        dataType: "json",
        success: function (data) {
            $('#LoadingImg').hide();
            if (data.ReturnValue == 1) {
                toastr.error('UserName and Password is Not Match!');
            }
            else if (data.ReturnValue == 2) {
                toastr.success('Successfully Signed In');
                window.location.replace("/");
            }
            else if (data.ReturnValue == 3) {
                toastr.error('Sorry! Please Try Again');
            }
        },
        error: function (data) {
            $('#LoadingImg').hide();
            if (data.ReturnValue == 3) {
                toastr.error('Sorry! Please Try Again');
            }
            else{
                toastr.error('Error! Please Refresh & Try Again');
            }
        }
    });
}

function handle(e) {
    if (e.keyCode === 13) {
        Login();
    }
}