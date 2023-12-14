document.addEventListener('DOMContentLoaded', (event) => {
    // Attach click events to buttons
    document.querySelectorAll('.navigate-button').forEach(button => {
        button.addEventListener('click', function() {
            window.location.href = this.getAttribute('data-target');
        });
    });

    // Attach click event to survey button
    document.getElementById('survey-button').addEventListener('click', function() {
        alert('Survey button clicked!');
    });
});
