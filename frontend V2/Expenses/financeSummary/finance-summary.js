function getFinanceSummary() {
    const userId = document.getElementById('userId').value;
    if (!userId) {
        alert("Please enter a User ID.");
        return;
    }

    fetch(`http://localhost:5000/api/expenses/summary/${userId}`)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(summary => {
        displaySummary(summary);
    })
    .catch(error => {
        alert(`Error: ${error.message}`);
    });
}

function displaySummary(summary) {
    const summaryDisplay = document.getElementById('summaryDisplay');
    summaryDisplay.innerHTML = `
        <p>Total Expenses: ${summary.totalExpenses}</p>
        <p>Total Income: ${summary.totalIncome}</p>
        <p>Remaining Budget: ${summary.remainingBudget}</p>
    `;
}
