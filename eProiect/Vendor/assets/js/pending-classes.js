$(document).ready(function () {
    loadClassGroups();
    /*const currentUrl = window.location.href;
    console.log(currentUrl); */

    $('#pendingClassesTable').click(function (event) {
        const button = $(event.target).closest('button'); 
        const row = $(event.target).closest('tr'); 

        if (button.length > 0 && button.hasClass('approve-class')) {


            $.ajax({
                url: "/Home/ConfirmPendingClass",
                method: "POST",
                data: { classId: button.attr("lessonId") },
                success: function (response) {
                    row.addClass('succes-row')
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
            /*console.error("APPROVE: cls-id:", button.attr("lessonId"))
            console.error("row: ", row)*/
            $.ajax({
                url: "/Home/DeleteClass",
                method: "POST",
                data: { id: button.attr("lessonId") },
                success: function (response) {
                    row.addClass('fail-row')
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

    })

    

});

function loadClassGroups() {
    $.ajax({
        url: '/Admin/GetPendingClasses',
        method: 'GET',
        success: function (data) {
            //console.error(data)
            insertClassesInTable(data)
        },

        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function insertClassesInTable(classLists) {
    $.each(classLists, function (index, classList) {
  
        
        if (Object.keys(classList.length == 1)) {    
            //console.error(classList.OverlapGroup)
            appendRowsToTable(classList.OverlapGroup);
            
        } else if (Object.keys(classList).length > 1) {
            
            appendRowsToTable(classList.OverlapGroup)   
        }
        
    });
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
        url: "/Home/ConfirmPendingClass",
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

function appendRowsToTable(items) { 
    const frequencyMap = {
        0: "Săptămânal",
        1: "Par",
        2: "Impar"  
    }

    $.each(items, function (index, item) {
        var newRow = $("<tr></tr>");

        if (items.length > 1) {
            var newRow = $("<tr></tr>");
            $('#pendingClassesTable').append(
                newRow.addClass("table-warning-row").append(
                    $("<td></td>").addClass("rowlike-tablecell font-weight-bold no-bg").text(index + 1),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.User.Name + ' ' + item.UserDiscipline.User.Surname),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.Discipline.ShortName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.UserDiscipline.Type.TypeName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.AcademicGroup.Name),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.ClassRoom.ClassroomName),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(item.WeekDay.Name),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(formatTime(item.StartTime.Hours, item.StartTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell no-bg").text(formatTime(item.EndTime.Hours, item.EndTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell-rb no-bg").text(frequencyMap[item.Frequency]),

                )
            );

            if (index == 0) {
                newRow.append($("<td></td>").addClass("rowlike-tablecell").attr("rowspan", items.length).append(
                    $("<button></button>").append($("<i></i>").addClass('ti-eye')),
                ))
            }
        } else {
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
                    /*$("<td></td>").addClass("rowlike-tablecell").append(
                        $("<button></button>").append($("<i></i>").addClass('ti-check')).attr("onclick", "confirmPendingClass("+item.Id+")"),
                        $("<button></button>").append($("<i></i>").addClass('ti-close')).attr("onclick", "denyPendingClass("+item.Id+")")                      
                    )*/
                    $("<td></td>").addClass("rowlike-tablecell").append(
                        $("<button></button>").append(
                            $("<i></i>").addClass('ti-check')
                        )
                        .attr("lessonId", item.Id)
                        .addClass("approve-class"),

                        $("<button></button>").append(
                            $("<i></i>").addClass('ti-close')
                        )
                        .attr("lessonId", item.Id)
                        .addClass("deny-class")                  
                    )
                )
            );
           
        }
    });
}