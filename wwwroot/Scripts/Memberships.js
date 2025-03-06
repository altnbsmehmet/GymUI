window.addEventListener("load", async () => {

    const membershipsContanier = document.getElementById('memberships');
    try {
        const response = await fetch("http://localhost:5410/api/membership/getall", {
            method: "GET",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            }
        });
        if (!response.ok) {
            throw new Error(`HTTP Error! Status: ${response.status}`);
        }
        const data = await response.json();
        console.log(`Response\n${JSON.stringify(data, null, 2)}`);
        
        if (data.isSuccess) {
            data.memberships.forEach(membership => {
                const membershipDiv = document.createElement('div');
                membershipDiv.classList.add('bg-white', 'p-6', 'rounded-lg', 'shadow-md', 'text-center', 'border');

                membershipDiv.innerHTML = `
                    <h3 class="text-xl font-semibold">Type: ${membership.type}</h3>
                    <p class="text-gray-600 mt-2">Months: ${membership.duration}</p>
                    <p class="text-lg font-bold mt-4">Price: ₺${membership.price}</p>
                    <button class="mt-4 bg-blue-600 text-white py-2 px-4 rounded-lg hover:bg-blue-700">Purchase</button>
                `;
                membershipsContanier.appendChild(membershipDiv);
            });
        } else {
            throw new Error('Invalid membership data.');
        }

    } catch (error) {
        console.error("Error -->", error);
        alert(`Error --> ${error.message}`);
    }

});