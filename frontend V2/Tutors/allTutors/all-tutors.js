document.addEventListener('DOMContentLoaded', () => {
    const searchByCourseButton = document.getElementById('searchByCourse');
    const searchByIdButton = document.getElementById('searchById');
    const searchAllButton = document.getElementById('searchAll');

    searchByCourseButton.addEventListener('click', getTutorsByCourse);
    searchByIdButton.addEventListener('click', getTutorById);
    searchAllButton.addEventListener('click', getAllTutors);
});

function getTutorsByCourse() {
    const course = document.getElementById('courseSearch').value;
    fetch(`http://localhost:5000/api/tutors/available/byCourse?course=${course}`)
        .then(response => response.json())
        .then(data => populateTable(data))
        .catch(error => console.error('Error fetching tutors by course:', error));
}

function getTutorById() {
    const tutorId = document.getElementById('tutorIdSearch').value;
    fetch(`http://localhost:5000/api/tutors/${tutorId}`)
        .then(response => response.json())
        .then(tutor => populateTable([tutor])) // Wrap single tutor in an array
        .catch(error => console.error('Error fetching tutor by ID:', error));
}

function getAllTutors() {
    fetch('http://localhost:5000/api/tutors')
        .then(response => response.json())
        .then(data => populateTable(data))
        .catch(error => console.error('Error fetching all tutors:', error));
}

// function populateTable(tutors) {
//     const table = document.getElementById('tutorsTable');
//     table.innerHTML = ''; // Clear the table first

//     // Create the header row if tutors array is not empty
//     if (tutors.length > 0) {
//         const headerRow = table.insertRow();
//         Object.keys(tutors[0]).forEach(key => {
//             const th = document.createElement('th');
//             th.textContent = key;
//             headerRow.appendChild(th);
//         });
//     }

//     // Populate the table rows with tutor data
//     tutors.forEach(tutor => {
//         const row = table.insertRow();
//         Object.values(tutor).forEach(text => {
//             const cell = row.insertCell();
//             cell.textContent = text;
//         });
//     });
// }


function populateTable(tutors) {
    const table = document.getElementById('tutorsTable');
    table.innerHTML = ''; // Clear the table first

    // Define columns to exclude
    const excludeColumns = ['sessions', 'reviews'];

    // Create the header row if tutors array is not empty
    if (tutors.length > 0) {
        const headerRow = table.insertRow();
        Object.keys(tutors[0])
            .filter(key => !excludeColumns.includes(key)) // Exclude certain columns
            .forEach(key => {
                const th = document.createElement('th');
                th.textContent = key;
                headerRow.appendChild(th);
            });
    }

    // Populate the table rows with tutor data
    tutors.forEach(tutor => {
        const row = table.insertRow();
        Object.entries(tutor)
            .filter(([key, _]) => !excludeColumns.includes(key)) // Exclude certain columns
            .forEach(([_, text]) => {
                const cell = row.insertCell();
                cell.textContent = text;
            });
    });
}

