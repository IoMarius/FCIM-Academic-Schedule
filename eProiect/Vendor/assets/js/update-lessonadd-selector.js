
$(document).ready(function () {
   
    $('#enableTableEditButton').click(function () {
        //hide undhide this one
        $('.edit-lesson-button').toggleClass('closed');
        $('#editIconTable').toggleClass('closed');
        $('#stopEditIconTable').toggleClass('closed');
        $('.edit-lesson-row').toggleClass('margin-bottom-min-23');
        //toggle activated not activated styles
    });

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

        $("#constructor-weekday").text(weekdayName);
        $("#constructor-time").text(positionTime).attr("data-start-time", positionTime);
        updateConstructorTime(positionTime)

        $('#hour').val($(this).data("positionHours"));
        $('#minute').val($(this).data("positionMinutes"));
        $('#day').val(dataWeekdayNr);
 
        loadDisciplineSelect();
        
    });

    $("#disciplineSelector").click(function () {
        if ($(this).val() != null) {

            //update constructor
            var disciplineShortname = $('#disciplineSelector').find(':selected').data("shortname")
            $('#constructorDisName').text(disciplineShortname)

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
    $(".add-lesson-radio-btns").click(function () {
        loadFreeClassrooms()

        //update constructor
        //frequency
        var checkedFrequency = $("input[name='frequency']:checked").data('frequency')
        if (checkedFrequency == 'par') {
            $('#constructorFrequency').toggleClass('timetable-label-even margin-left-3px').text('par')
        } else if (checkedFrequency == 'impar') {
            $('#constructorFrequency').toggleClass('timetable-label-odd margin-left-3px').text('impar')
        } else {
            $('#constructorFrequency').removeClass().text('');
        }




        //timespan
        var startTime = $("#constructor-time").data("startTime");
        updateConstructorTime(startTime)
     
    });

    $("#typeSelector").click(function (event) {
        var selectedType = $(event.target).val();

        if (selectedType != undefined && selectedType != null) {
            //console.error(selectedType)

            //update constructor
            var typeName = $('#typeSelector').find(':selected').data("typename")
            if (typeName == "Laborator") {
                $('#constructorDisType').text("lab.")
            } else {
                $('#constructorDisType').text(typeName[0].toLowerCase() +'.')
            }

            if (selectedType === "1" /*for CURS*/) { 
               // console.error("toggled multi (curs)")
                $("#group-select-row").toggleClass('closed');
                $("#multi-group-select").removeClass('closed');
            } else{
               // console.error("toggled single")
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

    $("#cancel-button-addlesson").click(function () {
        window.location.href = "/Home/Schedule";
    })
    

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
                //console.error(responses)

                var addStatus = false;
                $.each(responses, function (index, response) {
                    if (response && typeof response === 'object') {
                        console.error(response["Status"])
                        if (response["Status"]) {
                            addStatus = true;
                            return;
                        } 
                    }
                });
                if (addStatus) {
                    window.location.href = "/Home/Schedule";
                }

                $('#postStatus').empty();
                
                $.each(responses, function (index, response) {
                    if (response && typeof response === 'object') {
                        if (response["Status"] == false) {
                            $('#postStatus').append(
                                $('<div></div>').text((index + 1) + ". " + response["ActionStatusMsg"]).toggleClass("response-message-card error-response-addclass")
                            )

                        } else {
                            $('#postStatus').append(
                                $('<div></div>').text((index + 1) + ". " + response["ActionStatusMsg"]).toggleClass("response-message-card success-response-addclass")
                            )
                        }
                    }
                });
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

        if (selectorValue != null) {
            $("#constructorClassroom").text("cabinet");
        }
        
        if (selectorValue != null && selectorValue != -1) {

            jsonData = {
                StartHour: $('#hour').val(),        //get from class
                StartMinute: $('#minute').val(),    //get from class
                Span: $("input[name='length']:checked").val(), //get from class
                Floor: $("#floorSelector").find(':selected').val(),  //find 
                WeekdayId: $('#day').val(), //get from class
                Frequency: $("input[name='frequency']:checked").val() //get from class and check
            }
            loadFreeClassrooms(jsonData)
        }
    });

    $("#classroomSelector").click(function (event) {
        if (event != null) {
            if ($("#classroomSelector").find(':selected').val() > 0) {
                $("#constructorClassroom").text(
                    $("#classroomSelector").find(':selected').text()
                )
            }
        }
    });


    //EDIT-LESSON-LISTENERS
    $("#cancel-button-modlesson").click(function () {
        window.location.href = "/Home/Schedule";
    })

    $("#submit-button-modlesson").click(function () {
        //gather json data.
    })

    $("#modClassroomSelector").click(function (event) {
        if (event != null) {
            if ($("#modClassroomSelector").find(':selected').val() > 0) {
                $("#modConstructorClassroom").text(
                    $("#modClassroomSelector").find(':selected').text()
                )
            }
        }
    });

    //on change constructor updates
    $(".mod-lesson-radio-btns").click(function () {
        loadFreeClassrooms({
            StartHour: $('#modStartimeSelector').find(':selected').data("hours"),
            StartMinute: $('#modStartimeSelector').find(':selected').data("minutes"),
            Span: $("input[name='modLength']:checked").val(),
            Floor: $('#modFloorSelector').find(':selected').val(),
            WeekdayId: $('#modWeekdaySelector').find(':selected').val(),
            Frequency: $("input[name='modFrequency']:checked").val()
        })
    });

    $("#modWeekdaySelector").click(function () {
        var selectedOption = $(this).find(':selected').text()
        if(selectedOption != null)
        $("#modConstructorWeekday").text(
            selectedOption
        )
    });

    $("#modStartimeSelector").click(function () {
        var selectedOption = $(this).find(':selected').text()
        if (selectedOption != null) {

            $("#modStartime").text(
                selectedOption
            )

            //update endtime based on current span
            var currentSpan = $("input[name='modLength']:checked").val()
            if (currentSpan == 2) {
                //startTime check
                if ($("#modStartime").text() == "11:30") {
                    //add up 3:30
                    $("#modEndtime").text(
                        addTime(selectedOption, 3, 30)
                    )
                } else {
                    //add up 3:15
                    $("#modEndtime").text(
                        addTime(selectedOption, 3, 15)
                    )
                }
            } else {
                $("#modEndtime").text(
                    addTime(selectedOption, 1, 30)
                )
            }
        }
    });

    $("#modClassSpan").click(function () {
        var startTime = $("#modStartime").text()
        var currentSpan = $("input[name='modLength']:checked").val()
        if (currentSpan == 2) {
            //startTime check
            if ($("#modStartime").text() == "11:30") {
                //add up 3:30
                $("#modEndtime").text(
                    addTime(startTime, 3, 30)
                )
            } else {
                //add up 3:15
                $("#modEndtime").text(
                    addTime(startTime, 3, 15)
                )
            }
        } else {
            $("#modEndtime").text(
                addTime(startTime, 1, 30)
            )
        }
    });

    $("#modFreqSelect").click(function () {
        var selectedFreq = $("input[name='modFrequency']:checked").val();
        if (selectedFreq != 0) {
            //even 1
            if (!$('#modConstructorFrequency').hasClass('timetable-label-odd margin-left-3px'))
                $('#modConstructorFrequency').addClass('timetable-label-odd margin-left-3px')

            if (selectedFreq == 1) {
                $('#modConstructorFrequency').text('par')
            }/*odd 2*/else if (selectedFreq == 2) {
                $('#modConstructorFrequency').text('impar')
            }
        } else {
            $('#modConstructorFrequency').toggleClass('timetable-label-odd margin-left-3px')
            $('#modConstructorFrequency').empty();
        }
    });

    $("#modFloorSelector").click(function () {
        var selectorValue = $(this).val();
        if (selectorValue != null && selectorValue != -1) {            
            loadFreeClassrooms({
                StartHour: $('#modStartimeSelector').find(':selected').data("hours"),
                StartMinute: $('#modStartimeSelector').find(':selected').data("minutes"),
                Span: $("input[name='modLength']:checked").val(),
                Floor: $('#modFloorSelector').find(':selected').val(),
                WeekdayId: $('#modWeekdaySelector').find(':selected').val(),
                Frequency: $("input[name='modFrequency']:checked").val()
            })
        }
    })

    $('.edit-lesson-button').click(function () {
        $("#schedule-container").css("display", "none");
        $("#modlesson-container").css("display", "block");


        var id = $(this).data("classId");

        $.ajax({
            url: "/Home/GetClassById",
            method: "GET",
            data: { classId: id },

            success: function (classData) {
                $("#editedClassInfo").text(classData.UserDiscipline.Type.TypeName + ' ' + classData.UserDiscipline.Discipline.Name);
                $("#editClassAcadgroup").text(classData.AcademicGroup.Name);


                //set discipline
                $("#modConstructorDisName").text(classData.UserDiscipline.Discipline.ShortName)

                //set discipline type
                var typeName = classData.UserDiscipline.Type.TypeName
                if (typeName == "Laborator") {
                    $('#modConstructorDisType').text("lab.")
                } else {
                    $('#modConstructorDisType').text(typeName[0].toLowerCase() + '.')
                }


               //set default weekday
               $("#modWeekdaySelector option[value='" + classData.WeekDay.Id + "']").prop("selected", true).prop("disabled", true);
               $("#modConstructorWeekday").text(classData.WeekDay.Name)

               //set default start time
               $("#modStartimeSelector option[data-hours='" + classData.StartTime.Hours + "']").prop("selected", true).prop("disabled", true);
               $("#modStartime").text(
                   formatTime24hrs(classData.StartTime.Hours, classData.StartTime.Minutes)
                )
                //set default end time
                $("#modEndtime").text(
                    formatTime24hrs(classData.EndTime.Hours, classData.EndTime.Minutes)
                )

                //update span by endtime
                var timeDiff = classData.EndTime.TotalMinutes - classData.StartTime.TotalMinutes
                if (timeDiff > 90/*timeDiff of a single span class*/) {
                    $("input[name='modLength'][value=2]").attr("checked", "checked");
                } else {
                    $("input[name='modLength'][value=1]").attr("checked", "checked");
                }

                //set default frequency
                if (classData.Frequency != 0) {
                    //even 1
                    if (classData.Frequency == 1) {
                        $('#modConstructorFrequency').toggleClass('timetable-label-even margin-left-3px').text('par')
                    }/*odd 2*/else if (classData.Frequency == 2) {                        
                        $('#modConstructorFrequency').toggleClass('timetable-label-odd margin-left-3px').text('impar')
                    }
                }

                //set default day and floor.
                //floor
                $("#modFloorSelector option[value='" + classData.ClassRoom.Floor + "']").prop("selected", true);

                //updating classrooms acording to the floor then select and disable current classroom.
                //gather json data
                

                //set default classroom
                setTimeout(function () {
                    $("#modClassroomSelector option[value='" + classData.ClassRoom.Id + "']").prop("selected", true).prop("disabled", true);
                    $("#modConstructorClassroom").text(
                        classData.ClassRoom.ClassroomName
                    )
                }, 700)

                loadFreeClassrooms({
                    StartHour: classData.StartTime.Hours,       
                    StartMinute: classData.EndTime.Minutes,    
                    Span: $("input[name='modLength']:checked").val(), 
                    Floor: classData.ClassRoom.Floor,  
                    WeekdayId: classData.WeekDay.Id,
                    Frequency: $("input[name='modFrequency']:checked").val() 
                })


            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.error("Full jqXHR object:", jqXHR);
            }        
        });
    });
});

function formatTime24hrs(hours, minutes) {
    const formatHours = hours.toString().padStart(2, '0');
    const formatMinutes = minutes.toString().padStart(2, '0');

    return formatHours + ':' + formatMinutes;
}

function updateConstructorTime(startTime) {
    var endTime;
    var checkedSpan = $("input[name='length']:checked").val()
    if (checkedSpan == 2) {
        if (startTime == "11:30") {
            endTime = addTime(startTime, 3, 30);
        } else {
            endTime = addTime(startTime, 3, 15);
        }
    } else {
        endTime = addTime(startTime, 1, 30);
    }

    $('#constructor-time').empty();
    $('#constructor-time').text(startTime + " - " + endTime);
}

function addTime(startTime, hours, minutes) {
    // Parse the starting time into a Date object
    const parsedTime = new Date(`2024-04-29 ${startTime}`); // Replace with a real date if needed

    // Convert individual units to milliseconds and add them together
    const milliseconds = (hours * 60 * 60 * 1000) + (minutes * 60 * 1000);

    // Add the duration to the time
    parsedTime.setTime(parsedTime.getTime() + milliseconds);

    // Format the resulting time (optional)
    const formattedTime = parsedTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12:false });

    return formattedTime;
}

function loadFreeClassrooms(jsonData) {
    $.ajax({
        url: "/Home/GetFreeClassroomsByFloor",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({ requestData: jsonData }),
        success: function (classrooms) {
            // console.error(data);
            updateClassroomsSelect("#classroomSelector", classrooms, true)
            updateClassroomsSelect("#modClassroomSelector", classrooms, false)
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

function updateClassroomsSelect(selectorId, classrooms, setDefault) {
    $(selectorId).empty();
    if (setDefault) {
        $(selectorId).append(
            $('<option disabled selected></option>').text("cabinetul").val(-1)
        );
    }


    if (classrooms.length == 0) {
        $(selectorId).append(
            $('<option></option>').text("Nu există cabinete disponibile").prop("selected", true).prop("disabled", true).val(-1)
        );
        //return;
    }

    $.each(classrooms, function (index, item) {
        //console.error("id:"+item.Id+" name:"+item.ClassroomName)
        $(selectorId).append(
            $('<option></option>').val(item.Id).text(item.ClassroomName).addClass("sumShit")
        );
    });
}

function updateDisciplineSelect(data) {
    $('#disciplineSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("disciplina").val(-1);
    $('#disciplineSelector').append(defaultOption);
    $.each(data, function (index, item) { //////////////////////////////DID-STUFF//////DID-STUFF//////DID-STUFF//////DID-STUFF//////DID-STUFF///
        var option = $('<option></option>').val(item.Id).text(item.Name).attr("data-shortname", item.ShortName);
        $('#disciplineSelector').append(option);
    });
}

function updateDisciplineTypeSelect(data) {

    $('#typeSelector').empty();
    var defaultOption = $('<option disabled selected></option>').text("tipul").val(-1);

    $('#typeSelector').append(defaultOption);
     $.each(data, function (index, item) {
         var option = $('<option></option>').val(item.Id).text(item.TypeName).attr('data-typename', item.TypeName);
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

    $.each(data, function (index, item) {
        $('#groupSelector').append(
            $('<option></option>').val(item.Id).text(item.Name)
        );
    });
}

