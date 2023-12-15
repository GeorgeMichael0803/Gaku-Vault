let currentUpdatingEventId = null; 

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('eventForm').addEventListener('submit', submitEvent);
    fetchEvents(); 
});

function submitEvent(e) {
    e.preventDefault();

    const eventData = {
        Title: document.getElementById('eventTitle').value,
        Description: document.getElementById('eventDescription').value,
        StartTime: document.getElementById('eventStartTime').value,
        EndTime: document.getElementById('eventEndTime').value,
        IsReminderSet: document.getElementById('eventReminder').checked,
        IsRecurring: document.getElementById('eventRecurring').checked
    };

    if (currentUpdatingEventId) {
        
        submitUpdatedEvent(currentUpdatingEventId, eventData);
    } else {
        
        fetch('http://localhost:5000/api/events', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(eventData),
        })
        .then(handleResponse)
        .then(() => {
            fetchEvents();
        })
        .catch(handleError);
    }
}

function handleResponse(response) {
    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }
    return response.json();
}

function handleError(error) {
    console.error('Error:', error);
    alert(`Error: ${error.message}`);
}

function fetchEvents() {
    fetch('http://localhost:5000/api/events')
        .then(response => response.json())
        .then(data => {
            displayEvents(data);
        })
        .catch(handleError);
}

function updateEvent(eventId) {
    currentUpdatingEventId = eventId;
    fetch(`http://localhost:5000/api/events/${eventId}`)
        .then(handleResponse)
        .then(event => {
            document.getElementById('eventTitle').value = event.Title;
            document.getElementById('eventDescription').value = event.Description;
            document.getElementById('eventStartTime').value = event.StartTime;
            document.getElementById('eventEndTime').value = event.EndTime;
            document.getElementById('eventReminder').checked = event.IsReminderSet;
            document.getElementById('eventRecurring').checked = event.IsRecurring;
        })
        .catch(handleError);
}

function submitUpdatedEvent(eventId, eventData) {
    fetch(`http://localhost:5000/api/events/${eventId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(eventData),
    })
    .then(handleResponse)
    .then(() => {
        fetchEvents();
        resetForm();
        currentUpdatingEventId = null;
    })
    .catch(handleError);
}

function deleteEvent(eventId) {
    fetch(`http://localhost:5000/api/events/${eventId}`, {
        method: 'DELETE',
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        fetchEvents(); 
    })
    .catch((error) => {
        console.error('Error:', error);
        alert(`Error: ${error.message}`);
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
            <div class="event-actions">
                <button class="update-button" onclick="updateEvent('${event.id}')">Update</button>
                <button class="delete-button" onclick="deleteEvent('${event.id}')">Delete</button>
            </div>
            <div class="event-footer">
                <p>Reminder: ${event.isReminderSet ? 'Yes' : 'No'}</p>
                <p>Recurring: ${event.isRecurring ? 'Yes' : 'No'}</p>
            </div>
        </div>
    `).join('');
}

function resetForm() {
    
    document.getElementById('eventTitle').value = '';
    document.getElementById('eventDescription').value = '';
    document.getElementById('eventStartTime').value = '';
    document.getElementById('eventEndTime').value = '';
    document.getElementById('eventReminder').checked = false;
    document.getElementById('eventRecurring').checked = false;

    
    currentUpdatingEventId = null;

    
    document.getElementById('eventForm').onsubmit = submitEvent;
}



function formatDate(dateTimeStr) {
    const date = new Date(dateTimeStr);
    return date.toLocaleString();
}
