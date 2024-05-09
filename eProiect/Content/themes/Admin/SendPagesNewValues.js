

document.addEventListener('DOMContentLoaded', function () {
     var firstNameInput = document.getElementById('name');
     var lastNameInput = document.getElementById('surname');
     var emailInput = document.getElementById('email');
     var dateInput = document.querySelector('input[type="date"]');
     var selectInput = document.querySelector('select[name="select"]');

     // Ascultător pentru nume
     firstNameInput.addEventListener('input', function () {
          var parentDiv = firstNameInput.parentElement;
          if (!_minLengthCheck(firstNameInput.value, 3) && _checkForDigits(firstNameInput.value)) {

               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');

          }
     });

     // Ascultător pentru prenume
     lastNameInput.addEventListener('input', function () {
          var parentDiv = lastNameInput.parentElement;
          if (!_minLengthCheck(lastNameInput.value, 3) && _checkForDigits(lastNameInput.value)) {
               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');
          }
     });

     // Ascultător pentru email
     emailInput.addEventListener('input', function () {
          var parentDiv = emailInput.parentElement;
          if (_emailCheck(emailInput.value)) {
               parentDiv.classList.add('j-error-view');
          } else {
               parentDiv.classList.remove('j-error-view');
          }
     });

     // Ascultător pentru data
     dateInput.addEventListener('input', function () {
          var parentDiv = dateInput.parentElement;
          if (dateInput.value.trim() === '') {
               parentDiv.classList.add('j-error-view');
          } else {
               parentDiv.classList.remove('j-error-view');
          }
     });

     // Ascultător pentru select
     selectInput.addEventListener('input', function () {
          var parentDiv = selectInput.parentElement;
          if (selectInput.value === 'opt1') {
               parentDiv.classList.add('j-error-view');
          } else {
               parentDiv.classList.remove('j-error-view');
          }
     });


});


function send() {

     var token = $('input[name="__RequestVerificationToken"]').val();
     // Send AJAX request to server
     var userData = {
          Name: document.getElementById('name').value,
          Surname: document.getElementById('surname').value,
          Level: document.querySelector('select[name="select"]').value,
          Email: document.getElementById('email').value
     };

     $.ajax({
          url: '/Admin/AddUser',
          type: 'POST',
          contentType: 'application/json',
          data: JSON.stringify(userData),
          headers: {
               // Include the anti-forgery token in the request headers
               "__RequestVerificationToken": token
          },
          dataType: "json",
          success: function (response) {
               if (!response.success) {
                    var spanError = document.getElementById('error_span');

                    spanError.classList.add('j-error-message');
                    if (response.message === 'Inner Exception: An error occurred while updating the entries. See the inner exception for details.') {

                         var emailInput = document.getElementById('email');
                         var parentDiv = emailInput.parentElement;
                         parentDiv.classList.add('j-error-view');


                         spanError.textContent = 'There is already a user with this email address.';
                         spanError.style.display = '';

                    } else {
                         spanError.textContent = response.message;
                         spanError.style.display = '';
                    }
               } else {

                    var form = document.getElementById('j-pro');
                    var inputElements = form.getElementsByTagName('input');
                    var selectorElement = document.getElementById('role');
                    selectorElement.value = 'opt1';

                    for (var i = 0; i < inputElements.length; i++) {
                         var parentDiv = inputElements[i].parentElement;
                         parentDiv.classList.remove('j-error-view');
                         inputElements[i].value = inputElements[i].defaultValue;
                    }

                    var spanError = document.getElementById('error_span');
                    spanError.classList.remove('j-error-message');
                    spanError.classList.add('j-success-message');
                    spanError.style.display = 'block';
                    spanError.textContent = response.message;
                    setTimeout(function () {
                         spanError.classList.remove('j-success-message');
                         spanError.textContent = '';
                         spanError.style.display = 'none';
                    }, 3000);

               }


          },
          error: function (xhr, status, error) {
               console.error('Error sending data to the backend:', error);
               var spanError = document.getElementById('error_span');

               spanError.classList.add('j-error-message');
               spanError.textContent = error;
               spanError.style.display = '';

          }
     });


}


document.addEventListener('DOMContentLoaded', function () {
     var nameInput = document.getElementById('diciplineName');
     var shortNameInput = document.getElementById('shortName');

     // Listener for name input
     nameInput.addEventListener('input', function () {
          var parentDiv = nameInput.parentElement;
          if (!_minLengthCheck(nameInput.value, 3)) {
               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');
          }
     });

     // Listener for short name input
     shortNameInput.addEventListener('input', function () {
          var parentDiv = shortNameInput.parentElement;
          if (!_minLengthCheck(shortNameInput.value, 2)) {
               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');
          }
     });

});

