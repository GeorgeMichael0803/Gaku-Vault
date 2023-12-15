function fetchSessions() {
    const tutorId = document.getElementById('tutorId').value;
    if (!tutorId) {
        alert('Please enter a Tutor ID.');
        return;
    }

    fetch(`http://localhost:5000/api/tutors/sessions/${tutorId}`)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(sessions => {
        const tableBody = document.getElementById('sessionsTable').querySelector('tbody');
        tableBody.innerHTML = ''; // Clear existing rows
        sessions.forEach(session => {
            const row = tableBody.insertRow();
            row.innerHTML = `
                <td>${session.sessionId}</td>
                <td>${session.studentName}</td>
                <td>${session.studentId}</td>
                <td>${session.date}</td>
                <td>${session.time}</td>
            `;
        });
    })
    .catch(error => {
        alert(`Error: ${error.message}`);
    });
}
