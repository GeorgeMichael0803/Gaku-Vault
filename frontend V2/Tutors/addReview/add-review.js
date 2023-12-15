document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('reviewForm').addEventListener('submit', addReview);
});

function addReview(event) {
    event.preventDefault();

    const tutorId = document.getElementById('tutorId').value;
    const reviewDescription = document.getElementById('reviewDescription').value;
    const rating = document.getElementById('rating').value;

    const reviewData = {
        ReviewDescription: reviewDescription,
        Rating: parseFloat(rating),
        TutorId: tutorId
    };

    fetch(`http://localhost:5000/api/tutors/${tutorId}/review`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reviewData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(review => {
        alert('Review submitted successfully!');
        console.log('Review:', review);
    })
    .catch(error => {
        alert(`Error: ${error.message}`);
        console.error('Error:', error);
    });
}
