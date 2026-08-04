function showDialog(dialog, text) {
    // Displays the modal that was sent and also replaces
    // the dialog-text span with the text that was sent as well
    var span = dialog.getElementsByClassName("dialog-text")[0];
    span.innerHTML = text;
    dialog.showModal();
}

function closeDialog(dialog) {
    // Closes the modal
    dialog.close();
}

function removeTempDataMessages() {
    // Hide any existing temp-data messages before the removal request runs
    const successTempData = document.getElementById("successTempData");
    if (successTempData) {
        successTempData.remove();
    }

    const failureTempData = document.getElementById("failureTempData");
    if (failureTempData) {
        failureTempData.remove();
    }
}

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

/* Move a club up or down in the table */
function moveClub(tableId, direction) {
    // Remove any existing temp-data messages before proceeding with the removal request
    removeTempDataMessages();
    
    // Gets the waiting dialog and displays it on the page
    const waitingDialog = document.getElementById("waitingDialog");
    showDialog(waitingDialog, "Removing club from table... please wait");

    // Gets the verification token from the form
    var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');

    // Set the data for the api call
    var data = {
        Id: tableId,
        Direction: direction
    };

    // Set the url for the api call
    var url = "/Table/Move";

    // Configure the request
    const requestOptions = {
        method: 'POST', // Specify the request method
        headers: {
            'Content-Type': 'application/json',
            '__RequestVerificationToken': requestVerificationToken.value
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    };

    // Make the post request
    fetch(url, requestOptions)
        .then(response => {
            // Check if the request was successful (status code 2xx)
            if (!response.ok) {
                // Status was not ok - throw the response to be handled by the catch block
                throw response; //Will get picked up by the catch block
            }
            // Parse the response JSON
            return response.json();
        })
        .then(data => {
            // Handle the response data

            if (data.success) {
                //Club moved in the table succesfully

                //Refresh the participants list
                refreshTablesPanel();

                // Close the waiting diaglog and show the success dialog
                closeDialog(waitingDialog);
                showDialog(successDialog, data.messageToUser);
            } else {
                // The server was succesfully reached and the request to move the club in the table was done
                // However the club was not moved successfully for some reason

                // Close the waiting dialog
                closeDialog(waitingDialog);

                // Show the warning dialog with the message from the server so the user knows what went wrong
                showDialog(warningDialog, data.messageToUser);
            }
        })
        .catch(error => {
            // Complete error - the server wasn't reached or just completely failed

            // Close the waiting dialog
            closeDialog(waitingDialog);

            // Show the warning dialog with the message from the server
            showDialog(warningDialog, error);
        });
}

/* Removes a club from the table. This function is called when the user clicks the 
   "Remove" button next to a club in the table. It sends a POST request to the server 
   to remove the club from the table and handles the response accordingly. The 
   function also displays a waiting dialog while the request is being processed and 
   shows success or warning dialogs based on the outcome of the request */
function removeClub(tableId) {
    // Remove any existing temp-data messages before proceeding with the removal request
    removeTempDataMessages();
    
    // Gets the waiting dialog and displays it on the page
    const waitingDialog = document.getElementById("waitingDialog");
    showDialog(waitingDialog, "Removing club from table... please wait");

    // Gets the verification token from the form
    var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');

    // Set the data for the api call
    var data = {
        Id: tableId
    };

    // Set the url for the api call
    var url = "/Table/Remove";

    // Configure the request
    const requestOptions = {
        method: 'POST', // Specify the request method
        headers: {
            'Content-Type': 'application/json',
            '__RequestVerificationToken': requestVerificationToken.value
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    };

    // Make the post request
    fetch(url, requestOptions)
        .then(response => {
            // Check if the request was successful (status code 2xx)
            if (!response.ok) {
                // Status was not ok - throw the response to be handled by the catch block
                throw response; //Will get picked up by the catch block
            }
            // Parse the response JSON
            return response.json();
        })
        .then(data => {
            // Handle the response data

            if (data.success) {
                //Club removed from table succesfully

                //Refresh the participants list
                refreshTablesPanel();

                // Close the waiting diaglog and show the success dialog
                closeDialog(waitingDialog);
                showDialog(successDialog, data.messageToUser);
            } else {
                // The server was succesfully reached and the request to remove the club from the table was done
                // However the club was not removed successfully for some reason

                // Close the waiting dialog
                closeDialog(waitingDialog);

                // Show the warning dialog with the message from the server so the user knows what went wrong
                showDialog(warningDialog, data.messageToUser);
            }
        })
        .catch(error => {
            // Complete error - the server wasn't reached or just completely failed

            // Close the waiting dialog
            closeDialog(waitingDialog);

            // Show the warning dialog with the message from the server
            showDialog(warningDialog, error);
        });
}

/* Refreshes the tables panel with an updated table. Can be called when removing a club
   from the table or changing the order of a club in the table */
function refreshTablesPanel() {

    // Refreshes the panel with the table in it
    fetch("/Table/RefreshTablesPanel?SeasonId=" + document.getElementById('SeasonId').value).then(function (response) {
        return response.text();
    }).then(function (html) {
        // Puts the html returned from the view in the div
        var x = document.getElementById("table");
        x.replaceChildren();
        x.innerHTML = html;
    });
}