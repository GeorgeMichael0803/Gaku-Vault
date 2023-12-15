document.addEventListener('DOMContentLoaded', function() {
    // Handle star click events
    const stars = document.querySelectorAll('.star');
    stars.forEach(star => {
        star.addEventListener('click', function() {
            const ratingValue = this.getAttribute('data-value');
            document.getElementById('rating').value = ratingValue; // Update the hidden input value

            // Highlight the selected stars
            stars.forEach(s => {
                s.style.color = s.getAttribute('data-value') <= ratingValue ? '#ffc700' : '#ddd';
            });
        });
    });

    document.getElementById("feedbackForm").addEventListener("submit", function(event){
        event.preventDefault();

        const courseId = document.getElementById("courseId").value;
        const studentId = document.getElementById("studentId").value;
        const feedbackText = document.getElementById("feedback").value;
        const rating = document.getElementById("rating").value; // Get the rating from the hidden input
        const dateSubmitted = new Date().toISOString();

        const feedbackData = {
            CourseId: courseId,
            StudentId: studentId,
            Rating: parseInt(rating, 10),
            Feedback: feedbackText,
            DateSubmitted: dateSubmitted
        };

        // POST request to the API
        fetch('http://localhost:5000/api/coursefeedback', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(feedbackData),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log('Feedback submitted:', data);
            alert('Feedback submitted successfully!');
            displayFeedback(feedbackData);
        })
        .catch(error => {
            console.error('Error submitting feedback:', error);
            alert('An error occurred while submitting feedback.');
        });
    });

    function displayFeedback(feedback) {
        const feedbackDetails = document.getElementById("feedbackDetails");
        const feedbackElement = document.createElement("div");
        feedbackElement.innerHTML = `
            <p>Course ID: ${feedback.CourseId}</p>
            <p>Student ID: ${feedback.StudentId}</p>
            <p>Rating: ${'☆'.repeat(feedback.Rating)}</p>
            <p>Feedback: ${feedback.Feedback}</p>
            <p>Date: ${feedback.DateSubmitted}</p>
        `;
        feedbackDetails.appendChild(feedbackElement);
    }
});
