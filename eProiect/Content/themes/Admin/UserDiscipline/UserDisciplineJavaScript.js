/*window.onload = function () {

    var currentURL = window.location.pathname;

    if (currentURL.includes('Admin/UserDiscipline')) {
        getAllUsersDisciplines();
    }
}*/

function getAllUsersDisciplines() {

    $.ajax({
        url: '/Admin/GetAllUsersDisciplines',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            populateTable(data);
        },
        error: function (error) {
            console.error('Eroare la preluarea datelor:', error);
        }
    });
}

function populateTable(data) {
    var tbody = $('#tbody-UserDiscipline');
    tbody.empty(); // Golește corpul tabelului pentru a evita duplicarea datelor

    // Dicționar pentru a grupa datele după numele utilizatorilor
    var userGroups = {};

    data.forEach(function (item) {
        var userName = item.User.Name + ' ' + item.User.Surname;
        if (!userGroups[userName]) {
            userGroups[userName] = { userId: item.UserId, disciplines: {} };
        }
        if (!userGroups[userName].disciplines[item.Discipline.Name]) {
            userGroups[userName].disciplines[item.Discipline.Name] = [];
        }
        userGroups[userName].disciplines[item.Discipline.Name].push(item.Type.TypeName);
    });

    var rowIndex = 1;
    for (var userName in userGroups) {
        var userGroup = userGroups[userName];
        var userId = userGroup.userId;
        var disciplines = userGroup.disciplines;
        var firstRow = true;
        var rowspan = Object.keys(disciplines).length;

        for (var disciplineName in disciplines) {
            var types = disciplines[disciplineName].join(', ');
            var row = $('<tr id="' + userId + '">');

            if (firstRow) {
                var userCell = $('<td rowspan="' + rowspan + '">');


                var nameDiv = $('<div class="userDisciline-cell-div-name">').text(userName);
                var buttonDiv = $('<div class="userDisciline-cell-div-edit-buton">');
                var editButton = $('<button id="editUserDisciplineButton" data-user-id="' + userId + '" class="edit-lesson-button mod-lesson-button"><i class="ti-pencil"></i></button>');
                var tableCellDiv = $('<div>');

                buttonDiv.append(editButton);

                tableCellDiv.append(buttonDiv).append(nameDiv);

                userCell.append(tableCellDiv);
                row.append($('<td rowspan="' + rowspan + '">').text(rowIndex));
                row.append(userCell);
                firstRow = false;
            }

            row.append($('<td>').text(disciplineName));
            row.append($('<td>').text(types));
            tbody.append(row);
        }

        rowIndex++;
    }
}

//======================================================================================================

$(document).ready(function () {
    var currentURL = window.location.pathname;

    if (currentURL.includes('Admin/UserDiscipline')) {
        $('#container').on('click', '#userDisciplineID', function (event) {
            var userDisciplineId = $(this).data('userdisciplineid');
            postDeleteUserDisciplineById(userDisciplineId);

        });

        $('#tbody-UserDiscipline').on('click', '.edit-lesson-button', function () {
            var userId = $(this).data('user-id');

            $.ajax({
                url: '/Admin/GetDiscilineByUserId',
                method: 'GET',
                data: { userId: userId },
                success: function (response) {
                    toggleCardBlocks(2)
                    createDisciplineDivs(response);
                },
                error: function (jqXHR) {
                    console.error(jqXHR);
                }
            });
        });



    }
});

