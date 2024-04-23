
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

    $("#floorSelector").click(function (event) {
        var selectedFloor = $(event.target).val();

        if (selectedFloor != null) {
            $.ajax({
                url: "/Home/GetFreeClassroomsByFloor",
                method: "POST",
                data: { floor: selectedFloor },
                success: function (data) {
                    updateClassroomsSelect(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }
            });
        }
    });

    $("#typeSelector").click(function (event) {
        var selectedType = $(event.target).val();

        if (selectedType != undefined && selectedType != null) {
            //console.error(selectedType)

            if (selectedType === "1" /*for CURS*/) { 
                //console.error("toggled multi (curs)")
                $("#group-select-row").toggleClass('closed');
                $("#multi-group-select").removeClass('closed');
            } else{
                //console.error("toggled single")
                $("#group-select-row").removeClass('closed');
                $("#multi-group-select").toggleClass('closed');
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
        var sGroup = $('#groupSelector').find(':selected').val();
        var sClassroom = $('#classroomSelector').find(':selected').val();

        //spans list, take val()
        var sGroupsId = $('#selected-options').find('.selected');

        var sFequency = $("input[name='frequency']:checked").val();
        var sSpan = $("input[name='length']:checked").val();
        var sHour = $('#hour').val();
        var sMinute = $('#minute').val();
        var sDayNr = $('#day').val();*/

        90

        //compose json data (no groups)
        var jsonClassData = {
            discipline: $('#disciplineSelector').find(':selected').val(),
            type: $('#typeSelector').find(':selected').val(),
            year: $('#yearSelector').find(':selected').val(),
            classroom: $('#classroomSelector').find(':selected').val(),
            frequency: $("input[name='frequency']:checked").val(),
            span: $("input[name='length']:checked").val(),
            hours: $('#hour').val(),
            minutes: $('#minute').val(),
            daynr: $('#day').val(),
        }
        
        //grouplist id's to json
        var jsonGroups = {};
        for (var i = 0; i < sGroupsId.length; i++) {
            jsonGroups[i] = sGroupsId[i];
        }

        if (sGroupsId.length != 0) {
            for (var i = 0; i < sGroupsId.length; i++) {
                jsonGroups[i] = sGroupsId[i];
            }
        } else /*just one id for other classes types(!curs)*/{
            jsonGroups[1] = sGroup;
        }



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
});



function loadDisciplineSelect() {
    $.ajax({
        url: "/Home/GetLoggedUserDisciplines",
        method: "GET",
        success: function (data) {
            updateDisciplineSelect(data);
            updateDisciplineTypeSelect(data);
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
        $('<option disabled selected></option>').text("cabinetul")
    );

    $.each(data, function (index, item) {
        $('#classroomSelector').append(
            $('<option></option>').val(item.Id).text(item.ClassroomName)
        );
    });

}

function updateDisciplineSelect(data) {
    $('#disciplineSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("disciplina").val(-1);
    $('#disciplineSelector').append(defaultOption);
    $.each(data, function (index, item) {
        var option = $('<option></option>').val(item.Discipline.Id).text(item.Discipline.Name);
        $('#disciplineSelector').append(option);
    });
}

function updateDisciplineTypeSelect(data) {

    $('#typeSelector').empty();
    var defaultOption = $('<option disabled selected></option>').text("tipul").val(-1);

    $('#typeSelector').append(defaultOption);
     $.each(data, function (index, item) {
         var option = $('<option></option>').val(item.ClassTypeId).text(item.Type.TypeName);
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
        var option = $('<option></option>').val(item["Year"]).text(item["Name"]);

        $('#groupSelector').append(option);
    });
}

