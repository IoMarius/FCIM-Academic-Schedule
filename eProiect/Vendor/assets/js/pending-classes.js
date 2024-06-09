$(document).ready(function () {    
    const weekDays = ["Luni", "Marți", "Miercuri", "Joi", "Vineri", "Sâmbătă"];
    const timeIntervals = [
        new TimeInterval(8, 0, 9, 30),
        new TimeInterval(9, 45, 11, 15),
        new TimeInterval(11, 30, 13, 0),
        new TimeInterval(13, 30, 15, 0),
        new TimeInterval(15, 15, 16, 45),
        new TimeInterval(17, 0, 18, 30),
        new TimeInterval(18, 45, 20, 15)
    ] 
    const frequencies = ["Săptămânal", "Par", "Impar"];
    const floors = [1, 2, 3, 4, 5, 6, 7];

    function loadPendingClasses() {
        $.ajax({
            url: '/Admin/GetPendingClasses',
            method: 'GET',
            success: function (data) {
                if (data.length == 0) {
                    $('#pendingMessage').toggleClass('closed');
                    $('#pendingClassesTableWhole').toggleClass('closed');                    
                } else {
                    appendRowsToTable(data)
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.error("Full jqXHR object:", jqXHR);
            }
        });
    }

    function loadConflictingClasses() {
        $.ajax({
            url: '/Admin/GetConflictingClasses',
            method: 'GET',
            success: function (data) {
                if (data.length == 0) {
                    $('#conflictMessage').toggleClass('closed');
                }
                else
                {
                    //append classes to conflict container:
                    $("#conflictsContainer").empty();
                    insertConflicts($("#conflictsContainer"), data, weekDays, timeIntervals, frequencies, floors);
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                console.error("AJAX Error:", textStatus, errorThrown);
                console.error("Full jqXHR object:", jqXHR);
            }
        });
    }
   
    function appendRowsToTable(items) {
        const frequencyMap = {
            0: "Săptămânal",
            1: "Par",
            2: "Impar"
        }

        $.each(items, function (index, item) {
            $('#pendingClassesTable').append(
                $("<tr></tr>").addClass("normal-state-row").append(
                    $("<td></td>").addClass("rowlike-tablecell no-bg"),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.User.Name + ' ' + item.UserDiscipline.User.Surname),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.Discipline.ShortName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.Type.TypeName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.AcademicGroup.Name),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.ClassRoom.ClassroomName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.WeekDay.Name),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(formatTime(item.StartTime.Hours, item.StartTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell no-bg ").text(formatTime(item.EndTime.Hours, item.EndTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell-rb no-bg").text(frequencyMap[item.Frequency]),
                    $("<td></td>").addClass("rowlike-tablecell").append(
                        $("<div>").addClass("d-flex justify-content-around").append(
                            $("<button></button>").addClass("generic-button generic-button-positive decision-button").append(
                                $("<i></i>").addClass('ti-check')
                            )
                                .attr("lessonId", item.Id)
                                .addClass("approve-class"),
                            $("<button></button>").addClass("generic-button generic-button-negative decision-button").append(
                                $("<i></i>").addClass('ti-close')
                            )
                                .attr("lessonId", item.Id)
                                .addClass("deny-class")
                        )
                    )
                )
            );
        });

       
    }
        
    loadPendingClasses();

    loadConflictingClasses();

    $('#pendingClassesTable').click(function (event) {
        const button = $(event.target).closest('button');
        const row = $(event.target).closest('tr');

        checkConflictsState();

        if (button.length > 0 && button.hasClass('approve-class')) {
            $.ajax({
                url: "/Admin/ConfirmPendingClass",
                method: "POST",
                data: { classId: button.attr("lessonId") },
                success: function (response) {
                    row.addClass('succes-row')
                    loadConflictingClasses();
                    setTimeout(function () {
                        row.addClass('no-opacity')
                        setTimeout(function () {
                            row.addClass('closed')
                        }, 300)
                    }, 400)
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }

            })
        }

        if (button.length > 0 && button.hasClass('deny-class')) {
            $.ajax({
                url: "/Home/DeleteClass",
                method: "POST",
                data: { id: button.attr("lessonId") },
                success: function (response) {
                    row.addClass('fail-row')
                    loadConflictingClasses();
                    setTimeout(function () {
                        row.addClass('no-opacity')
                        setTimeout(function () {
                            row.addClass('closed')
                        }, 300)
                    }, 400)
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }
            })
        }

    });

    $('#conflictsContainer').click(function (event) {
        const button = $(event.target).closest('button');
        if (button.length > 0 && button.hasClass('inspect-conflict-button')) {
            var index = 0;            
            
            $('#conflictSolverCard').empty();

            while (button.attr(`class-id-${index}`) != undefined) {
                var classReqId = button.attr(`class-id-${index}`);

                var currentClass = getClassById(classReqId);
                $('#conflictSolverCard').append(
                    addEditBlock(
                        currentClass
                    )
                );

                initSelectWithArray(`modWeekdaySelector${currentClass.Id}`, weekDays, currentClass.WeekDay.Id);
                initTimeSelect(`modStartimeSelector${currentClass.Id}`, timeIntervals, currentClass.StartTime.Hours);
                initFrequencyRadio(`modFreqSelect${currentClass.Id}`, currentClass.Frequency);
                initSpanRadio(`modLength${currentClass.Id}`, currentClass.EndTime.TotalMinutes - currentClass.StartTime.TotalMinutes);
                initSelectWithArray(`modFloorSelector${currentClass.Id}`, floors, currentClass.ClassRoom.Floor);
                initClassroomSelect(`modClassroomSelector${currentClass.Id}`, currentClass.ClassRoom.Floor, currentClass.ClassRoom.Id);

                index += 1;               

            }
            highlightConflicts();

            $('#conflictsList').toggleClass("closed");
            $('#inEditConflicts').toggleClass("closed");
           
        }
    });

    $('#conflictSolverCard').click(function (event) {
        if (event.target.tagName == "SELECT" || event.target.tagName== "INPUT"){
            highlightConflicts();
        }
        if (event.target.id.includes("Floor")) {
            //target class: 
            var classId = event.target.id.replace(/^\D+/g, "");
            updateClassroomsForClass(classId);
        }
    });

});