function postDeleteUserDisciplineById(userDisciplineId) {
    var spanError = document.getElementById('error_span_edit');
    $.ajax({
        url: '/Admin/DeleteUserDsiciplineByUserDisciplineId',
        method: 'POST',
        data: { userDisciplineId: userDisciplineId },
        success: function (response) {
            if (!response.success) {
                spanError.classList.add('j-error-message');
                spanError.textContent = response.message;
                spanError.style.display = 'block';
            } else {
                spanError.classList.remove('j-error-message');
                spanError.classList.add('j-success-message');
                spanError.style.display = 'block';
                spanError.textContent = response.message;
                setTimeout(function () {
                    spanError.classList.remove('j-success-message');

                    spanError.style.display = 'none';
                }, 3000);
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error sending data to the backend:', jqXHR, textStatus);
            spanError.classList.add('j-error-message');
            spanError.textContent = errorThrown;
            spanError.style.display = 'block';
        }
    });
}


//----------------------------------------------------------------------------------------------------------------


// click Renunta 
function cancellationEditUserDiscipline(typeOfCard) {
    getAllUsersDisciplines();
    toggleCardBlocks(typeOfCard);
}

function toggleCardBlocks(typeOfCard) {
    const cardBlock1 = document.getElementById('cardblock1');
    const cardBlock2 = document.getElementById('cardblock2');
    const cardBlock3 = document.getElementById('cardblock3');

    if (!cardBlock1 || !cardBlock2 || !cardBlock3) {
        console.error('One or both card blocks not found.');
        return;
    }

    if (1 == typeOfCard) {
        cardBlock1.style.display = 'block';
        cardBlock2.style.display = 'none';
        cardBlock3.style.display = 'none';
    }
    if (2 == typeOfCard) {
        cardBlock1.style.display = 'none';
        cardBlock2.style.display = 'block';
        cardBlock3.style.display = 'none';
    }
    if (3 == typeOfCard) {
        cardBlock1.style.display = 'none';
        cardBlock2.style.display = 'none';
        cardBlock3.style.display = 'block';
    }
}


function createDisciplineDivs(disciplines) {
    var userNameSpan = $('#UserName');
    userNameSpan.text(disciplines[0].User.Name + ' ' + disciplines[0].User.Surname);
    const userNameSpanDataValue = document.getElementById('UserName');
    userNameSpanDataValue.setAttribute('data-userId', disciplines[0].User.Id)
    console.error(disciplines[0].User.Name);

    const container = document.getElementById('container');
    containerElement = $('#container');
    containerElement.empty();
    disciplines.forEach(discipline => {
        // Create a new div
        const div = document.createElement('div');
        div.className = 'j-row toclone-widget-right toclone cloneya';
        div.id = discipline.Id;

        // Create the first select for discipline name
        const disciplineNameSelect = document.createElement('select');
        disciplineNameSelect.id = 'disciplineName';
        disciplineNameSelect.className = 'form-select';
        disciplineNameSelect.disabled = true;
        const optionName = document.createElement('option');
        optionName.selected = true;
        optionName.textContent = discipline.Discipline.ShortName;
        optionName.value = discipline.Discipline.Id
        disciplineNameSelect.appendChild(optionName);

        // Create the second select for discipline type
        const disciplineTypeSelect = document.createElement('select');
        disciplineTypeSelect.id = 'disciplineType';
        disciplineTypeSelect.className = 'form-select';
        disciplineTypeSelect.disabled = true;
        const optionType = document.createElement('option');
        optionType.selected = true;
        optionType.textContent = discipline.Type.TypeName;
        optionType.value = discipline.Type.Id
        disciplineTypeSelect.appendChild(optionType);

        // Add the selects to the div
        const span6Unit1 = document.createElement('div');
        span6Unit1.className = 'span6 unit';
        span6Unit1.appendChild(disciplineNameSelect);
        div.appendChild(span6Unit1);

        const span6Unit2 = document.createElement('div');
        span6Unit2.className = 'span6 unit';
        span6Unit2.appendChild(disciplineTypeSelect);
        div.appendChild(span6Unit2);

        // Add buttons (assuming you need these as in the example)
        const addButton = document.createElement('button');
        addButton.type = 'button';
        addButton.id = 'addButton';
        addButton.onclick = getAllDisciplines;
        addButton.className = 'btn btn-primary clone-btn-right ';
        addButton.innerHTML = '<i class="icofont icofont-plus"></i>';

        div.appendChild(addButton);

        const deleteButton = document.createElement('button');
        deleteButton.type = 'button';
        deleteButton.id = 'userDisciplineID';
        deleteButton.setAttribute('data-userDisciplineId', discipline.Id);
        deleteButton.className = 'btn btn-default clone-btn-right delete userDiscipline';
        deleteButton.innerHTML = '<i class="icofont icofont-minus"></i>';
        div.appendChild(deleteButton);

        // Append the created div to the container
        container.appendChild(div);
    });
}

let fetchedDisciplines = null;
// Function to fetch all disciplines
function getAllDisciplines() {
    if (fetchedDisciplines === null) {
        $.ajax({
            url: '/Admin/GetAllDiscipline',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                fetchedDisciplines = data;
                console.error(data);
                populateDisciplineSelect(fetchedDisciplines);
            },
            error: function (jqXHR) {
                console.error(jqXHR);
            }
        });
    } else {
        populateDisciplineSelect(fetchedDisciplines);
    }
}
function populateDisciplineSelect(data) {
    const container = document.getElementById('container');
    containerElement = $('#container');

    // Create a new div
    const div = document.createElement('div');
    div.className = 'j-row toclone-widget-right toclone cloneya';


    // Create the first select for discipline name
    const disciplineNameSelect = document.createElement('select');
    disciplineNameSelect.id = 'disciplineName';
    disciplineNameSelect.className = 'form-select';


    const optionName = document.createElement('option');
    optionName.textContent = 'Disciplina';
    optionName.value = 0;
    optionName.selected = true;
    disciplineNameSelect.appendChild(optionName);

    data.forEach(discipline => {

        const optionName = document.createElement('option');
        optionName.textContent = discipline.ShortName;
        optionName.value = discipline.Id;
        disciplineNameSelect.appendChild(optionName);
    });

    // Create the second select for discipline type
    const disciplineTypeSelect = document.createElement('select');
    disciplineTypeSelect.id = 'disciplineType';
    disciplineTypeSelect.className = 'form-select';

    const ClassType = Object.freeze({
        Curs: 1,
        Seminar: 2,
        Laborator: 3
    });

    const optionType = document.createElement('option');
    optionType.textContent = 'Tipul disciplinei';
    optionName.selected = true;
    optionType.value = 0;
    disciplineTypeSelect.appendChild(optionType);

    Object.entries(ClassType).forEach(([key, value]) => {

        const optionType = document.createElement('option');
        optionType.textContent = key;
        optionType.value = value;
        disciplineTypeSelect.appendChild(optionType);
    });

    // Add the selects to the div
    const span6Unit1 = document.createElement('div');
    span6Unit1.className = 'span6 unit';
    span6Unit1.appendChild(disciplineNameSelect);
    div.appendChild(span6Unit1);

    const span6Unit2 = document.createElement('div');
    span6Unit2.className = 'span6 unit';
    span6Unit2.appendChild(disciplineTypeSelect);
    div.appendChild(span6Unit2);

    // Add buttons (assuming you need these as in the example)
    const addButton = document.createElement('button');
    addButton.type = 'button';
    addButton.id = 'addButton';
    addButton.onclick = getAllDisciplines;
    addButton.className = 'btn btn-primary clone-btn-right';
    addButton.innerHTML = '<i class="icofont icofont-plus"></i>';
    div.appendChild(addButton);

    const deleteButton = document.createElement('button');
    deleteButton.type = 'button';

    deleteButton.className = 'btn btn-default clone-btn-right delete';
    deleteButton.innerHTML = '<i class="icofont icofont-minus"></i>';
    div.appendChild(deleteButton);

    // Append the created div to the container
    container.appendChild(div);

}

