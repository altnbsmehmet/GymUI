window.addEventListener("DOMContentLoaded", async () => {

    const signInButton0 = document.getElementById('signInButton0');
    const signInButton1 = document.getElementById('signInButton1');
    const signInForm = document.getElementById('signInForm');
    const signUpButton = document.getElementById('signUpButton');
    const signUpForm = document.getElementById('signUpForm');

    signInButton0.addEventListener('click', function() {
        if (!signUpForm.classList.contains('hidden')) signUpForm.classList.add('hidden');
        signInForm.classList.toggle('hidden');

        const buttonRect = signInButton0.getBoundingClientRect();
        signInForm.style.top = `${buttonRect.bottom + window.scrollY + 5}px`;
        signInForm.style.left = `${buttonRect.left + window.scrollX - 153}px`;
    });

    signInButton1.addEventListener('click', function() {
        if (!signUpForm.classList.contains('hidden')) signUpForm.classList.add('hidden');
        signInForm.classList.toggle('hidden');

        const buttonRect = signInButton0.getBoundingClientRect();
        signInForm.style.top = `${buttonRect.bottom + window.scrollY + 5}px`;
        signInForm.style.left = `${buttonRect.left + window.scrollX - 153}px`;
    });

    signUpButton.addEventListener('click', function() {
        if (!signInForm.classList.contains('hidden')) signInForm.classList.add('hidden');
        signUpForm.classList.toggle('hidden');

        const buttonRect = signInButton0.getBoundingClientRect();
        signUpForm.style.top = `${buttonRect.bottom + window.scrollY + 5}px`;
        signUpForm.style.left = `${buttonRect.left + window.scrollX - 208}px`;
    });

});