async function checkConflictsState() {      
    if ($('.active-conflict-element').length == 0)
    {
        if (!$('#conflictMessage').hasClass('closed')){
            $('#conflictMessage').addClass('closed');
        }
    }


    if ($('.normal-state-row').length==0)
    {
        if (!$('#pendingClassesTableWhole').hasClass('closed')){            
            $('#pendingClassesTableWhole').addClass('closed');
        }
    }
}

function returnToConflictList() {
    $('#conflictsList').toggleClass("closed");
    $('#inEditConflicts').toggleClass("closed");
}

function solveConflict() {
    if (highlightConflicts()) {
        const activeConflicts = $(".conflict-in-edit");
        $('#responseBlock').empty();
        $.each(activeConflicts, function (index, conflict) {
            var conId = $(conflict).attr("value");
            saveClassChanges(conId);
        })

        //restart page
        

    } else {
        //notify user that conflicts are present.
        $('.float-card').toggleClass("error-bg");
        setTimeout(function () {
            $('.float-card').toggleClass("error-bg");
        }, 500);

    }
}

async function saveClassChanges(id) {
    modifyClassRequest = {
        ClassId: id,
        WeekdayId: $(`#modWeekdaySelector${id}`).find(':selected').val() - 1,
        StartHours: $(`#modStartimeSelector${id}`).find(':selected').attr("value"),
        StartMinutes: $(`#modStartimeSelector${id}`).find(':selected').attr("value-minutes"),
        Span: $(`input[name='modLength${id}']:checked`).val(),
        Floor: $(`#modFloorSelector${id}`).find(':selected').val(),
        Frequency: $(`input[name='modFrequency${id}']:checked`).val(),
        ClassroomId: $(`#modClassroomSelector${id}`).find(':selected').val(),
        UserDisciplineId: $(`#modCardData${id}`).attr("user-discipline-id"),

        ClassTypeId: $(`#modCardData${id}`).attr("class-type-id"),
        AcademicGroupId: $(`#modCardData${id}`).attr("academic-group-id"),
        AcademicGroupYear: $(`#modCardData${id}`).attr("academic-group-year")
    }

    $.ajax({
        url: "/Home/EditExistingClassroom",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({ data: modifyClassRequest }),

        success: function (response) {
            if (response.Status == false) {                
                $('#responseBlock').append(
                    $('<div></div>').addClass('response-message-card error-response-addclass').append(
                        $('<p></p>').addClass('m-b-0').text(response.ActionStatusMsg)
                    )
                );
            } else {
                window.location.href = "/Admin/PendingClasses";
            }    
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

async function updateClassroomsForClass(classId) {    
    request = {
        StartHour: $(`#modStartimeSelector${classId}`).find(':selected').attr("value"),
        StartMinute: $(`#modStartimeSelector${classId}`).find(':selected').attr("value-minutes"),
        Span: $(`input[name='modLength${classId}']:checked`).val(),
        Floor: $(`#modFloorSelector${classId}`).find(':selected').val(),
        WeekdayId: $(`#modWeekdaySelector${classId}`).find(':selected').val() - 1,
        Frequency: $(`input[name='modFrequency${classId}']:checked`).val()
    }

  
    var resultClassrooms;
    
    $.ajax({
        url: '/Home/GetFreeClassroomsByFloor',
        method: 'POST',
        data: { requestData: request },
        async: false,
        success: function (classrooms) {
            resultClassrooms=classrooms;
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });


    $.ajax({
        url: '/Home/GetClassById',
        method: 'GET',
        data: { classId: classId },
        async: false,
        success: function (resultClass) {    
            if (request.Floor == resultClass.Floor) {
                resultClassrooms.unshift(resultClass.ClassRoom);
            }
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });

    //MAKE SELECTED ONE 
    $(`#modClassroomSelector${classId}`).empty();
    $.each(resultClassrooms, function (index, classroom) {
        var option = $('<option>').val(classroom.Id).text(classroom.ClassroomName);
               
        if (classroom.Id == resultClassrooms[0].Id) {
            option.attr("selected", "selected");
        }
        
        $(`#modClassroomSelector${classId}`).append(option);
    })
}

async function markConflict(conflictId) {
    if (!$(conflictId).hasClass('error-bg')) {
        $(conflictId).addClass('error-bg');                
    }
}

async function unmarkConflict(conflictId) {       
    $(conflictId).removeClass('error-bg');
}

function highlightConflicts() {        
    const activeConflicts = $(".conflict-in-edit");
    var frequencies = [];
    var days = [];
    var hours = [];
    var classrooms = [];
    var missingConflict = true;

    $.each(activeConflicts, function (index, conflict) {
        //conflictIds.push($(conflict).attr("value"));

        conflictId = $(conflict).attr("value");
        frequencies.push($(`input[name='modFrequency${conflictId}']:checked`).attr('value'));
        days.push($(`#modWeekdaySelector${conflictId}`).find(':selected').attr('value'));
        hours.push($(`#modStartimeSelector${conflictId}`).find(':selected').attr('value'));
        classrooms.push($(`#modClassroomSelector${conflictId}`).find(':selected').attr('value'));    
    });
    

    if (new Set(frequencies).size === 1) {
        for (let i = 0; i < activeConflicts.length; i++) {
            for (let j = 0; j < activeConflicts.length; j++) {
                if (i == j) {
                    continue;
                }                
                //mark hours if days are the same:
                if (days[i] == days[j]) {
                    if (hours[i] == hours[j]) {      
                    
                        
                        missingConflict = false;
                        markConflict(`#modStartimeSelector${$(activeConflicts[i]).attr("value")}`);
                        markConflict(`#modStartimeSelector${$(activeConflicts[j]).attr("value")}`);
                    } else {                        
                        unmarkConflict(`#modStartimeSelector${$(activeConflicts[i]).attr("value")}`);
                        unmarkConflict(`#modStartimeSelector${$(activeConflicts[j]).attr("value")}`);
                    }
                } else {
                    unmarkConflict(`#modStartimeSelector${$(activeConflicts[i]).attr("value")}`);
                    unmarkConflict(`#modStartimeSelector${$(activeConflicts[j]).attr("value")}`);
                }

                if (classrooms[i] != undefined) {                    
                    if (classrooms[i] == classrooms[j]) {        
                        missingConflict = false;
                        markConflict(`#modClassroomSelector${$(activeConflicts[i]).attr("value")}`);
                        markConflict(`#modClassroomSelector${$(activeConflicts[j]).attr("value")}`);
                    } else {
                        unmarkConflict(`#modClassroomSelector${$(activeConflicts[i]).attr("value")}`);
                        unmarkConflict(`#modClassroomSelector${$(activeConflicts[j]).attr("value")}`);
                    }
                }
            }
        }

    } else {
        //unmark conflicts
        for (let i = 0; i < activeConflicts.length; i++) {
            unmarkConflict(`#modFreqSelect${$(activeConflicts[i]).attr("value")}`);
            unmarkConflict(`#modStartimeSelector${$(activeConflicts[i]).attr("value")}`);   
            unmarkConflict(`#modClassroomSelector${$(activeConflicts[i]).attr("value")}`);            
        }
    }

    return missingConflict;
}

function getClassById(id) {
    var classdata;
    $.ajax({
        url: '/Home/GetClassById',
        method: 'GET',
        async: false,
        data: { classId: id },
        success: function (currentClass) {
            classdata = currentClass;
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
    return classdata;
}

function initClassroomSelect(selectId, floorNr, classroomId) {
    const select = $(`#${selectId}`);
    select.empty();

    $.ajax({
        url: '/Home/GetClassRoomsByFloor',
        method: 'GET',
        data: { floor: floorNr },
        success: function (classrooms) {
            $.each(classrooms, function (index, classroom) {
                var option = $('<option>').val(classroom.Id).text(classroom.ClassroomName);
                
                if (classroom.Id == classroomId) {
                    option.attr("selected", "selected");
                }

                select.append(option);
            })
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function initFrequencyRadio(inputName, selectedFreq) {
    $(`input[name='${inputName}'][value=${selectedFreq}]`).attr("checked", "checked");
}

function initSpanRadio(inputName, timeDiff) {
    if (timeDiff > 90) {
        $(`input[name='${inputName}'][value=2]`).attr("checked", "checked");
    } else {
        $(`input[name='${inputName}'][value=1]`).attr("checked", "checked");
    }
}

function initSelectWithArray(selectId, weekDays, selectedId) {
    const select = $(`#${selectId}`)
    select.empty();

    $.each(weekDays, function (index, item) {   
        var option = $('<option>').val(index + 1).text(item);

        if (index + 1 == selectedId) {
            option.attr("selected", "selected");
        }

        select.append(option);
    });
}

function initTimeSelect(selectId, timeIntervals, selectedHour) {
    const select = $(`#${selectId}`)
    select.empty();

    $.each(timeIntervals, function (index, item) {
        var option = $('<option>').val(item.startHour).attr("value-minutes", item.startMinute).text(item.formatStringStart());

        if (item.startHour == selectedHour) {
            option.attr("selected", "selected");
        }

        select.append(option);
    });
}

function addEditBlock(classData) {
    return `<div id="modClassSection" class="row float-card" style="margin-bottom:0; max-width: 284px; padding: 10px 0 10px 0;">
            <div class="col">
                <hidden class="conflict-in-edit" value="${classData.Id}"></hidden>
                <hidden id="modCardData${classData.Id}" user-discipline-id="${classData.UserDiscipline.Id}" class-type-id="${classData.UserDiscipline.Type.Id}" academic-group-id="${classData.AcademicGroup.Id}" academic-group-year="${classData.AcademicGroup.Year}"></hidden>

                <!--SELECTORS-->    
                <div class="container p-0" style="margin:0; ">
                    <div class="d-flex align-items-center flex-column f-16 m-b-10">
                        <span class="font-weight-bold w-fc">${classData.UserDiscipline.User.Surname + ' ' + classData.UserDiscipline.User.Name}</span>
                        <span class="w-fc">${classData.UserDiscipline.Discipline.Name}</span>
                        <span class="w-fc">${classData.UserDiscipline.Type.TypeName} ${classData.AcademicGroup.Name}</span>
                    </div>
                    

                    <!--Weekday, Start time-->
                    <div class="row">
                        <div class="col">
                            <select id="modWeekdaySelector${classData.Id}" name="select" class="mod-lesson-mdfiers form-control form-control-inverse">
                                <option value="1">Luni</option>
                                <option value="2">Marți</option>
                                <option value="3">Miercuri</option>
                                <option value="4">Joi</option>
                                <option value="5">Vineri</option>
                                <option value="6">Sâmbătă</option>
                            </select>   
                        </div>
                    </div>


                    <div class="row">
                        <div class="col">
                            <select id="modStartimeSelector${classData.Id}" name="select" class="mod-lesson-mdfiers form-control form-control-inverse">
                                <option data-hours="8" data-minutes="00">08:00</option>
                                <option data-hours="9" data-minutes="45">09:45</option>
                                <option data-hours="11" data-minutes="30">11:30</option>
                                <option data-hours="13" data-minutes="30">13:30</option>
                                <option data-hours="15" data-minutes="15">15:15</option>
                                <option data-hours="17" data-minutes="0">17:00</option>
                                <option data-hours="18" data-minutes="45">18:45</option>
                            </select>
                        </div>
                    </div>



                    <!--Even Odd radio buttons-->
                    <div class="form-radio">
                        <div class="row">
                            <div class="col-5">
                                <div id="modFreqSelect${classData.Id}" class="mod-lesson-mdfiers class-param-radio radio radiofill radio-info" style="display: inline-flex; flex-direction: row-reverse;">
                                    <label class="class-type-label">
                                        <input type="radio" name="modFrequency${classData.Id}" value="1" checked="checked" data-frequency="par">
                                        <i class="helper"></i>Pară
                                    </label>

                                    <label class="class-type-label">
                                        <input type="radio" name="modFrequency${classData.Id}" value="2" checked="checked" data-frequency="impar">
                                        <i class="helper"></i>Impară
                                    </label>

                                    <label class="class-type-label">
                                        <input type="radio" name="modFrequency${classData.Id}" value="0" checked="checked" data-frequency="0">
                                        <i class="helper"></i>Săptămânală
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Lesson duration-->
                    <div class="form-radio">
                        <div class="row">
                            <div class="col">
                                <div id="modClassSpan${classData.Id}" class="mod-lesson-mdfiers class-param-radio radio radiofill radio-info" style="display: inline-flex; flex-direction: row-reverse;">
                                    <label class="class-type-label">
                                        <input type="radio" name="modLength${classData.Id}" value="2">
                                        <i class="helper"></i>2 perechi
                                    </label>

                                    <label class="class-type-label">
                                        <input type="radio" name="modLength${classData.Id}" value="1">
                                        <i class="helper"></i>1 pereche
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
            
                    <!--Etajul, cabinetul-->
                    <div class="row">
                        <div class="col">
                            <select id="modFloorSelector${classData.Id}" name="select" class="floor-selector form-control form-control-inverse">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                                <option value="7">7</option>
                            </select>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <select id="modClassroomSelector${classData.Id}" name="select" class="classroom-selector form-control form-control-inverse">
                                <option disabled selected value="-1">cabinetul</option>
                            </select>
                        </div>
                    </div>
            

                </div>
            </div>            
        </div>`;
}

function createElement(elementContainer, data, index, rowClass) {
    var freqMap = {
        0:"Săptămânal",
        1:"Par",
        2:"Impar"
    }

    elementContainer.append(
        $("<div>").addClass("d-flex flex-row " + rowClass).append(
            $("<div>").addClass("row-index d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(index),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.UserDiscipline.User.Surname + ' ' + data.UserDiscipline.User.Name + ' '),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.UserDiscipline.Discipline.ShortName),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.UserDiscipline.Type.TypeName),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.AcademicGroup.Name),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.ClassRoom.ClassroomName),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(data.WeekDay.Name),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(formatTime(data.StartTime.Hours, data.StartTime.Minutes) + " - " + formatTime(data.EndTime.Hours, data.EndTime.Minutes)),
            $("<div>").addClass("d-flex justify-content-center w-100 rowlike-tablecell no-bg p-7").text(freqMap[data.Frequency])                
        ),        
    );
}

function insertConflicts(container, conflicts) {
    $.each(conflicts, function (index, conflict) {        
        var conflictWrapper = $("<div>").addClass("m-b-20 d-inline-flex w-100 active-conflict-element");
        var conflictContainer = $("<div>").addClass("d-flex flex-column conflict-table-border w-100")
        var inspectButton = $("<button>").addClass("w-100 h-100 inspect-conflict-button").append($("<i>").addClass("ti-pencil"));
    

        createElement(conflictContainer, conflict.MainClass, null, "conflict-main");
        inspectButton.attr("class-id-0", conflict.MainClass.Id);
       

        $.each(conflict.OverlappingClasses, function (index, overlap) {                       
            inspectButton.attr(`class-id-${index + 1}`, overlap.Id);
            createElement(conflictContainer, overlap, index+1, "conflict-member")
        });


        conflictWrapper.append(conflictContainer, $("<div>").append(inspectButton));
        container.append(conflictWrapper)
    });
}

function calcSpan(startHour, startMinutes, endHour, endMinutes) {
    const startDate = new Date(0, 0, 0, startHour, startMinutes); // Create Date objects for comparison
    const endDate = new Date(0, 0, 0, endHour, endMinutes);

    if (endDate < startDate) {
        endDate.setDate(endDate.getDate() + 1); // Handle overnight shifts by adding a day
    }

    const diffInMilliseconds = endDate - startDate; // Difference in milliseconds
    const diffInSeconds = diffInMilliseconds / 1000;
    const hours = Math.floor(diffInSeconds / 3600);
    const minutes = Math.floor((diffInSeconds % 3600) / 60);

    if (hours == 1)
        return 1;
    else
        return 2;
}

function formatTime(hours, minutes) {
    if (hours < 0 || minutes < 0) {
        return "00:00";
    }

    hours = hours.toString().padStart(2, "0");
    minutes = minutes.toString().padStart(2, "0");
    return hours + ':' + minutes;
}

function confirmPendingClass(classId) {
    $.ajax({
        url: "/Ad/onfirmPendingClass",
        method: "POST",
        data: { classId: classId },
        success: function (response) {
            //console.error(response)
            //NOTIFY USER THEN COLLAPSE ROW
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
        
    })
}

function denyPendingClass(classId) {
    $.ajax({
        url: "/Home/DeleteClass",
        method: "POST",
        data: { id: classId },
        success: function (response) {
            // console.error(response)
           //NOTIFY USER THEN COLLAPSE ROW
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    })
}


class TimeInterval {
    constructor(startHour, startMinute, endHour, endMinute) {
        this.startHour = startHour;
        this.startMinute = startMinute;
        this.endHour = endHour;
        this.endMinute = endMinute;
    }

    formatString() {
        var formatStartHour = this.startHour.toString().padStart(2, "0");
        var formatStartMinute = this.startMinute.toString().padStart(2, "0");

        var formatEndHour = this.endHour.toString().padStart(2, "0");
        var formatEndMinute = this.endMinute.toString().padStart(2, "0");

        return formatStartHour + ':' + formatStartMinute + ' - ' + formatEndHour + ':' + formatEndMinute;
    }

    formatStringStart() {
        var formatStartHour = this.startHour.toString().padStart(2, "0");
        var formatStartMinute = this.startMinute.toString().padStart(2, "0");
        return formatStartHour + ':' + formatStartMinute;
    }

    formatStringEnd() {
        var formatEndHour = this.endHour.toString().padStart(2, "0");
        var formatEndMinute = this.endMinute.toString().padStart(2, "0");
        return formatEndHour + ':' + formatEndMinute;
    }
}