//--------------------------------------------------------------------------------------------------------------- 

function ScanDisciplineSelections() {
    const existingDisciplineList = [];

    const userIdFromSpan = document.getElementById("UserName").dataset.userid;

    const disciplineDisabledNameSelector = document.querySelectorAll(
        'select[id="disciplineName"][disabled]'
    );
    const disciplineDisabledTypeSelects = document.querySelectorAll(
        'select[id="disciplineType"][disabled]'
    );

    const disciplineNameSelects = document.querySelectorAll(
        'select[id="disciplineName"]:not([disabled])'
    );
    const disciplineTypeSelects = document.querySelectorAll(
        'select[id="disciplineType"]:not([disabled])'
    );

    for (let i = 0; i < disciplineDisabledNameSelector.length; i++) {

        const DisciplineId = disciplineDisabledNameSelector[i].value;
        const DisciplineTypeId = disciplineDisabledTypeSelects[i].value;
        existingDisciplineList.push({ DisciplineId, DisciplineTypeId });
    }

    if (disciplineNameSelects.length !== disciplineTypeSelects.length) {
        throw new Error("Număr inegal de selecții pentru nume și tip disciplină.");
    }

    const disciplineList = [];
    for (let i = 0; i < disciplineNameSelects.length; i++) {
        const UserId = userIdFromSpan;
        const DisciplineId = disciplineNameSelects[i].value;
        const DisciplineTypeId = disciplineTypeSelects[i].value;



        if ((0 == DisciplineId) || (0 == DisciplineTypeId)) {
            continue;
        }

        const existingEntry = existingDisciplineList.find(
            (entry) => entry.DisciplineId == DisciplineId && entry.DisciplineTypeId == DisciplineTypeId
        );
        const existingEntryInUserInput = disciplineList.find(
            (entry) => entry.DisciplineId == DisciplineId && entry.DisciplineTypeId == DisciplineTypeId
        );
        if (!existingEntry && !existingEntryInUserInput) {
            disciplineList.push({ UserId, DisciplineId, DisciplineTypeId });
        }
    }
    return disciplineList;
}

