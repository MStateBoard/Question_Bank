

$(document).ready(function () {
    Bind_Part();
    $('#user').find("#ID").val(0);
    $("#btn_submit").click(function (e) {
        debugger
        var formData = new FormData($(user)[0]);
        console.log(formData);
        $.ajax({
            url: "Create_Class",
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
        url: "Get_Data",
        type: "POST",
        dataType: 'json',
        data: {
            User_ID: $("#UserID").val(),
            Type: 2,
        },
        beforeSend: function () {
            Showloader();
        },
        complete: function () {
            Hideloader();
        },
        success: function (Data) {
            debugger
            var temp = "";
            if (Data.Result) {
                $("#tbl_user").empty();
                $.each(Data.Response, function (i, data) {
                    var editurl = "EditStudentInfo('/Master/Get_Class_Record/" + data.ID + "')";
                    var delurl = "DelStudentInfo('/Master/Delete_Class_Record/" + data.ID + "')";
                    temp += `<tr><td>` + (i + 1) + `</td><td>` + data.User_ID + `</td><td>` + data.Class + `</td><td>` + data.Subject + `</td><td>` + data.Question_Type + `</td><td>` + data.Question + `</td><td>` + data.Option1 + `</td><td>` + data.Option2 + `</td><td>` + data.Option3 + `</td><td>` + data.Option4 + `</td><td>` + data.Option5 + `</td><td>` + data.Option6 + `</td><td>` + data.Answer1 + `</td><td>` + data.Answer2 + `</td><td>` + data.Answer3 + `</td><td>` + `<input type = "button" value = "Edit" onclick = ` + editurl + ` class="btn btn-warning"/>` + `</td><td>` + `<input type = "button" value = "Delete" onclick="` + delurl + `" class="btn btn-danger"/>` + `</td></tr>`;

                });
                $("#tbl_user").append(temp);
            }
            else {
                Error_Alert(Data.Response);
            }

        },
        error: function (e1, e2, e3) {
            Error_Alert(Data.Message);
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
                $('#Class').val(data.Response.Class);
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