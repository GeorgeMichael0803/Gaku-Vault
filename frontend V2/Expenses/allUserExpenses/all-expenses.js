document.addEventListener('DOMContentLoaded', function() {
    const baseApiUrl = 'http://localhost:5000/api/expenses/';
    let currentPageNumber = 1;
    const pageSize = 10;
    let totalPages = 0;

    const userIdInput = document.getElementById('user-id-input');
    const loadExpensesBtn = document.getElementById('load-expenses');
    const expensesTableBody = document.getElementById('expenses-table').querySelector('tbody');
    const firstPageBtn = document.getElementById('first-page');
    const lastPageBtn = document.getElementById('last-page');
    const nextPageBtn = document.getElementById('next-page');
    const prevPageBtn = document.getElementById('previous-page');

    loadExpensesBtn.addEventListener('click', function() {
        const userId = userIdInput.value.trim();
        if (userId) {
            loadExpenses(userId, currentPageNumber);
        } else {
            alert('Please enter a User ID.');
        }
    });

    firstPageBtn.addEventListener('click', function() {
        if (currentPageNumber !== 1) {
            currentPageNumber = 1;
            loadExpenses(userIdInput.value.trim(), currentPageNumber);
        }
    });

    lastPageBtn.addEventListener('click', function() {
        if (totalPages > 0 && currentPageNumber !== totalPages) {
            currentPageNumber = totalPages;
            loadExpenses(userIdInput.value.trim(), currentPageNumber);
        }
    });

    nextPageBtn.addEventListener('click', function() {
        if (currentPageNumber < totalPages) {
            currentPageNumber++;
            loadExpenses(userIdInput.value.trim(), currentPageNumber);
        }
    });

    prevPageBtn.addEventListener('click', function() {
        if (currentPageNumber > 1) {
            currentPageNumber--;
            loadExpenses(userIdInput.value.trim(), currentPageNumber);
        }
    });

    function loadExpenses(userId, pageNumber) {
        let endpoint = `${baseApiUrl}${userId}?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        fetch(endpoint)
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                updateTable(data.data);
                totalPages = data.meta.TotalPages;
                updatePaginationControls(data.links);
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Failed to load expenses. Please try again later.');
            });
    }

    function updateTable(expenses) {
        expensesTableBody.innerHTML = '';
        expenses.forEach(expense => {
            const row = expensesTableBody.insertRow();
            row.innerHTML = `
                <td>${expense.expenseId}</td>
                <td>$${expense.amount.toFixed(2)}</td>
                <td>${expense.category}</td>
                <td>${expense.nameOrDescription}</td>
                <td>${new Date(expense.date).toLocaleString()}</td>
            `;
        });
    }

    function updatePaginationControls(links) {
        firstPageBtn.disabled = !links.First;
        lastPageBtn.disabled = !links.Last;
        nextPageBtn.disabled = !(links.Next && currentPageNumber < totalPages);
        prevPageBtn.disabled = !(links.Prev && currentPageNumber > 1);
    }
});
