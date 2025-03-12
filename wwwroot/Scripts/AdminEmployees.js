window.addEventListener("DOMContentLoaded", async () => {

    const employeeCreationButton = document.getElementById('employeeCreationButton');
    const employeeCreationForm = document.getElementById('employeeCreationForm');
    employeeCreationButton.addEventListener('click', function () {
        employeeCreationForm.classList.toggle('hidden');

        const buttonRect = employeeCreationButton.getBoundingClientRect();
        employeeCreationForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
        employeeCreationForm.style.left = `${buttonRect.left + window.scrollX}px`;
    });

    const fileInput = document.getElementById('fileInput');
    const fileName = document.getElementById('fileName');
    fileInput.addEventListener('change', function () {
        fileName.textContent = fileInput.files.length > 0 ? fileInput.files[0].name : "Not Chosen";
    });

    const employeeUpdateButtons = document.querySelectorAll('.employeeUpdateButton');
    const employeeUpdateForm = document.getElementById('employeeUpdateForm');
    const userIdInputUpdate = document.getElementById('userIdInputUpdate');
    employeeUpdateButtons.forEach(employeeUpdateButton => {
        employeeUpdateButton.addEventListener('click', function() {
            userIdInputUpdate.value = this.dataset.userId;

            employeeUpdateForm.classList.toggle('hidden');

            const buttonRect = employeeUpdateButton.getBoundingClientRect();
            employeeUpdateForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
            employeeUpdateForm.style.left = `${buttonRect.left + window.scrollX}px`;
        });
    });

    const employeeDeletionButtons = document.querySelectorAll('.employeeDeletionButton');
    const employeeDeletionForm = document.getElementById('employeeDeletionForm');
    const userIdInputDelete = document.getElementById('userIdInputDelete');
    employeeDeletionButtons.forEach(employeeDeletionButton => {
        employeeDeletionButton.addEventListener('click', function() {
            userIdInputDelete.value = this.dataset.userId;
            console.dir(userIdInputDelete);

            employeeDeletionForm.classList.toggle('hidden');

            const buttonRect = employeeDeletionButton.getBoundingClientRect();
            employeeDeletionForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
            employeeDeletionForm.style.left = `${buttonRect.left + window.scrollX}px`;
        });
    });
    const cancelEmployeeDeletionButtons = document.querySelectorAll('.cancelEmployeeDeletionButton');
    cancelEmployeeDeletionButtons.forEach(cancelEmployeeDeletionButton => {
        cancelEmployeeDeletionButton.addEventListener('click', function() {
            employeeDeletionForm.classList.toggle('hidden');
        })
    });

});