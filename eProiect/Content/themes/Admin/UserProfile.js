window.onload = function () {

    var currentURL = window.location.pathname;

    if (currentURL.includes('Admin/UserProfile')) {
        getUserProfile();
    }

    if (currentURL.includes('Admin/UserDiscipline')) {
        getAllUsersDisciplines();
    }
}
function convertCSharpDateTimeToJson(csharpDateTimeString) {
    // Extract the milliseconds since epoch
    const millisecondsSinceEpoch = parseInt(csharpDateTimeString.match(/-?\d+/)[0]);
    const birthday = new Date(millisecondsSinceEpoch);
    const isoDateString = birthday.toISOString();

    return isoDateString;
}

function populateUserProfileData(userProfileData) {
    document.getElementById('name').textContent = userProfileData.Name;
    document.getElementById('surname').textContent = userProfileData.Surname;
    const birthDate = convertCSharpDateTimeToJson(userProfileData.Birthday);
    const d = new Date(birthDate);

    document.getElementById('birthDate').textContent = d.toLocaleDateString();
    document.getElementById('email').innerHTML = `<a href="mailto:${userProfileData.Email}">${userProfileData.Email}</a>`;
}
function getUserProfile() {

    $.ajax({
        url: '/Admin/GetUserProfileData',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            populateUserProfileData(data);
        },
        error: function (error) {
            console.error('Eroare la preluarea datelor:', error);
        }
    });
}


document.getElementById('edit-btn').addEventListener('click', function () {
    // Obțineți valorile din tabel
    var name = document.getElementById('name').innerText.trim();
    var surname = document.getElementById('surname').innerText.trim();
    var birthDate = document.getElementById('birthDate').innerText.trim();
    var email = document.getElementById('email').innerText.trim();
    document.getElementById('profile').style.display = 'none';
    document.getElementById('edit_profile').style.display = 'block';
    // Completați valorile în câmpurile de intrare
    document.querySelector('.edit-info input[placeholder="Name"]').value = name;
    document.querySelector('.edit-info input[placeholder="Surname"]').value = surname;
    var formattedDate = new Date(birthDate);
    var year = formattedDate.getFullYear();
    var month = (formattedDate.getMonth() + 1).toString().padStart(2, '0');
    var day = formattedDate.getDate().toString().padStart(2, '0');
    var formattedDateString = year + '-' + month + '-' + day;
    document.getElementById('dropper-default').value = formattedDateString;
    document.querySelector('.edit-info input[placeholder="Email"]').value = email;
});

document.getElementById('edit-cancel').addEventListener('click', function () {

    document.getElementById('profile').style.display = 'block';
    document.getElementById('edit_profile').style.display = 'none';

});

document.getElementById('save-profil').addEventListener('click', function () {
    var userProfile = {
        "Name": document.querySelector('.edit-info input[placeholder="Name"]').value,
        "Surname": document.querySelector('.edit-info input[placeholder="Surname"]').value,
        "Email": document.querySelector('.edit-info input[placeholder="Email"]').value,
        "Birthday": document.getElementById('dropper-default').value
    };
    $.ajax({
        url: '/Admin/PostUserProfileData',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userProfile),
        dataType: "json",
        success: function (response) {
            if (!response.success) {
                console.error(response);
            } else {
                document.getElementById('profile').style.display = 'block';
                document.getElementById('edit_profile').style.display = 'none';
                document.getElementById('name').textContent = userProfile.Name;
                document.getElementById('surname').textContent = userProfile.Surname;
                document.getElementById('birthDate').textContent = userProfile.Birthday;
                document.getElementById('email').innerHTML = `<a href="mailto:${userProfile.Email}">${userProfile.Email}</a>`;
            }
        },
        error: function (xhr, status, error) {
            console.error('Error sending data to the backend:', error);
            // Handle error gracefully
        }
    });
});

function sendChangeUserPassword() {
    var changeUserPasswordViewModel = {
        OldPassword: document.getElementById('oldPassword').value,
        NewPassword: document.getElementById('newPassword').value,
        ConfirmPassword: document.getElementById('newConfirmPassword').value
    };
    $.ajax({
        url: '/Admin/PostUserChangePassword',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(changeUserPasswordViewModel),
        dataType: "json",
        success: function (response) {
            var spanError = document.getElementById('errorspanpass');
            if (!response.success) {
                spanError.classList.add('j-error-message');
                spanError.textContent = response.message; // Display the error message
                spanError.style.display = 'block'; // Make sure the error message is displayed
                setTimeout(function () {
                    spanError.textContent = '';
                    spanError.style.display = 'none';

                }, 3000);

            } else {
                spanError.classList.remove('j-error-message');
                spanError.classList.add('j-success-message');
                spanError.textContent = response.message; // Display the success message
                spanError.style.display = 'block'; // Make sure the success message is displayed
                setTimeout(function () {
                    spanError.classList.remove('j-success-message');
                    spanError.style.display = 'none';
                    spanError.textContent = '';
                }, 3000);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error sending data to the backend:', error);
            var spanError = document.getElementById('errorspanpass');
            spanError.classList.add('j-error-message');
            spanError.textContent = 'An error occurred: ' + error;
            spanError.style.display = 'block'; // Make sure the error message is displayed
        }
    });
}
