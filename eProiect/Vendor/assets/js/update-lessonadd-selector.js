
$(document).ready(function () {
   
    $(".plus-button").click(function () {
        var positionTime = $(this).data("positionTime");
        var dataWeekdayNr = $(this).data("positionWeekdayNr");

        $("#schedule-container").css("display", "none");
        $("#addlesson-container").css("display", "block");

        var weekdayName;
        switch (dataWeekdayNr) {
            case 0: weekdayName = "Luni"; break;
            case 1: weekdayName = "Marti"; break;
            case 2: weekdayName = "Miercuri"; break;
            case 3: weekdayName = "Joi"; break;
            case 4: weekdayName = "Vineri"; break;
            case 5: weekdayName = "Sambata"; break;
        }
        $("#selectedTime").text(positionTime + ' ' + weekdayName);

        $('#hour').val($(this).data("positionHours"));
        $('#minute').val($(this).data("positionMinutes"));
        $('#day').val(dataWeekdayNr);
 
        loadDisciplineSelect();
        
    });

    $("#disciplineSelector").click(function () {
        console.error('thisval:', $(this).val());
        if ($(this).val() != null) {
            $.ajax({
                url: "/Home/GetLoggedUserDisciplineTypes",
                method: "GET",
                data: { disciplineId:$(this).val() },
                success: function (classTypes) {
                    updateDisciplineTypeSelect(classTypes);
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }
            })
        }
    })

    $("#yearSelector").click(function (event) {
        var selectedYear = $(event.target).val();
        if (selectedYear != null) {
            $.ajax({
                url: "/Home/GetOptionsByYear",
                method: "POST",
                data: { year: selectedYear },
                success: function (data) {
                    // Update the second select options based on the response (data)
                    updateGroupSelect(data);
                    updateGroupMultiSelect(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }
            });
        }
    });

    // span clicks
    // frequency clicks
    // listen to both:
    $(".class-param-radio").click( function(){
        loadFreeClassrooms()
    });

    $("#typeSelector").click(function (event) {
        var selectedType = $(event.target).val();

        if (selectedType != undefined && selectedType != null) {
            //console.error(selectedType)

            if (selectedType === "1" /*for CURS*/) { 
                console.error("toggled multi (curs)")
                $("#group-select-row").toggleClass('closed');
                $("#multi-group-select").removeClass('closed');
            } else{
                console.error("toggled single")
                $("#group-select-row").removeClass('closed');
                if (!$("#multi-group-select").hasClass('closed')) {
                    $("#multi-group-select").toggleClass('closed');
                }
            }
        }
    });

    $('#multi-group-select').click(function (event) {
        if (event.target != null && event.target != undefined) {
            if (event.target.id === 'option') {
                var targetDiv = $("#selected-options");
                if ($(event.target).parent().attr("id") === "dropdown-options") {
                    $(event.target).toggleClass('selected')
                    $(event.target).removeClass('listed')
                    $(event.target).appendTo(targetDiv).append('<i class="ti-close"></i>');
                } else {
                    $(event.target).removeClass('selected')
                    $(event.target).toggleClass('listed')
                    $(event.target).appendTo($("#dropdown-options")).find("i").remove();
                }
            }
        }
    });

    $("#dropdown-options span").click(function () {
        var targetDiv = $("#selected-options");
        if ($(this).parent().attr("id") === "dropdown-options") {
            $(this).toggleClass('selected')
            $(this).removeClass('listed')
            $(this).appendTo(targetDiv).append('<i class="ti-close"></i>');
        } else {
            $(this).removeClass('selected')
            $(this).toggleClass('listed')
            $(this).appendTo($("#dropdown-options")).find("i").remove();
        }
    });

    $("#toggle-dropdown").click(function () {
        var dropdownMenu = $("#group-menu");
        if (dropdownMenu.hasClass("closed")) {
            dropdownMenu.removeClass("closed")
        } else
            dropdownMenu.toggleClass("closed")
    });

    $("#submit-button-addlesson").click(function () {

        //old vars
        /*var sDiscipline = $('#disciplineSelector').find(':selected').val();
        var sType = $('#typeSelector').find(':selected').val();
        var sYear = $('#yearSelector').find(':selected').val();
        var sClassroom = $('#classroomSelector').find(':selected').val();

        //spans list, take val()

        var sFequency = $("input[name='frequency']:checked").val();
        var sSpan = $("input[name='length']:checked").val();
        var sHour = $('#hour').val();
        var sMinute = $('#minute').val();
        var sDayNr = $('#day').val();*/

        var sGroupsId = $('#selected-options').find('.selected');
        var sGroup = $('#groupSelector').find(':selected').val();
        

        //compose json data (no groups)
        var jsonClassData = {
            UserDisciplineId: $('#disciplineSelector').find(':selected').val(),
            TypeId: $('#typeSelector').find(':selected').val(),
            Year: $('#yearSelector').find(':selected').val(),
            ClassroomId: $('#classroomSelector').find(':selected').val(),
            Frequency: $("input[name='frequency']:checked").val(),
            Span: $("input[name='length']:checked").val(),
            Hours: $('#hour').val(),
            Minutes: $('#minute').val(),
            Day: $('#day').val(),
        }
        
        
        //grouplist id's to json
        var jsonGroups = [];
        if (sGroupsId.length != 0) {
            sGroupsId.each(function (index) {
                jsonGroups[index] = +$(this).val();
            })
        } else /*just one id for other classes types(!curs)*/{
            jsonGroups[0] = +sGroup;
        }


        /*console.error(jsonClassData)
        console.error("groups:", jsonGroups)*/

        $.ajax({
            url: "/Home/AddNewClass",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({ composedData: jsonClassData, groupIds: jsonGroups }),
            success: function (responses) {
                //notify user
                console.error(responses)
                $.each(responses, function (index, response) {
                    if (response && typeof response === 'object') {
                        if (response["Status"] == false) {
                            $('#postStatus').empty()
                            $('#postStatus').append(
                                $('<div></div>').text("[" + (index + 1) + "]" + response["ActionStatusMsg"])
                            )
                        } else {
                            //notify succes! exit page.
                        }
                    } 
                    /*var text = $('#postStatus').text();*/
                    //+ " resp[", index, "]", response.Status + " " + response.ActionStatusMsg);
                })
            },

            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.error("Full jqXHR object:", jqXHR);
            }
        });



        //debug console error print
        if (false) {
            console.clear();
            console.error("discipline ID:", sDiscipline);
            console.error("type INT:", sType);
            console.error("year INT:", sYear);

            if (sGroupsId.length != 0) {
                console.error('grouplist:')
                sGroupsId.each(function (index) {
                    console.error("at" + index + ": ", $(this).val());
                })
            } else {
                console.error("group ID:", sGroup);
            }

            console.error("classroom ID:", sClassroom)
            console.error("freq:", sFequency)
            console.error("span:", sSpan)
            console.error('h:' + sHour + ' m:' + sMinute + ' dnr:' + sDayNr);
        }

    });

    $("#floorSelector").click(function () {
        var selectorValue = $(this).val();
        if (selectorValue != null && selectorValue != -1) {

            loadFreeClassrooms()
        }
    });
});


