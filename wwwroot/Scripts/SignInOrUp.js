window.addEventListener("DOMContentLoaded", async () => {

    const hamburgerMenu = document.getElementById('hamburgerMenu');
    const signInForm = document.getElementById('signInForm');
    const signUpForm = document.getElementById('signUpForm');
    document.addEventListener('click', (event) => {
        const buttonType = event.target.dataset.buttonType;
        if (!buttonType) return;
        switch (buttonType) {
            case 'hamburgerMenuButton':
                if (!signInForm.classList.contains('hidden')) signInForm.classList.add('hidden');
                if (!signUpForm.classList.contains('hidden')) signUpForm.classList.add('hidden');
                hamburgerMenu.classList.toggle('hidden');
            break;
            case 'signInButton':
                if (!hamburgerMenu.classList.contains('hidden')) hamburgerMenu.classList.add('hidden');
                if (!signUpForm.classList.contains('hidden')) signUpForm.classList.add('hidden');
                signInForm.classList.toggle('hidden');
            break;
            case 'signUpButton':
                if (!hamburgerMenu.classList.contains('hidden')) hamburgerMenu.classList.add('hidden');
                if (!signInForm.classList.contains('hidden')) signInForm.classList.add('hidden');
                signUpForm.classList.toggle('hidden');
            break;
        }
    });

});