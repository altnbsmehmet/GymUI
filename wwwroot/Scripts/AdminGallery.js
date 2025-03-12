window.addEventListener("DOMContentLoaded", async () => {

    const imageCreationButton = document.getElementById('imageCreationButton');
    const imageCreationForm = document.getElementById('imageCreationForm');
    imageCreationButton.addEventListener('click', function() {
        imageCreationForm.classList.toggle('hidden');

        const buttonRect = imageCreationButton.getBoundingClientRect();
        imageCreationForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
        imageCreationForm.style.left = `${buttonRect.left + window.scrollX}px`;
    });

    const fileInput = document.getElementById('fileInput');
    const fileName = document.getElementById('fileName');
    fileInput.addEventListener('change', function () {
        fileName.textContent = fileInput.files.length > 0 ? fileInput.files[0].name : "Not Chosen";
    });

});