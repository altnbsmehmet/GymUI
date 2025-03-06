window.addEventListener("DOMContentLoaded", async () => {

    const personalInformation = document.getElementById('personalInformation');
    const updateDivButton = document.getElementById('updateDivButton');
    const updateForm = document.getElementById('updateForm');
    const cancelFormButton = document.getElementById('cancelFormButton');
    const updateFormButton = document.getElementById('updateFormButton');
    updateDivButton.addEventListener('click', function() {
        personalInformation.classList.add('hidden');
        updateForm.classList.remove('hidden');
    });
    updateFormButton.addEventListener('click', function() {
        updateForm.classList.add('hidden');
        personalInformation.classList.remove('hidden');
    });
    cancelFormButton.addEventListener('click', function() {
        updateForm.classList.add('hidden');
        personalInformation.classList.remove('hidden');
    });

});