window.addEventListener("DOMContentLoaded", async () => {

    assignMembershipIds();

    const membershipCreationButton = document.getElementById('membershipCreationButton');
    const membershipCreationForm = document.getElementById('membershipCreationForm');
    membershipCreationButton.addEventListener('click', function() {
        membershipCreationForm.classList.toggle('hidden');

        const buttonRect = membershipCreationButton.getBoundingClientRect();
        membershipCreationForm.style.top = `${buttonRect.bottom + window.scrollY}px`;
        membershipCreationForm.style.left = `${buttonRect.left + window.scrollX}px`;
    });

    console.log("whatsup");

});

function assignMembershipIds() {
    const memberships = document.querySelectorAll('.membershipInformation');
    memberships.forEach((membership, index) => {
        membership.setAttribute('data-membership-id', index + 1);
    });

}