document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('eventForm').addEventListener('submit', function(e) {
        e.preventDefault();

        const eventData = {
            Title: document.getElementById('eventTitle').value,
            Description: document.getElementById('eventDescription').value,
            StartTime: document.getElementById('eventStartTime').value,
            EndTime: document.getElementById('eventEndTime').value,
            IsReminderSet: document.getElementById('eventReminder').checked,
            IsRecurring: document.getElementById('eventRecurring').checked
        };

        fetch('http://localhost:5000/api/events', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(eventData),
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(() => {
            fetchEvents(); // Fetch all events including the newly added one
        })
        .catch((error) => {
            console.error('Error:', error);
            alert(`Error: ${error.message}`);
        });
    });

    fetchEvents(); // Fetch events when the page loads
});

function fetchEvents() {
    fetch('http://localhost:5000/api/events')
        .then(response => response.json())
        .then(data => {
            console.log("Fetched Events:", data); 
            displayEvents(data);
        })
        .catch(error => {
            console.error('Error fetching events:', error);
        });
}


function displayEvents(events) {
    const eventsDetails = document.getElementById('eventsDetails');
    eventsDetails.innerHTML = events.map(event => `
        <div class="event-card">
            <div class="event-details">
                <h4>${event.title}</h4>
                <p>Description: ${event.description}</p>
                <p>Start Time: ${formatDate(event.startTime)}</p>
                <p>End Time: ${formatDate(event.endTime)}</p>
            </div>
            <div class="event-footer">
                <p>Reminder: ${event.isReminderSet ? 'Yes' : 'No'}</p>
                <p>Recurring: ${event.isRecurring ? 'Yes' : 'No'}</p>
            </div>
        </div>
    `).join('');
}


function formatDate(dateTimeStr) {
    const date = new Date(dateTimeStr);
    return date.toLocaleString();
}
