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
            url: "addQuestions",
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
            Type: 3,
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
                    var editurl = "EditStudentInfo('/Question/Get_Question_Record/" + data.ID + "')";
                    var delurl = "DelStudentInfo('/Question/Delete_Question_Record/" + data.ID + "')";
                    temp += `<tr><td>` + (i + 1) + `</td><td>` + data.Class + `</td><td>` + data.Subject + `</td><td>` + data.Question_Type + `</td><td>` + data.Question + `</td><td>` + data.Option1 + `</td><td>` + data.Option2 + `</td><td>` + data.Option3 + `</td><td>` + data.Option4 + `</td><td>` + data.Option5 + `</td><td>` + data.Option6 + `</td><td>` + data.Marks + `</td><td>` + `<input type = "button" value = "Edit" onclick = ` + editurl + ` class="btn btn-warning"/>` + `</td><td>` + `<input type = "button" value = "Delete" onclick="` + delurl + `" class="btn btn-danger"/>` + `</td></tr>`;

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

    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {
            if (data.Result) {
                $('#Class').val(data.Response.Class);
                $('#Question_Type').val(data.Response.Question_Type);
                $('#Question').val(data.Response.Question);
                $('#Option1').val(data.Response.Option1);
                $('#Option2').val(data.Response.Option2);
                $('#Option3').val(data.Response.Option3);
                $('#Option4').val(data.Response.Option4);
                $('#Option5').val(data.Response.Option5);
                $('#Option6').val(data.Response.Option6);
                $('#Marks').val(data.Response.Marks);
                $('#user').find("#ID").val(data.Response.ID);
                setSub(data.Response.Subject);
            }
        }
    });

}

function setSub(sub) {
    
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
            $('#Subject').val(sub);
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}

function DelStudentInfo(url) {
    debugger
    if (confirm('Are you sure want to delete this Question!') == true) {
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