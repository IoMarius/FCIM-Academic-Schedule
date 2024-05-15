$(document).ready(function () {
    initScheduleTable();
    var lastSelectedGroup=-1;
    var lastSelectedYear=-1;

    $('#teachersDropdown').click(function (event) {
        if (event.target.id == 'spanOptionTeacher') {
            var TeacherId = $(event.target).val();
            $('#nameInput').toggleClass('opened-input');
            $('#searchIconBox').toggleClass('closed');
            $('#teachersDropdown').toggleClass('closed');

            $('#nameInput').val($(event.target).text())
            loadTeacherClasses(TeacherId);
        }
    });

    $('#nameInput').on("focus", function () {
        $(this).toggleClass('opened-input');
        $('#searchIconBox').toggleClass('closed');
        $('#teachersDropdown').toggleClass('closed');
    });


    $('#switchToGroups').click(function () {
        if ($(this).hasClass('disabled')) {
            $(this).toggleClass('disabled');
            $('#switchToTeachers').toggleClass('disabled');
            cleanTable();

            $('#teachersSelectors').toggleClass('closed');
            $('#academicGroupSelectors').toggleClass('closed');

            $('#newsletterCard').toggleClass('closed')
        }
    })

    $('#switchToTeachers').click(function () {
        if ($(this).hasClass('disabled')){
            $(this).toggleClass('disabled');
            $('#switchToGroups').toggleClass('disabled');
            cleanTable();

            $('#teachersSelectors').toggleClass('closed');
            $('#academicGroupSelectors').toggleClass('closed');

            //close fii la curent thingy
            $('#newsletterCard').toggleClass('closed')

            //get teachers list...
            loadTeachers();
        }
    })

    $('#loginGuest').click(function () {
        console.error("WAS CLICK")
        window.location.href = "/Login/Login"
    });

    $('#yearGuestSelector').click(function () {
        var yearId = $(this).find(':selected').val();
        if (yearId != null && yearId > 0) {
            if (yearId != lastSelectedYear) {               
                loadAcademicGroupsByYear(yearId);
            }
        }
    });

    $('#academicGroupSelector').click(function () {
        var groupId = $(this).find(':selected').val();
        if (groupId != null && groupId>0) {
            if (groupId != lastSelectedGroup) {
                lastSelectedGroup = groupId;
                $('#userNotice').addClass('closed')
                $('#newsletterCard').removeClass('invisible')
                $('#newsletter-message-groupname').text($(this).find(':selected').text())
                loadAcademicGroupClasses(groupId)                
            }
        }
    });
});

function cleanTable() {
    $('.table-cell-wrapper').empty();
    $('.table-cell-wrapper').addClass('table-empty-cell-hiddenmobile');
    $('.collapsed').removeClass('collapsed')
    $('.double-height').removeClass('double-height')
    $('.mobile-visible-col-block').removeClass('mobile-visible-col-block')
}

function filterFunction() {
    const input = document.getElementById("nameInput");
    const filter = input.value.toUpperCase();
    const div = document.getElementById("searchDropdown");
    const a = div.getElementsByTagName("div");

    for (let i = 0; i < a.length; i++) {
        txtValue = a[i].textContent || a[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            a[i].style.display = "";
        } else {
            a[i].style.display = "none";
        }
    }
}

