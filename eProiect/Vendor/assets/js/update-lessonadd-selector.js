
$(document).ready(function () {
    $(".plus-button").click(function () {
        var dataHour = $(this).data("positionHour");
        var dataMinutes = $(this).data("positionMinutes");
        var dataWeekdayNr = $(this).data("positionWeekdayNr");

        $.ajax({
            url: "/Home/AddLesson",
            type: "POST",
            data: { sHour: dataHour, sMinutes: dataMinutes, sDay: dataWeekdayNr },
            success: function (response) {
                $("#partial-view-container").html(response);
            },
            error: function (error) {
                console.error("Error sending data to route '/Home/AddLesson':", error);
            }
        });

        loadDisciplineSelect();
    });

    $("#static-container").click(function (event) {            
        if (event.target.id === "yearSelector") {
            var selectedYear = $(event.target).val();

            if (selectedYear != null) {
                $.ajax({
                    url: "/Home/GetOptionsByYear", 
                    method: "POST",
                    data: { year: selectedYear },
                    success: function (data) {
                        // Update the second select options based on the response (data)
                        updateGroupSelect(data);
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.error("AJAX Error:", textStatus, errorThrown);
                        console.error("Full jqXHR object:", jqXHR);
                    }
                });
            }
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

function updateDisciplineSelect(data) {
    $('#disciplineSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("disciplina");
    $('#disciplineSelector').append(defaultOption);
    $.each(data, function (index, item) {
        var option = $('<option></option>').val(item.Discipline.Id).text(item.Discipline.Name);
        $('#disciplineSelector').append(option);
    });
}

function updateDisciplineTypeSelect(data) {

    $('#typeSelector').empty();
    var defaultOption = $('<option disabled selected></option>').text("tipul");

    $('#typeSelector').append(defaultOption);
     $.each(data, function (index, item) {
         var option = $('<option></option>').val(item.ClassTypeId).text(item.Type.TypeName);
         $('#typeSelector').append(option);
    });

}

function updateGroupSelect(data) {
    // Clear existing options
    $('#groupSelector').empty();

    var defaultOption = $('<option disabled selected></option>').text("grupa");
    $("#groupSelector").append(defaultOption)

    // Add options based on the received data
    $.each(data, function (index, item) {
        var option = $('<option></option>').val(item["Year"]).text(item["Name"]);

        $('#groupSelector').append(option);
    });
}