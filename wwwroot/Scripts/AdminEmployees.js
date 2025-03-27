window.addEventListener("DOMContentLoaded", async () => {

    const employeeCreationButton = document.getElementById('employeeCreationButton');
    const employeeCreationForm = document.getElementById('employeeCreationForm');
    employeeCreationButton.addEventListener('click', function () {
        employeeCreationForm.classList.toggle('hidden');

        const buttonRect = employeeCreationButton.getBoundingClientRect();
        const formRect = employeeCreationForm.getBoundingClientRect();
        employeeCreationForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
        employeeCreationForm.style.left = `${buttonRect.left + (buttonRect.width/2) + window.scrollX - (formRect.width/2)}px`;
    });

    const fileInput = document.getElementById('fileInput');
    const fileName = document.getElementById('fileName');
    fileInput.addEventListener('change', function () {
        fileName.textContent = fileInput.files.length > 0 ? fileInput.files[0].name : "Not Chosen";
    });

    const employeeUpdateButtons = document.querySelectorAll('.employeeUpdateButton');
    const employeeUpdateForm = document.getElementById('employeeUpdateForm');
    const userIdInputForUpdate = document.getElementById('userIdInputForUpdate');
    const userNameInputForUpdate = document.getElementById('userNameInputForUpdate');
    const firstNameeInputForUpdate = document.getElementById('firstNameeInputForUpdate');
    const lastNameInputForUpdate = document.getElementById('lastNameInputForUpdate');
    const positionInputForUpdate = document.getElementById('positionInputForUpdate');
    const salaryInputForUpdate = document.getElementById('salaryInputForUpdate');
    employeeUpdateButtons.forEach(employeeUpdateButton => {
        employeeUpdateButton.addEventListener('click', function() {
            const userId = this.dataset.userId;
            userIdInputForUpdate.value = userId;

            fetch(`${window.AppConfig.apiSettings.localhostUrl}user/getbyid/${userId}`, {
                method: 'GET',
                credentials: 'include'
            })
                .then(response => response.json())
                .then(data => {
                    userNameInputForUpdate = data.user.userName;
                    firstNameeInputForUpdate = data.user.firstName;
                    lastNameInputForUpdate = data.user.lastName;
                    positionInputForUpdate = data.employee.position;
                    salaryInputForUpdate = data.employee.salary;
                }).catch(error => console.log(`Error from api url 'user/getbyid/{id}' --> ${error}`));

            const buttonRect = employeeUpdateButton.getBoundingClientRect();
            const formRect = employeeUpdateForm.getBoundingClientRect();
            employeeUpdateForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
            employeeUpdateForm.style.left = `${buttonRect.left + (buttonRect.width/2) + window.scrollX - (formRect.width/2)}px`;
            employeeUpdateForm.classList.toggle('hidden');
        });
    });

    const employeeDeletionButtons = document.querySelectorAll('.employeeDeletionButton');
    const employeeDeletionForm = document.getElementById('employeeDeletionForm');
    const userIdInputForDelete = document.getElementById('userIdInputForDelete');
    employeeDeletionButtons.forEach(employeeDeletionButton => {
        employeeDeletionButton.addEventListener('click', function() {
            userIdInputForDelete.value = this.dataset.userId;

            employeeDeletionForm.classList.toggle('hidden');

            const buttonRect = employeeDeletionButton.getBoundingClientRect();
            const formRect = employeeDeletionForm.getBoundingClientRect();
            employeeDeletionForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
            employeeDeletionForm.style.left = `${buttonRect.left + (buttonRect.width/2) + window.scrollX - (formRect.width/2)}px`;
        });
    });
    const cancelEmployeeDeletionButtons = document.querySelectorAll('.cancelEmployeeDeletionButton');
    cancelEmployeeDeletionButtons.forEach(cancelEmployeeDeletionButton => {
        cancelEmployeeDeletionButton.addEventListener('click', function() {
            employeeDeletionForm.classList.toggle('hidden');
        })
    });

});