function disableSelectInput() {

    const disciplineNameSelects = document.querySelectorAll(
        'select[id="disciplineName"]:not([disabled])'
    );
    const disciplineTypeSelects = document.querySelectorAll(
        'select[id="disciplineType"]:not([disabled])'
    );
    for (let i = 0; i < disciplineNameSelects.length; i++) {
        disciplineNameSelects[i].disabled = true;
        disciplineTypeSelects[i].disabled = true;
    }

}

function postUserDisciplines() {
    var spanError = document.getElementById('error_span_edit');
    $.ajax({
        url: '/Admin/AddUserDiscipline',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ScanDisciplineSelections()),
        dataType: "json",
        success: function (response) {
            if (!response.success) {
                spanError.classList.add('j-error-message');
                spanError.textContent = response.message;
                spanError.style.display = 'block';
            } else {

                disableSelectInput();
                spanError.classList.remove('j-error-message');
                spanError.classList.add('j-success-message');
                spanError.style.display = 'block';
                spanError.textContent = response.message;
                setTimeout(function () {
                    spanError.classList.remove('j-success-message');

                    spanError.style.display = 'none';
                }, 3000);
            }

        },
        error: function (xhr, status, error) {
            console.error('Error sending data to the backend:', error);
            spanError.classList.add('j-error-message');
            spanError.textContent = error;
            spanError.style.display = 'block';
        }
    });
}

//----------Add new user to discipline--------

function addNewUserToDiscipline() {
    containerElement = $('#container_ForNewUser');
    containerElement.empty();
    loadUsers();
    getAllDisciplinesForNewUserDisciplines();
    toggleCardBlocks(3);
}


function filterFunction() {
    const input = document.getElementById("nameInput");
    const filter = input.value.toUpperCase();
    const div = document.getElementById("teachersDropdown");
    const spans = div.getElementsByClassName("dropdown-span");

    for (let i = 0; i < spans.length; i++) {
        const txtValue = spans[i].textContent || spans[i].innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            spans[i].style.display = "";
        } else {
            spans[i].style.display = "none";
        }
    }
}

function loadUsers() {
    $.ajax({
        url: "/Admin/GetAllUserEsentialsData",
        method: "GET",
        success: function (data) {
            InsertTeachersInDropdown(data);
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
            $('<div></div>').val(teacher.Id).addClass('pl-2 dropdown-span').attr('id', 'spanOptionTeacher').append(
                $('<span></span>').text(teacher.Name + ' ' + teacher.Surname)
            )
        );
    });
}

let TeacherId;
$(document).ready(function () {

    // Optional: Close the dropdown if clicking outside
    $('#searchDropdown').click(function (event) {
        if (event.target.id == 'spanOptionTeacher') {

            TeacherId = $(event.target).val();
            $('#nameInput').toggleClass('opened-input');
            $('#searchIconBox').toggleClass('closed');

            $('#teachersDropdown').toggleClass('closed');
            $('#nameInput').val($(event.target).text())
        }
    });

    $('#nameInput').on("focus", function () {
        $(this).toggleClass('opened-input');
        $('#searchIconBox').toggleClass('closed');
        $('#teachersDropdown').toggleClass('closed');
    });

});

function ScanDisciplineSelectionsForNewUser() {
    const userIdFromSpan = TeacherId;

    const disciplineNameSelects = document.querySelectorAll(
        'select[id="disciplineNameForNewUser"]:not([disabled])'
    );

    const disciplineTypeSelects = document.querySelectorAll(
        'select[id="disciplineTypeForNewUser"]:not([disabled])'
    );
    if (disciplineNameSelects.length !== disciplineTypeSelects.length) {
        throw new Error("Număr inegal de selecții pentru nume și tip disciplină.");
    }

    const disciplineList = [];
    for (let i = 0; i < disciplineNameSelects.length; i++) {
        const UserId = userIdFromSpan;
        const DisciplineId = disciplineNameSelects[i].value;
        const DisciplineTypeId = disciplineTypeSelects[i].value;

        if ((0 == DisciplineId) || (0 == DisciplineTypeId)) {
            continue;
        }


        const existingEntryInUserInput = disciplineList.find(
            (entry) => entry.DisciplineId == DisciplineId && entry.DisciplineTypeId == DisciplineTypeId
        );
        if (!existingEntryInUserInput) {
            disciplineList.push({ UserId, DisciplineId, DisciplineTypeId });
        }
    }
    console.error(disciplineList);
    return disciplineList;
}

