window.addEventListener("DOMContentLoaded", async () => {

    const hamburgerMenu = document.getElementById('hamburgerMenu');
    document.addEventListener('click', (event) => {
        const buttonType = event.target.dataset.buttonType;
        if (!buttonType) return;
        switch (buttonType) {
            case 'hamburgerMenuButton':
                hamburgerMenu.classList.toggle('hidden');
            break;
        }
    });

});