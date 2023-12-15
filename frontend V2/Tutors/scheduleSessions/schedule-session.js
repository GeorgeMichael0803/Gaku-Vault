document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('scheduleSessionForm');
    form.addEventListener('submit', scheduleSession);
});

function scheduleSession(event) {
    event.preventDefault();

    // Fetching the tutor ID from the input field
    const tutorId = document.getElementById('tutorId').value;

    const sessionData = {
            StudentName: document.getElementById('studentName').value,
            StudentId: parseInt(document.getElementById('studentId').value, 10),
            Date: document.getElementById('date').value,
            Time: document.getElementById('time').value + ":00"
            //SessionStatus: 'Scheduled', // Default status
    };

    fetch(`http://localhost:5000/api/tutors/${tutorId}/schedule`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(sessionData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        let message = `Session ID: ${data.sessionId}\n`;
        message += `Student Name: ${data.studentName}\n`;
        message += `Student ID: ${data.studentId}\n`;
        message += `Date: ${data.date}\n`;
        message += `Time: ${data.time}\n`;
        message += `Session Status: ${data.sessionStatus}\n`;
        message += `Tutor ID: ${data.tutorId}\n`;
        message += "Session is Scheduled Successfully.";

    alert(message);
    })
    .catch(error => {
        alert(`Error: ${error.message}`);
    });
}
