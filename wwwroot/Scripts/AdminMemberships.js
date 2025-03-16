window.addEventListener("DOMContentLoaded", async () => {

    assignMembershipIds();

    const membershipCreationButton = document.getElementById('membershipCreationButton');
    const membershipCreationForm = document.getElementById('membershipCreationForm');
    membershipCreationButton.addEventListener('click', function() {
        membershipCreationForm.classList.toggle('hidden');

        const buttonRect = membershipCreationButton.getBoundingClientRect();
        const formRect = membershipCreationForm.getBoundingClientRect();
        membershipCreationForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
        membershipCreationForm.style.left = `${buttonRect.left + (buttonRect.width/2) + window.scrollX - (formRect.width/2)}px`;
    });

    console.log("whatsup");

});

function assignMembershipIds() {
    const memberships = document.querySelectorAll('.membershipInformation');
    memberships.forEach((membership, index) => {
        membership.setAttribute('data-membership-id', index + 1);
    });

}