function addToTable(event) {
    // Prevents the button from doing a form submit
    event.preventDefault();

    // Validate the form using the jQuery Validator plugin
    var validator = $("#addTeamToTable").validate();

    // Checks if the form is valid
    if (validator.form()) {
        console.log("Form is okay - carry on");

        alert("Form is valid - submit to server");

    } else {
        console.log("Form is not okay - stop");
    }
}

function editClub(tableID) {
    alert(tableID);
}

function moveClub(tableID, direction) {
    alert(direction);
    alert(tableID);
}

function removeClub(tableID) {
    alert(tableID);
}