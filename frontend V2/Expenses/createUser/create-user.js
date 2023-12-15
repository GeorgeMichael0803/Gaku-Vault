document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('createFinanceForm').addEventListener('submit', createFinanceUser);
});

function createFinanceUser(event) {
    event.preventDefault();

    const userName = document.getElementById('userName').value;
    const monthlyBudget = parseFloat(document.getElementById('monthlyBudget').value);
    const biWeeklySalary1 = parseFloat(document.getElementById('biWeeklySalary1').value);
    const biWeeklySalary2 = parseFloat(document.getElementById('biWeeklySalary2').value);

    const financeData = {
        UserName: userName,
        MonthlyBudget: monthlyBudget,
        BiWeeklySalary1: biWeeklySalary1,
        BiWeeklySalary2: biWeeklySalary2,
    };

    fetch('http://localhost:5000/api/expenses/finance', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(financeData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        alert(`UserId: ${data.userId}\nUser Name: ${data.userName}\nMonthly Budget: ${data.monthlyBudget}\nBi-Weekly Salary 1: ${data.biWeeklySalary1}\nBi-Weekly Salary 2: ${data.biWeeklySalary2}\nUser Created`);
    })
    .catch(error => {
        alert(`Error: ${error.message}`);
    });
}
