function Build()
{
    var obj = new Object();
    obj.ActivationMaster_Key = $('#ProductKey').val();
    obj.InstalledBy = $('#InstalledBy').val();
    obj.CompanyName = $('#CompanyName').val();
    return obj;
}
function Save()
{
    var build = Build();
    jsonBuild = JSON.stringify(build);
    if (!jsonBuild) return null;
    $('#LoadingImg').show();
    $.ajax({
    type: "POST",
    url: '/Activation/Save',
    contentType: "application/json; charset=utf-8",
    data: JSON.stringify({"jsonBuild": jsonBuild}),
    dataType: "json",
    success: function (data)
    {
        $('#LoadingImg').hide();
        if (data.ReturnValue == 1)
        {
            toastr.error('Sorry! Product Key Already Used');
        }
        else if (data.ReturnValue == 2)
        {
            alert('Product Key Activated Sucessfully');
            window.close();
        }
        else if (data.ReturnValue == 5) {
            toastr.error('Product Key is Invalid');
        }
    },
    error: function (data)
    {
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