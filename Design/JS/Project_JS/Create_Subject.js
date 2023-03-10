$(document).ready(function () {
    Bind_Part();

   
    $("#Class").change(function () {
        debugger
        $("#Subject").empty();
        $.ajax({
            type: 'POST',
            url: '../Question/Load_Subject',
            dataType: 'json',
            data: {
                Class: $("#Class").val()
            },
            success: function (Data) {
                for (var item in Data) {
                    $("#Subject").append('<option value="' + Data[item].Value + '">' + Data[item].Text + '</option>');
                }

            },
            error: function (ex) {
                alert('Failed.' + ex);
            }
        });
    });


    $('#user').find("#ID").val(0);
    $("#btn_submit").click(function (e) {
        debugger
        var formData = new FormData($(user)[0]);
        console.log(formData);
        $.ajax({
            url: "Create_Subject",
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

            var temp = "";
            if (Data.Result) {
                $("#tbl_user").empty();
                $.each(Data.Response, function (i, data) {
                    var editurl = "EditStudentInfo('/Master/Get_Class_Record/" + data.ID + "')";
                    var delurl = "DelStudentInfo('/Master/Delete_Subject_Record/" + data.ID + "')";
                    temp += `<tr><td>` + (i + 1) + `</td><td>` + data.Class + `</td><td>` + data.Subject + `</td><td>` + data.Question_Type + `</td><td>` + data.Question + `</td><td>` + data.Option1 + `</td><td>` + data.Option2 + `</td><td>` + data.Option3 + `</td><td>` + data.Option4 + `</td><td>` + data.Option5 + `</td><td>` + data.Option6 + `</td><td>` + data.Answer1 + `</td><td>` + data.Answer2 + `</td><td>` + data.Answer3 + `</td><td>` + `<input type = "button" value = "Edit" onclick = ` + editurl + ` class="btn btn-warning"/>` + `</td><td>` + `<input type = "button" value = "Delete" onclick="` + delurl + `" class="btn btn-danger"/>` + `</td></tr>`;

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

    $.when(
    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            if (data.Result) {
                $('#Subject').val(data.Response.Subject);
                $('#Class').val(data.Response.Class);
                $('#user').find("#ID").val(data.Response.ID);

            }
        }
    }),
    ).done(function (){
        $.ajax({
            type: 'POST',
            url: '../Question/Load_Subject',
            dataType: 'json',
            data: {
                Class: $("#Class").val()
            },
            success: function (Data) {
                $("#Subject").empty();
                for (var item in Data) {
                    $("#Subject").append('<option value="' + Data[item].Value + '">' + Data[item].Text + '</option>');
                }

            },
            error: function (ex) {
                alert('Failed.' + ex);
            }
        });
    });
   
}

function DelStudentInfo(url) {
    debugger
    if (confirm('Are you sure want to delete this subject! If you want to delete entire record please go to Create Class Page!') == true) {
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