$(document).ready(function () {
    Bind_Part();
    $('#user').find("#ID").val(0);
    $("#btn_submit").click(function (e) {
        var formData = new FormData($(user)[0]);
        console.log(formData);
        $.ajax({
            url: "../Master/Create_User",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                Showloader();
            },
            complete: function () {
                Hideloader();
            },
            success: function (Data) {
                debugger;
                if (Data.Result) {
                    Success_Alert(Data.Response);
                    $(user)[0].reset();
                    $('#user').find("#ID").val(0);
                    Bind_Part();

                }
                else {
                    Error_Alert(Data.Response);
                }
            },
            error: function (e1, e2, e3) {
                Error_Alert(Data.Message);
            }

        });
    });

});


function Bind_Part() {
    $.ajax({
        url: "../Master/Get_Data",
        type: "POST",
        dataType: 'json',
        data: {
            User_ID: $("#UserID").val(),
            Type: 1,
        },
        beforeSend: function () {
            Showloader();
        },
        complete: function () {
            Hideloader();
        },
        success: function (Data) {

            var temp = "";
            if (Data.Result) {
                $("#tbl_user").empty();
                $.each(Data.Response, function (i, data) {
                    var editurl = "EditStudentInfo('/Master/Get_User_Record/" + data.ID + "')";
                    var delurl = "DelStudentInfo('/Master/Delete_User_Record/" + data.ID + "')";
                    temp += `<tr><td>` + (i + 1) + `</td><td>` + data.Index_No + `</td><td>` + data.Institute_Name + `</td><td>` + data.User_ID + `</td><td>` + data.Name + `</td><td>` + data.Mobile_No + `</td><td>` + data.Email_ID + `</td><td>` + data.Address + `</td><td>` + data.Password + `</td><td>` + `<input type = "button" value = "Edit" onclick = ` + editurl + ` class="btn btn-warning"/>` + `</td><td>` + `<input type = "button" value = "Delete" onclick="` + delurl + `" class="btn btn-danger"/>` + `</td></tr>`;

                });
                $("#tbl_user").append(temp);

            }

        },
        error: function (e1, e2, e3) {
            alert(e1);
        }
    });
}

function EditStudentInfo(url) {
    debugger
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            if (data.Result) {
                $('#User_ID').val(data.Response.User_ID);
                $('#Mobile_No').val(data.Response.Mobile_No);
                $('#Email_ID').val(data.Response.Email_ID);
                $('#Index_No').val(data.Response.Index_No);
                $('#Address').val(data.Response.Address);
                $('#Password').val(data.Response.Password);
                $('#Institute_Name').val(data.Response.Institute_Name);
                $('#Name').val(data.Response.Name);
                $('#user').find("#ID").val(data.Response.ID);

            }
        }
    });
}

function DelStudentInfo(url) {
    debugger
    if (confirm('Are you sure want to delete this record ?') == true) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (data) {
                if (data.Result) {
                    Bind_Part();
                    $(user)[0].reset();
                    Success_Alert(data.Response);
                }
                else {
                    Error_Alert(data.Response);
                }
            }
        });
    }
}