function loadTeachers() {
    $.ajax({
        url: "/Home/GetTeachers",
        method: "GET",
        success: function (data) {
            InsertTeachersInDropdown(data)
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function InsertTeachersInDropdown(teachers) {
    $('#teachersDropdown').empty();
    $.each(teachers, function (index, teacher) {
        $('#teachersDropdown').append(
            $('<div></div>').val(teacher.Id).addClass('pl-2 dropdown-span').attr('Id', 'spanOptionTeacher').append(
                $('<span></span>').text(teacher.Name + ' ' + teacher.Surname)
            )
        )
    });
}

function loadAcademicGroupsByYear(year) {
    if (year != null) {
        if (year > 0 && year < 5) {
            $.ajax({
                url: "/Home/GetOptionsByYear",
                method: "GET",
                data: { year: year },
                success: function (data) {
                    insertOptionsInSelect(
                        '#academicGroupSelector',
                        data,
                        $('<option disabled selected></option>').text("Grupă").val(-1)
                    );                  
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("AJAX Error:", textStatus, errorThrown);
                    console.error("Full jqXHR object:", jqXHR);
                }
            });
        }
    }
}

function loadTeacherClasses(teacherId) {
    $.ajax({
        url: "/Home/GetUserClasses",
        method: "GET",
        data: { userId: teacherId },
        success: function (data) {
            insertClassesIntoSchedule(data, true)           
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}

function loadAcademicGroupClasses(groupId) {
    $.ajax({
        url: "/Home/GetGroupClasses",
        method: "GET",
        data: { groupId: groupId },
        success: function (data) {
            insertClassesIntoSchedule(data)
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX Error:", textStatus, errorThrown);
            console.error("Full jqXHR object:", jqXHR);
        }
    });
}


function getTimeDiff(sHour, sMinute, eHour, eMinute) {
    const startTime = new Date(2024, 4, 11, sHour, sMinute);
    const endTime = new Date(2024, 4, 11, eHour, eMinute);

    const timeDiff = endTime.getTime() - startTime.getTime();
    const hoursDiff = Math.floor(timeDiff / (1000 * 3600));
    return hoursDiff;
}

function insertClassesIntoSchedule(classes, isTeacher) {
    cleanTable();

    $.each(classes, function (index, item)
    {
        //make the column visible for mobile
    /*    console.error($('.table-col-block[daynr="' + item.WeekDay.Id + '"]'))*/
        $('.table-col-block[daynr="'+item.WeekDay.Id+'"]').addClass("mobile-visible-col-block")

        //default structure of cell
        var insertAt = $(".table-cell-wrapper[day='" + item.WeekDay.Id + "'][hour='" + item.StartTime.Hours + "']");
        var mobileTime = $('<div></div>').addClass('table-mobile-interval-time').append(
            $('<span></span>').addClass('table-time-block-interval').text(formatTime(item.StartTime.Hours, item.StartTime.Minutes) + ' - ' + formatTime(item.EndTime.Hours, item.EndTime.Minutes))
        );

        insertAt.removeClass('table-empty-cell-hiddenmobile')

        //to only insert mobileTime once
        var hasTime = insertAt.find('div.table-mobile-interval-time')
        if (hasTime.length==0) {
            insertAt.append(mobileTime)
        } 

        var contentWrapper = $('<div></div>').addClass('table-inner-cell') 

        //compose general structure
        insertAt.append(contentWrapper)

        //identify cell span
        var hoursDiff = getTimeDiff(item.StartTime.Hours, item.StartTime.Minutes, item.EndTime.Hours, item.EndTime.Minutes);
        if (hoursDiff != 1) {
            insertAt.addClass('double-height')
            contentWrapper.addClass('table-simple-cell');
            $(".table-cell-wrapper[day='" + item.WeekDay.Id + "'][classnr='" + (+insertAt.attr('classnr') + 1) + "']").addClass('collapsed');   
        }

        var firstPair = $('<div></div>').addClass("mobile-itempair");
        var secondPair = $('<div></div>').addClass("mobile-itempair");

        var newSpan;
        if (isTeacher) {
            newSpan = $('<span></span>').addClass("inner-cell-profname").text(item.AcademicGroup.Name)
        } else {
            newSpan = $('<span></span>').addClass("inner-cell-profname").text(item.UserDiscipline.User.Surname[0] + '. ' + item.UserDiscipline.User.Name)
        }

        contentWrapper.append(
            firstPair.append(
                $('<span></span>').addClass("inner-cell-disciplinename").text(item.UserDiscipline.Type.TypeName[0].toLowerCase() +'. '+item.UserDiscipline.Discipline.ShortName),                
                newSpan
            ),
            secondPair.append(
                $('<span></span>').addClass("inner-cell-classroom").text(item.ClassRoom.ClassroomName)
            )
        );

        //frequency if necessary
        if (item.Frequency == 0) {
            contentWrapper.addClass('table-simple-cell');
        }else if (item.Frequency == 1) {            
            contentWrapper.append(
                secondPair.append(
                    $('<span></span>').addClass("inner-cell-frequency").text('par')
                )
            )
            contentWrapper.addClass('mobile-left-border');
            if (!$('#currentWeekFrequency').attr('even')) {
                contentWrapper.addClass("inner-disabled-lesson")
            }
        }else if(item.Frequency==2){
            contentWrapper.append(
                secondPair.append(
                    $('<span></span>').addClass("inner-cell-frequency").text('impar')
                )
            )
            contentWrapper.addClass('mobile-left-border');
            if ($('#currentWeekFrequency').attr('even')) {
                contentWrapper.addClass("inner-disabled-lesson")
            }
        }

        
        if ($('#currentWeekFrequency').attr('even')) {
            contentWrapper.addClass()
        }
    })
}

function insertOptionsInSelect(elementId ,jsonOptions, defaultOption) {
    $(elementId).empty();    
    $(elementId).append(defaultOption)
    

    $.each(jsonOptions, function (index, item) {
        $(elementId).append(
            $('<option></option>').val(item.Id).text(item.Name)
        );
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

function isCurrentWeekEven(saveInElement) {
    $.ajax({
        url: '/Home/IsCurrentWeekEven',
        method: 'GET',
        success: function (data) {
            saveInElement.append(
                $('<div id="currentWeekFrequency"></div>').attr('even', data)
            )            
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
}

class Weekday {
    constructor(name, dayNr) {
        this.name = name
        this.dayNr = dayNr;
    }
};

function initScheduleTable (){
    var table = $('#divTable');
    table.empty();

    isCurrentWeekEven(table)

    var timeList = [
        new TimeInterval(8, 0, 9, 30),
        new TimeInterval(9, 45, 11, 15),
        new TimeInterval(11, 30, 13, 0),
        new TimeInterval(13, 30, 15, 0),
        new TimeInterval(15, 15, 16, 45),
        new TimeInterval(17, 0, 18, 30),
        new TimeInterval(18, 45, 20, 15)
    ];
    var dayList = [
        new Weekday("empty", 0),
        new Weekday("Luni", 1),
        new Weekday("Marți", 2),
        new Weekday("Miercuri", 3),
        new Weekday("Joi", 4),
        new Weekday("Vineri", 5),
        new Weekday("Sâmbătă", 6)
    ];

    var timeColumn = $('<div></div>').addClass("table-time-block");
    timeColumn.append(
        $('<div></div>').addClass("table-time-block-empty").text(".")
    )
    $.each(timeList, function (index, interval) {
        var newDiv = $('<div></div>').addClass("table-time-block-cell")
        timeColumn.append(
            newDiv.append(
                $('<span></span>').addClass("table-time-block-interval").text(interval.formatString())
            )
        )

        if (index==0) {
            newDiv.addClass('top-left-round-corner');
        }
        if (index == 6) {
            newDiv.addClass('bottom-left-round-corner');
        }
    })  
    table.append(timeColumn);

    for (let col = 1; col < 7; col++) {
        var newCol = $('<div></div>').addClass("table-col-block").attr("daynr", col);

        for (let row = 0; row < 8; row++) {

            var tableCell = $('<div></div>').addClass("table-cell-wrapper");
            var div = $('<div></div>').addClass("table-cell-inner");
            tableCell.append(div);

            if (row == 0) {
                tableCell.removeClass("table-cell-wrapper");
                tableCell.addClass("table-header");
                div.removeClass("table-cell-inner")
                div.text(dayList[col].name);
                if (col == 1) {
                    tableCell.addClass("top-left-round-corner")
                }
                if (col == 6) {
                    tableCell.addClass("top-right-round-corner")
                }
            } else {
                tableCell.addClass("table-empty-cell-hiddenmobile")
                tableCell.attr("day", dayList[col].dayNr);
                tableCell.attr("hour", timeList[row - 1].startHour)
                tableCell.attr("classnr", row)
            }

            newCol.append(tableCell);
        }
        table.append(newCol)
    }
}