function send_discipline() {
     // Send AJAX request to server
     var disciplineData = {
          Name: document.getElementById('diciplineName').value,
          ShortName: document.getElementById('shortName').value
     };

     $.ajax({
          url: '/Admin/AddDiscipline',
          type: 'POST',
          contentType: 'application/json',
          data: JSON.stringify(disciplineData),
          success: function (response) {
               if (!response.success) {
                    // Handle error response

                    var spanError = document.getElementById('error_span');
                    spanError.classList.add('j-error-message');
                    if (response.message === 'Inner Exception: An error occurred while updating the entries. See the inner exception for details.') {
                         spanError.textContent = 'There is already a discipline with these paraperers.';
                         spanError.style.display = 'block';
                    }
                    else {
                         spanError.textContent = response.message;
                         spanError.style.display = 'block';

                    }
               } else {
                    // Handle success response
                    var form = document.getElementById('j-pro');
                    var inputElements = form.getElementsByTagName('input');

                    for (var i = 0; i < inputElements.length; i++) {
                         var parentDiv = inputElements[i].parentElement;
                         parentDiv.classList.remove('j-error-view');
                         inputElements[i].value = inputElements[i].defaultValue;
                    }

                    var spanError = document.getElementById('error_span');
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
               var spanError = document.getElementById('error_span');

               spanError.classList.add('j-error-message');
               spanError.textContent = error;
               spanError.style.display = 'block';
          }
     });
}

document.addEventListener('DOMContentLoaded', function () {
     var nameInput = document.getElementById('academicGroupName');
     var selectInput = document.querySelector('select[name="select"]');

     // Listener for name input
     nameInput.addEventListener('input', function () {
          var parentDiv = nameInput.parentElement;
          if (!_minLengthCheck(nameInput.value, 3)) {
               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');
          }
     });
     selectInput.addEventListener('input', function () {
          var parentDiv = selectInput.parentElement;
          if (selectInput.value === 'opt1') {
               parentDiv.classList.add('j-error-view');
          } else {
               parentDiv.classList.remove('j-error-view');
          }
     });


});

function send_academicGroup() {
     // Send AJAX request to server
     var academicGroupData = {
          Name: document.getElementById('academicGroupName').value,
          Year: document.querySelector('select[name="select"]').value
     };

     $.ajax({
          url: '/Admin/AddAcademicGroup',
          type: 'POST',
          contentType: 'application/json',
          data: JSON.stringify(academicGroupData),
          success: function (response) {
               if (!response.success) {
                    // Handle error response

                    var spanError = document.getElementById('error_span');
                    spanError.classList.add('j-error-message');
                    if (response.message === 'Inner Exception: An error occurred while updating the entries. See the inner exception for details.') {
                         spanError.textContent = 'There is already a academinc group with this name.';
                         spanError.style.display = 'block';
                    }
                    else {
                         spanError.textContent = response.message;
                         spanError.style.display = 'block';

                    }
               } else {
                    // Handle success response
                    var form = document.getElementById('j-pro');
                    var inputElements = form.getElementsByTagName('input');
                    var selectorElement = document.getElementById('academicGroupYear');
                    selectorElement.value = 'opt1';
                    for (var i = 0; i < inputElements.length; i++) {
                         var parentDiv = inputElements[i].parentElement;
                         parentDiv.classList.remove('j-error-view');
                         inputElements[i].value = inputElements[i].defaultValue;
                    }

                    var spanError = document.getElementById('error_span');
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
               var spanError = document.getElementById('error_span');
               spanError.classList.add('j-error-message');
               spanError.textContent = error;
               spanError.style.display = 'block';
          }
     });
}

document.addEventListener('DOMContentLoaded', function () {
     var nameInput = document.getElementById('classRoomName');
     var selectInput = document.querySelector('select[name="select"]');

     // Listener for name input
     nameInput.addEventListener('input', function () {
          var parentDiv = nameInput.parentElement;
          if (!_minLengthCheck(nameInput.value, 3)) {
               parentDiv.classList.remove('j-error-view');
          } else {
               parentDiv.classList.add('j-error-view');
          }
     });
     selectInput.addEventListener('input', function () {
          var parentDiv = selectInput.parentElement;
          if (selectInput.value === 'opt1') {
               parentDiv.classList.add('j-error-view');
          } else {
               parentDiv.classList.remove('j-error-view');
          }
     });


});

function send_classRoom() {
     // Send AJAX request to server
     var classRommData = {
          ClassroomName: document.getElementById('classRoomName').value,
          Floor: document.querySelector('select[name="select"]').value
     };

     $.ajax({
          url: '/Admin/AddClassRoom',
          type: 'POST',
          contentType: 'application/json',
          data: JSON.stringify(classRommData),
          success: function (response) {
               if (!response.success) {
                    // Handle error response

                    var spanError = document.getElementById('error_span');
                    spanError.classList.add('j-error-message');
                    if (response.message === 'Inner Exception: An error occurred while updating the entries. See the inner exception for details.') {
                         spanError.textContent = 'There is already a class room with this name.';
                         spanError.style.display = 'block';
                    }
                    else {
                         spanError.textContent = response.message;
                         spanError.style.display = 'block';

                    }
               } else {
                    // Handle success response
                    var form = document.getElementById('j-pro');
                    var inputElements = form.getElementsByTagName('input');
                    var selectorElement = document.getElementById('classRoomFloor');
                    selectorElement.value = 'opt1';
                    for (var i = 0; i < inputElements.length; i++) {
                         var parentDiv = inputElements[i].parentElement;
                         parentDiv.classList.remove('j-error-view');
                         inputElements[i].value = inputElements[i].defaultValue;
                    }

                    var spanError = document.getElementById('error_span');
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
               var spanError = document.getElementById('error_span');
               spanError.classList.add('j-error-message');
               spanError.textContent = error;
               spanError.style.display = 'block';
          }
     });
}