function getAllDisciplinesForNewUserDisciplines() {
    if (fetchedDisciplines === null) {
        $.ajax({
            url: '/Admin/GetAllDiscipline',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                fetchedDisciplines = data;
                populateDisciplineSelectInNewUser(fetchedDisciplines);
            },
            error: function (jqXHR) {
                console.error(jqXHR);
            }
        });
    } else {
        populateDisciplineSelectInNewUser(fetchedDisciplines);
    }
}

function populateDisciplineSelectInNewUser(data) {

    const container = document.getElementById('container_ForNewUser');
    containerElement = $('#container_ForNewUser');

    // Create a new div
    const div = document.createElement('div');
    div.className = 'j-row toclone-widget-right toclone cloneya';


    // Create the first select for discipline name
    const disciplineNameSelect = document.createElement('select');
    disciplineNameSelect.id = 'disciplineNameForNewUser';
    disciplineNameSelect.className = 'form-select';


    const optionName = document.createElement('option');
    optionName.textContent = 'Disciplina';
    optionName.value = 0;
    optionName.selected = true;
    disciplineNameSelect.appendChild(optionName);

    data.forEach(discipline => {

        const optionName = document.createElement('option');
        optionName.textContent = discipline.ShortName;
        optionName.value = discipline.Id;
        disciplineNameSelect.appendChild(optionName);
    });

    // Create the second select for discipline type
    const disciplineTypeSelect = document.createElement('select');
    disciplineTypeSelect.id = 'disciplineTypeForNewUser';
    disciplineTypeSelect.className = 'form-select';

    const ClassType = Object.freeze({
        Curs: 1,
        Seminar: 2,
        Laborator: 3
    });

    const optionType = document.createElement('option');
    optionType.textContent = 'Tipul disciplinei';
    optionName.selected = true;
    optionType.value = 0;
    disciplineTypeSelect.appendChild(optionType);

    Object.entries(ClassType).forEach(([key, value]) => {

        const optionType = document.createElement('option');
        optionType.textContent = key;
        optionType.value = value;
        disciplineTypeSelect.appendChild(optionType);
    });

    // Add the selects to the div
    const span6Unit1 = document.createElement('div');
    span6Unit1.className = 'span6 unit';
    span6Unit1.appendChild(disciplineNameSelect);
    div.appendChild(span6Unit1);

    const span6Unit2 = document.createElement('div');
    span6Unit2.className = 'span6 unit';
    span6Unit2.appendChild(disciplineTypeSelect);
    div.appendChild(span6Unit2);

    // Add buttons (assuming you need these as in the example)
    const addButton = document.createElement('button');
    addButton.type = 'button';
    addButton.id = 'addButton';
    addButton.onclick = getAllDisciplinesForNewUserDisciplines;
    addButton.className = 'btn btn-primary clone-btn-right';
    addButton.innerHTML = '<i class="icofont icofont-plus"></i>';
    div.appendChild(addButton);

    const deleteButton = document.createElement('button');
    deleteButton.type = 'button';

    deleteButton.className = 'btn btn-default clone-btn-right delete';
    deleteButton.innerHTML = '<i class="icofont icofont-minus"></i>';
    div.appendChild(deleteButton);

    // Append the created div to the container
    container.appendChild(div);

}

function postNewUserDisciplinesForNewThec() {
    var spanError = document.getElementById('error_new_user');
    $.ajax({
        url: '/Admin/AddUserDiscipline',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ScanDisciplineSelectionsForNewUser(TeacherId)),
        dataType: "json",
        success: function (response) {
            if (!response.success) {
                spanError.classList.add('j-error-message');
                spanError.textContent = response.message;
                spanError.style.display = 'block';
            }
            else {
                getAllUsersDisciplines();
                toggleCardBlocks(1);
                spanError.classList.remove('j-error-message');
                spanError.classList.add('j-success-message');
                spanError.style.display = 'block';
                spanError.textContent = response.message;
                setTimeout(function () {
                    spanError.classList.remove('j-success-message');

                    spanError.style.display = 'none';
                }, 3000);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error sending data to the backend:', error);
            spanError.classList.add('j-error-message');
            spanError.textContent = error;
            spanError.style.display = 'block';
        }
    });
}


