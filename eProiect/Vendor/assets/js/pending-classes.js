$(document).ready(function () {
    loadClassGroups();
    /*const currentUrl = window.location.href;
    console.log(currentUrl); */

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
                    $("<td></td>").addClass("rowlike-tablecell font-weight-bold").text(index + 1),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.User.Name + ' ' + item.UserDiscipline.User.Surname),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.Discipline.ShortName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.Type.TypeName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.AcademicGroup.Name),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.ClassRoom.ClassroomName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.WeekDay.Name),
                    $("<td></td>").addClass("rowlike-tablecell").text(formatTime(item.StartTime.Hours, item.StartTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell").text(formatTime(item.EndTime.Hours, item.EndTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell-rb").text(frequencyMap[item.Frequency]),
                    
                )
            );

            if (!index == items.length - 1) {
                newRow.append($("<td></td>"))
            } else {                
                newRow.append(
                    $("<td></td>").addClass("rowlike-tablecell").append(
                        $("<button></button>").append($("<i></i>").addClass('ti-eye')),
                    ))
            }
            
        } else {
            $('#pendingClassesTable').append(
                $("<tr></tr>").append(
                    $("<td></td>").addClass("rowlike-tablecell"),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.User.Name + ' ' + item.UserDiscipline.User.Surname),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.Discipline.ShortName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.UserDiscipline.Type.TypeName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.AcademicGroup.Name),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.ClassRoom.ClassroomName),
                    $("<td></td>").addClass("rowlike-tablecell").text(item.WeekDay.Name),
                    $("<td></td>").addClass("rowlike-tablecell").text(formatTime(item.StartTime.Hours, item.StartTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell").text(formatTime(item.EndTime.Hours, item.EndTime.Minutes)),
                    $("<td></td>").addClass("rowlike-tablecell-rb").text(frequencyMap[item.Frequency]),
                    $("<td></td>").addClass("rowlike-tablecell").append(
                        $("<button></button>").append($("<i></i>").addClass('ti-check')).attr("lesson-id", item.Id),
                        $("<button></button>").append( $("<i></i>").addClass('ti-close')).attr("lesson-id", item.Id)                      
                    )
                )
            );
        }
    });
}