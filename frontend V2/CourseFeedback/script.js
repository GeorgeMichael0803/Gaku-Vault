document.addEventListener('DOMContentLoaded', function() {
    document.getElementById('feedbackForm').addEventListener('submit', submitFeedback);
    fetchFeedback(); 
});

function submitFeedback(e) {
    e.preventDefault();

    const feedbackData = {
        CourseId: document.getElementById('CourseId').value,
        StudentId: document.getElementById('StudentId').value,
        Rating: parseInt(document.getElementById('Rating').value, 10),
        Feedback: document.getElementById('Feedback').value,
    };

    fetch('http://localhost:5000/api/coursefeedback', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(feedbackData),
    })
    .then(response => response.json())
    .then(data => {
        displayFeedback(data); // Call this to update the UI
    })
    .catch(error => console.error('Error:', error));
}

function displayFeedback(feedbacks) {
    const feedbackDetails = document.getElementById('feedbackDetails');
    feedbackDetails.innerHTML = feedbacks.map(feedback => `
        <div class="feedback-card">
            <p><strong>Course ID:</strong> ${feedback.CourseId}</p>
            <p><strong>Student ID:</strong> ${feedback.StudentId}</p>
            <p><strong>Rating:</strong> ${feedback.Rating}</p>
            <p><strong>Feedback:</strong> ${feedback.Feedback}</p>
        </div>
    `).join('');
}


function fetchFeedback() {
    fetch('http://localhost:5000/api/coursefeedback')
        .then(response => response.json())
        .then(data => {
            displayFeedback(data);
        })
        .catch(error => console.error('Error:', error));
}