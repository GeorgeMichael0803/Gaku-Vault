// document.addEventListener('DOMContentLoaded', () => {
//     const tutorForm = document.getElementById('tutorForm');
//     const responseMessage = document.getElementById('responseMessage');

//     tutorForm.addEventListener('submit', function(e) {
//         e.preventDefault() ? e.preventDefault() :e.preventDefault()  ;

//         // Gather form data
//         const tutorData = {
//             TutorName: document.getElementById('tutorName').value,
//             Email: document.getElementById('email').value,
//             PhoneNumber: document.getElementById('phoneNumber').value,
//             Course: document.getElementById('course').value,
//             Description: document.getElementById('description').value,
//             IsAvailable: document.getElementById('isAvailable').checked
//         };

//         // Display the inputted details
//         let details = '';
//         for (let key in tutorData) {
//             details += `${key}: ${tutorData[key]}<br>`;
//         }
//         displayMessage(details, 'info');

//         // Send POST request to the server
//         fetch('http://localhost:5000/api/tutors', { // Adjust the URL as needed
//             method: 'POST',
//             headers: {
//                 'Content-Type': 'application/json',
//             },
//             body: JSON.stringify(tutorData),
//         })
//         .then(response => {
//             if (!response.ok) {
//                 throw new Error(`HTTP error! Status: ${response.status}`);
//             }
//             return response.json();
//         })
//         .then(data => {
//             // Displaying the response
//             displayMessage(`Registration Successful! Name: ${data.UserName}, ID: ${data.UserId}`, 'success');
//         })
//         .catch((error) => {
//             displayMessage(`Error: ${error.message}`, 'error');
//         });

//         // return false;  
//     });

//     function displayMessage(message, type) {
//         responseMessage.innerHTML = '';
//         const messageElement = document.createElement('p');
//         messageElement.innerHTML = message;
//         messageElement.className = type === 'success' ? 'success-message' : 'error-message';
//         responseMessage.appendChild(messageElement);

//         // Keep the message for 10 seconds
//         setTimeout(() => {
//             responseMessage.removeChild(messageElement);
//         }, 10000);
//     }
// });


document.addEventListener('DOMContentLoaded', () => {
    const tutorForm = document.getElementById('tutorForm');

    tutorForm.addEventListener('submit', function(e) {
        e.preventDefault();

        const tutorData = {
            TutorName: document.getElementById('tutorName').value,
            Email: document.getElementById('email').value,
            PhoneNumber: document.getElementById('phoneNumber').value,
            Course: document.getElementById('course').value,
            Description: document.getElementById('description').value,
            IsAvailable: document.getElementById('isAvailable').checked
        };

        fetch('http://localhost:5000/api/tutors', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(tutorData),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            // Displaying the response in an alert
            //alert(`Registration Successful! Name: ${data.tutorName}, ID: ${data.tutorId}`);
             alert(`Registration Successful!\nName: ${data.tutorName}\nID: ${data.tutorId}\n\n'Save the ID'`);
        })
        .catch((error) => {
            alert(`Error: ${error.message}`);
        });
    });
});
