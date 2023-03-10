$(document).ready(function () {
    FirstQ();
    var index = 1;
    var distance = "";
    var time = "";
    var saved_countdown = localStorage.getItem('saved_countdown');

    $.ajax({
        type: "POST",
        url: "../IT_Exam/GetTime",
        dataType: "json",
        success: function (data) {
            if (data.Result) {

                var JsonDate = data.Response;
                document.getElementById("demo").innerHTML = JsonDate.Hours + "h " + JsonDate.Minutes + "m " + JsonDate.Seconds + "s ";
                var JavaScriptDate = new Date(parseInt(data.End_T.substr(6)));
                var dateObject = new Date(JavaScriptDate);
                time = dateObject.getTime();
                localStorage.setItem('saved_countdown', time);
                saved_countdown = localStorage.getItem('saved_countdown');
            }
            else {
                Error_Alert("Time is Up! All the Best for Result!");
                localStorage.removeItem('saved_countdown');
                window.location = "../IT_Exam/Exam_Login";
            }

        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            alert(msg);
        },

    });

    if ($(window).width() <= 1000) {
        $('[data-toggle=collapse]').prop('disabled', false);
    }
    else {
        $('[data-toggle=collapse]').prop('disabled', true);
    }

    $(window).resize(function () {
        if ($(window).width() <= 1000) {
            $('[data-toggle=collapse]').prop('disabled', false);
        }
        else {
            $('[data-toggle=collapse]').prop('disabled', true);
        }

    });

    $(document).keydown(function (evtobj) {

        //if (evtobj.keyCode === 16) {
        //    return false;
        //}
        //if (evtobj.keyCode === 17) {
        //    return false;
        //}
        //if (evtobj.keyCode === 18) {
        //    return false;
        //}
        //if (evtobj.keyCode === 46) {
        //    return false;
        //}
        //if (evtobj.keyCode === 9) {
        //    return false;
        //}
        //if (evtobj.keyCode === 116) {
        //    return false;
        //}

    });

    //document.addEventListener('contextmenu', event => event.preventDefault());

    $('.body').on("cut copy paste", function (e) {
        e.preventDefault();
    });

    $('.form-control').on("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).mouseleave(function () {
        //alert("????");
    });

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    setInterval(function () {
        if (window.navigator.onLine) {
            document.getElementById("ics").innerHTML = "Online";
        }
        else {
            document.getElementById("ics").innerHTML = "Offline";
        }
    }, 1000);


    setInterval(function () {

        $.ajax({
            type: "POST",
            url: "../IT_Exam/GetTime",
            dataType: "json",
            success: function (data) {
                if (data.Result) {
                    var JsonDate = data.Response;
                    document.getElementById("demo").innerHTML = JsonDate.Hours + "h " + JsonDate.Minutes + "m " + JsonDate.Seconds + "s ";
                    var JavaScriptDate = new Date(parseInt(data.End_T.substr(6)));
                    var dateObject = new Date(JavaScriptDate);
                    var daate = dateObject.getTime();
                    localStorage.setItem('saved_countdown', daate);
                }
                else {
                    Error_Alert("Time is Up! All the Best for Result!");
                    localStorage.removeItem('saved_countdown');
                    window.location = "../IT_Exam/Exam_Login";
                }

            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            },

        });
    }, 120000);



    function setTimer() {

        var now = new Date().getTime();

        if (saved_countdown === null) {
            localStorage.setItem('saved_countdown', time)
            saved_countdown = localStorage.getItem('saved_countdown');
        }

        distance = saved_countdown - now;

        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        document.getElementById("demo").innerHTML = hours + "h " + minutes + "m " + seconds + "s ";

        if (distance < 0) {
            clearInterval(x);
            localStorage.removeItem('saved_countdown');
            document.getElementById("demo").innerHTML = "EXPIRED";
        }
    }
    var x = setInterval(setTimer, 1000);


    $("#end_exm").click(function () {

        clearInterval(x);
        localStorage.removeItem('saved_countdown');
        window.location = "../IT_Exam/Exam_Login";
    });
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function disableselect(e) {
        return false;
    }

    function reEnable() {
        return true;
    }

    document.onselectstart = new Function("return false");

    if (window.sidebar) {
        document.onmousedown = disableselect;
        document.onclick = reEnable;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    (function () {
        function checkTime(i) {
            return (i < 10) ? "0" + i : i;
        }

        function startTime() {
            var today = new Date(),
                h = checkTime(today.getHours()),
                hr = h - 12;
            m = checkTime(today.getMinutes()),
                s = checkTime(today.getSeconds());
            if (hr > 0) {
                document.getElementById('time').innerHTML = hr + ":" + m + " PM ";
            }
            else {
                document.getElementById('time').innerHTML = h + ":" + m + " AM ";
            }

            t = setTimeout(function () {
                startTime()
            }, 1000);
        }
        startTime();
    })();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function FirstQ() {
        $('#prev').prop('disabled', true);
        $.ajax({
            type: "POST",
            url: "../IT_Exam/Get_Question",
            dataType: "json",
            success: function (data) {
                if (data.Result) {
                    var temp = "";
                    $("#q3_body").empty();

                    var list = data.Response.questions[0];

                    var ans_list = data.Response.tbl_Q3[0];

                    if (ans_list == null) {
                        temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + 1 + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                        $("#q3_body").append(temp);
                        MCQ_Q3();
                    }
                    else {

                        temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + 1 + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option1) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `" checked>`
                        }
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option1) {
                            temp += `<input type="checkbox" class="radio_chk" id = ` + list.ID + ` name = ` + list.Option1 + ` value="` + list.Option1 + `">`
                        }
                        temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option2) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `" checked>`
                        }
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option2) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">`
                        }

                        temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option3) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `" checked>`
                        }
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option3) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">`
                        }

                        temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option4) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `" checked>`
                        }
                        if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option4) {
                            temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">`
                        }

                        temp += ` </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                        $("#q3_body").append(temp);
                        MCQ_Q3();
                    }
                }

            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            },

        });
    }

    $("#next").click(function () {
        $('#prev').prop('disabled', false);
        $.ajax({
            type: "POST",
            url: "../IT_Exam/Get_Question",
            dataType: "json",
            success: function (data) {
                if (data.Result) {
                    var temp = "";
                    $("#q3_body").empty();
                    var len = data.Response.questions.length;
                    var list = data.Response.questions[index];
                    var ans_list = data.Response.tbl_Q3[index];
                    debugger
                    if (index == len-1) {
                        $('#next').prop('disabled', true);
                    }
                    if (list != undefined) {

                        if (ans_list == null) {
                            temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + ++index + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                            $("#q3_body").append(temp);
                            MCQ_Q3();
                        }
                        else {
                          
                            temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + ++index + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option1) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option1) {
                                temp += `<input type="checkbox" class="radio_chk" id = ` + list.ID + ` name = ` + list.Option1 + ` value="` + list.Option1 + `">`
                            }
                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option2) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option2) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">`
                            }

                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option3) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option3) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">`
                            }

                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option4) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option4) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">`
                            }

                            temp += ` </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                            $("#q3_body").append(temp);
                            MCQ_Q3();
                        }
                    }
                    else {
                        index = 1;
                        FirstQ();
                    }
                }

            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            },

        });

    });

    $("#prev").click(function () {
        --index;
        $('#next').prop('disabled', false);
        if (index == 1) {
            $('#prev').prop('disabled', true);
        }
      
        $.ajax({
            type: "POST",
            url: "../IT_Exam/Get_Question",
            dataType: "json",
            success: function (data) {
                if (data.Result) {
                    var temp = "";
                    $("#q3_body").empty();
                  
                    var list = data.Response.questions[index-1];
                    var ans_list = data.Response.tbl_Q3[index-1];
                    if (list != undefined) {
                        if (ans_list == null) {
                            
                            temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + index + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                  <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                <input type="checkbox" class="radio_chk" id=`+ list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">
                                                                            </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                            $("#q3_body").append(temp);
                            MCQ_Q3();
                        }
                        else {
                           
                            
                            temp += `<tr> <td style="width:100px;"><input type="hidden" id="hd_id" name="hd_id" value=` + list.ID + `><b id="sqno">Q.` + index + `</b>
                                                                        <input type="hidden" id="chk_name1" name="chk_name1" value=` + list.Option1 + `>
                                                                        <input type="hidden" id="chk_name2" name="chk_name2" value=` + list.Option2 + `>
                                                                        <input type="hidden" id="chk_name3" name="chk_name3" value=` + list.Option3 + `>
                                                                        <input type="hidden" id="chk_name4" name="chk_name4" value=` + list.Option4 + `></td>
                                                                        <td> <label><b>Question :</b></label>
                                                                        <p>`+ list.Question + `</p>
                                                                        <label><b>Options :</b></label>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option1) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option1 + ` value="` + list.Option1 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option1) {
                                temp += `<input type="checkbox" class="radio_chk" id = ` + list.ID + ` name = ` + list.Option1 + ` value="` + list.Option1 + `">`
                            }
                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>1) ` + list.Option1 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option2) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option2) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option2 + ` value="` + list.Option2 + `">`
                            }

                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>2) ` + list.Option2 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option3) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option3) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option3 + ` value="` + list.Option3 + `">`
                            }

                            temp += `</div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>3) ` + list.Option3 + ` </p>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-xs-3" style="padding-left:1em">`
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer == list.Option4) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `" checked>`
                            }
                            if (list.Question_ID == ans_list.Question_ID && ans_list.Answer != list.Option4) {
                                temp += `<input type="checkbox" class="radio_chk" id=` + list.ID + ` name=` + list.Option4 + ` value="` + list.Option4 + `">`
                            }

                            temp += ` </div>
                                                                            <div class="col-xs-3" style="padding-left:1em">
                                                                                    <p>4) ` + list.Option4 + ` </p>
                                                                            </div>
                                                                        </div></td></tr>`;

                            $("#q3_body").append(temp);
                            MCQ_Q3();
                        }
                    }
                    else {
                        index = 1;
                        FirstQ();
                    }
                }
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            },

        });
    });

    $('#mcq_q3').on('change', 'input', function () {
        MCQ_Q3();
    });

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function MCQ_Q3() {
        $("#mcq_q3").each(function () {

            var row = $(this);

            var ID = row.find("input[id=hd_id]").val();
            var max = 1;
            if ($('input[id=' + ID + ']:checked').length == max) {
                $('input[id=' + ID + ']').attr('disabled', 'disabled');
                $('input[id=' + ID + ']:checked').removeAttr('disabled');
            } else {
                $('input[id=' + ID + ']').removeAttr('disabled');
            }
        });
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $("#btn_q3_submit").click(function (e) {

        var model = {};

        var Question1 = new Array();
        $("#mcq_q3").each(function () {
            debugger
            var row = $(this);
            var Question = {};

            var op1 = row.find("input[id='chk_name1']").val();
            var op2 = row.find("input[id='chk_name2']").val();
            var op3 = row.find("input[id='chk_name3']").val();
            var op4 = row.find("input[id='chk_name4']").val();

            Question.ID = row.find("input[id='hd_id']").val();
            Question.QNO = row.find("#sqno").html();

            if (row.find('input[name=' + op1 + ']').is(':checked')) {
                if (Question.Ans == null) {
                    Question.Ans = row.find("input[name=" + op1 + ']:checked').val();

                }
            }
            if (row.find('input[name=' + op2 + ']').is(':checked')) {
                if (Question.Ans == null) {
                    Question.Ans = row.find("input[name=" + op2 + ']:checked').val();

                }
            }
            if (row.find('input[name=' + op3 + ']').is(':checked')) {
                if (Question.Ans == null) {
                    Question.Ans = row.find("input[name=" + op3 + ']:checked').val();

                }
            }
            if (row.find('input[name=' + op4 + ']').is(':checked')) {
                if (Question.Ans == null) {
                    Question.Ans = row.find("input[name=" + op4 + ']:checked').val();

                }
            }

            Question1.push(Question);
        });

        model.Question1_List = Question1;
        model.Index_No = $("#Index_No").html();
        model.Paper_No = $("#Paper_No").html();
        model.Seat_No = $("#Seat_No").html();
        model.Sub_Code = $("#Sub_Code").html();

        if (confirm('Are you sure, you want submit Question3!') == true) {
            $.ajax({
                type: "POST",
                url: "../IT_Exam/Submit_Question3",
                data: JSON.stringify(model),
                contentType: "application/json;",
                dataType: "json",
                success: function (data) {
                    if (data.Result) {
                        Success_Alert_Reload(data.Response);
                    }
                    else {
                        Error_Alert(data.Response);
                    }

                },
                error: function (jqXHR, exception) {
                    var msg = '';
                    if (jqXHR.status === 0) {
                        msg = 'Not connect.\n Verify Network.';
                    } else if (jqXHR.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (jqXHR.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else if (exception === 'parsererror') {
                        msg = 'Requested JSON parse failed.';
                    } else if (exception === 'timeout') {
                        msg = 'Time out error.';
                    } else if (exception === 'abort') {
                        msg = 'Ajax request aborted.';
                    } else {
                        msg = 'Uncaught Error.\n' + jqXHR.responseText;
                    }
                    alert(msg);
                },

            });
        }
    });

});