function loadFreeClassrooms() {
    jsonData = {
        StartHour: $('#hour').val(),
        StartMinute: $('#minute').val(),
        Span: $("input[name='length']:checked").val(),
        Floor: $("#floorSelector").find(':selected').val(),
        WeekdayId: $('#day').val(),
        Frequency: $("input[name='frequency']:checked").val()
    }
    $.ajax({
        url: "/Home/GetFreeClassroomsByFloor",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({ requestData: jsonData }),
        success: function (data) {
            console.error(data);
            updateClassroomsSelect(data)
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function loadDisciplineSelect() {
    $.ajax({
        url: "/Home/GetLoggedUserDisciplines",
        method: "GET",
        success: function (data) {
            updateDisciplineSelect(data);
            /*updateDisciplineTypeSelect(data);*/
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function updateClassroomsSelect(data) {
    $('#classroomSelector').empty();
    $('#classroomSelector').append(
        $('<option disabled selected></option>').text("cabinetul").val(-1)
    );

    
    $.each(data, function (index, item) {
        console.error("id:"+item.Id+" name:"+item.ClassroomName)
        $('#classroomSelector').append(
            $('<option></option>').val(item.Id).text(item.ClassroomName)
        );
    });

}

function updateDisciplineSelect(data) {
    $('#disciplineSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("disciplina").val(-1);
    $('#disciplineSelector').append(defaultOption);
    $.each(data, function (index, item) { //////////////////////////////DID-STUFF//////DID-STUFF//////DID-STUFF//////DID-STUFF//////DID-STUFF///
        var option = $('<option></option>').val(item.Id).text(item.Name);
        $('#disciplineSelector').append(option);
    });
}

function updateDisciplineTypeSelect(data) {

    $('#typeSelector').empty();
    var defaultOption = $('<option disabled selected></option>').text("tipul").val(-1);

    $('#typeSelector').append(defaultOption);
     $.each(data, function (index, item) {
         var option = $('<option></option>').val(item.Id).text(item.TypeName);
         $('#typeSelector').append(option);
    });
}

function updateGroupMultiSelect(data) {


    $('#selected-options').empty();
    $('#dropdown-options').empty();
    $.each(data, function (index, item) {
        var groupSpan = $('<span>').attr('id', 'option').addClass('option listed')/*.attr("value", item.Id)*/.text(item.Name).val(item.Id);
        $('#dropdown-options').append(groupSpan);
    });
}

function updateGroupSelect(data) {
    // Clear existing options
    $('#groupSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("grupa").val(-1);
    $("#groupSelector").append(defaultOption)

    // Add options based on the received data
    $.each(data, function (index, item) {
        var option = $('<option></option>').val(item.Id).text(item.Name);

        $('#groupSelector').append(option);
    });
}

