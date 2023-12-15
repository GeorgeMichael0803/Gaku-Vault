document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('createFinanceForm').addEventListener('submit', createExpenseEntry);
});

function createExpenseEntry(event) {
    event.preventDefault();

    const userId = document.getElementById('userId').value;
    const amount = parseFloat(document.getElementById('amount').value);
    const category = document.getElementById('category').value;
    const description = document.getElementById('description').value;
    const date = document.getElementById('date').value;

    const expenseData = {
        UserId: userId,
        Amount: amount,
        Category: category,
        NameOrDescription: description,
        Date: date
    };

    fetch('http://localhost:5000/api/expenses', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(expenseData)
    })
    .then(response => {
        if (!response.ok) {
            // If we're not ok, throw an error to skip to the catch block
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    })
    .then(data => {
        alert(`Expense Created:\nUser ID: ${data.userId}\nExpenseId: ${data.expenseId}\nAmount: ${data.amount}\nCategory: ${data.category}\nDescription: ${data.nameOrDescription}\nDate: ${new Date(data.date).toLocaleDateString()}\nUser Created`);
    })
    .catch(error => {
        alert('Failed to create expense: ' + error.message);
    });
}
