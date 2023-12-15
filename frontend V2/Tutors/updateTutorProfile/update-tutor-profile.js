document.addEventListener('DOMContentLoaded', () => {
    const updateForm = document.getElementById('updateTutorForm');
    updateForm.addEventListener('submit', updateTutorProfile);
});

function updateTutorProfile(event) {
    event.preventDefault(); // Prevent default form submission
    console.log('Form submission triggered'); // Debug log

    const tutorId = document.getElementById('tutorId').value;
    const updatedData = {
        Email: document.getElementById('email').value || "",
        Course: document.getElementById('course').value || "",
        TutorName: document.getElementById('tutorName').value || "",
        Description: document.getElementById('description').value || "",
        PhoneNumber: document.getElementById('phoneNumber').value || "",
        isAvailable: document.getElementById('isAvailable').checked
    };

    fetch(`http://localhost:5000/api/tutors/${tutorId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        console.log('Response received'); // Debug log
        return response.json();
    })
    .then(data => {
        console.log('Data:', data); // Debug log
        displayResponse(data);
    })
    .catch((error) => {
        console.error('Error:', error);
        displayResponse({ error: error.message });
    });
}

function displayResponse(data) {
    let message = '';

    if (data.error) {
        console.log('Error in response:', data.error);
        message = `Error: ${data.error}`;
    } else {
        console.log('Displaying response in alert'); // Debug log
        message = `Email: ${data.Email || ''}
Course: ${data.course || ''}
Tutor Name: ${data.tutorName || ''}
Description: ${data.description || ''}
Phone Number: ${data.phoneNumber || ''}
Is Available: ${data.isAvailable ? 'Yes' : 'No'}`;
    }

    alert(message);
}

