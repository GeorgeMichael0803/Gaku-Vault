function deleteExpenseById() {
    const userId = document.getElementById('userId-deleteById').value.trim();
    const expenseId = document.getElementById('expenseId-deleteById').value.trim();
    const endpoint = `http://localhost:5000/api/expenses/${userId}/deleteById/${expenseId}`;

    fetch(endpoint, { method: 'DELETE' })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete the expense by ID.');
            }
            return response.text();
        })
        .then(message => alert(message))
        .catch(error => alert(error));
}

function deleteExpenseByNameOrDescription() {
    const userId = document.getElementById('userId-deleteByName').value.trim();
    const nameOrDescription = document.getElementById('nameOrDescription-deleteByName').value.trim();
    const endpoint = `http://localhost:5000/api/expenses/${userId}/deleteByName/${encodeURIComponent(nameOrDescription)}`;

    fetch(endpoint, { method: 'DELETE' })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete the expense by name/description.');
            }
            return response.text();
        })
        .then(message => alert(message))
        .catch(error => alert(error));
}

function deleteUserById() {
    const userId = document.getElementById('userId-deleteUser').value.trim();
    const endpoint = `http://localhost:5000/api/expenses/finance/${userId}`;

    fetch(endpoint, { method: 'DELETE' })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete the user by ID.');
            }
            return response.text();
        })
        .then(message => alert(message))
        .catch(error => alert(error));
}

function deleteUserByName() {
    const userName = document.getElementById('userName-deleteUserByName').value.trim();
    const endpoint = `http://localhost:5000/api/expenses/finance/byName/${encodeURIComponent(userName)}`;

    fetch(endpoint, { method: 'DELETE' })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete the user by name.');
            }
            return response.text();
        })
        .then(message => alert(message))
        .catch(error => alert(error));
}
