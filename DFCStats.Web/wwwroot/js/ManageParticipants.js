function updateParticipant(event) {
	// Validate the form using the jQuery Validator plugin
	var validator = $("#updateParticipant").validate();

	// Checks if the form is valid
	if (validator.form()) {
		console.log("Form is okay - carry on");

		// Disable the submit button to stop the person being updated again until the post is dealt with
		// The dialog boxes should stop it being used but this makes absolutly sure
		document.querySelector('#updatePerson').disabled = true;

		// Get all the dialogs on the page - These will be needed later for displaying relevant messages
		const waitingDialog = document.getElementById("waitingDialog");
		const successDialog = document.getElementById("successDialog");
		const warningDialog = document.getElementById("warningDialog");

		// Show the waiting dialog and sets a waiting message
		showDialog(waitingDialog, "Please wait a moment");

		//Gets the fields from the form including the request verification token
		var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');
		var participantIDField = document.getElementById('Id');
		var fixtureIDField = document.getElementById('FixtureId');
		var personIDField = document.getElementById('PersonId');
		var roleInFixtureField = document.getElementById('RoleInFixture');
		var goalsField = document.getElementById('Goals');
		var yellowCardField = document.getElementById('YellowCard');
		var redCardField = document.getElementById('RedCard');
		var replacedByPersonIDField = document.getElementById('ReplacedByPersonId');
		var replacedTimeField = document.getElementById('ReplacedTime');

		// Set the data and the url for the api call
		var data = {
			Id: participantIDField.value,
			FixtureId: fixtureIDField.value,
			PersonId: personIDField.value,
			RoleInFixture: roleInFixtureField.value,
			Goals: Number(goalsField.value),
			YellowCard: JSON.parse(yellowCardField.value),
			RedCard: JSON.parse(redCardField.value),
			ReplacedByPersonId: replacedByPersonIDField.value,
			ReplacedTime: Number(replacedTimeField.value)
		};

		// If the replaced by person id field is empty - ie a sub hasn't been picked
		// set the replacedByPersonID and ReplacedTime to null in the data variable
		if (replacedByPersonIDField.value == "") {
			data.ReplacedByPersonId = null;
			data.ReplacedTime = null;
		}

		console.log(data);

		var url = "/Participation/EditParticipant";

		//Configuring the request
		const requestOptions = {
			method: 'POST', // Specify the request method
			headers: {
				'Content-Type': 'application/json',
				'__RequestVerificationToken': requestVerificationToken.value
			},
			body: JSON.stringify(data) // Convert the data to JSON format
		};

		//Make the post request
		fetch(url, requestOptions)
			.then(response => {
				// Check if the request was successful (status code 2xx)
				if (!response.ok) {
					throw response; //Will get picked up by the catch block
				}
				// Parse the response JSON
				return response.json();
			})
			.then(data => {
				// Handle the response data

				if (data.success) {
					// Person updated succesfully

					//Refresh the participants list
					refreshParticipantsPanel();

					//Load the add participant form back in
					loadAddParticipantForm(fixtureIDField.value);

					//Eable the submit button and close the waiting dialog
					document.querySelector('#updatePerson').disabled = false;
					closeDialog(waitingDialog);

					//Show the success dialog with the message from the server
					showDialog(successDialog, data.messageToUser);
				} else {
					//The server was succesfully reached and the request to add the person was done
					//However the person was not added successfully for some reason

					//Eable the submit button and close the waiting dialog
					document.querySelector('#updatePerson').disabled = false;
					closeDialog(waitingDialog);

					//Show the warning dialog with the message from the server so the user knows what went wrong
					showDialog(warningDialog, data.messageToUser);
				}
			})
			.catch(error => {
				//Complete error - the server wasn't reached or just completely failed

				//Eable the submit button and close the waiting dialog
				document.querySelector('#updatePerson').disabled = false;
				closeDialog(waitingDialog);

				//Show the warning dialog with the message from the server
				showDialog(warningDialog, error);
			});
	}

	//Prevents the button from doing a form submit
	event.preventDefault();
}

function addAsParticipant(event) {
	//Validate the form using the jQuery Validator plugin
	var validator = $("#addPersonToFixture").validate();

	//Checks if the form is valid
	if (validator.form()) {
		console.log("Form is okay - carry on");

		//Disable the submit button to stop the person being added again until the post is dealt with
		//The dialog boxes should stop it being used but this makes absolutly sure
		document.querySelector('#addPerson').disabled = true;

		//Get all the dialogs on the page - These will be needed later for displaying relevant messages
		const waitingDialog = document.getElementById("waitingDialog");
		const successDialog = document.getElementById("successDialog");
		const warningDialog = document.getElementById("warningDialog");

		//Show the waiting dialog and sets a waiting message
		showDialog(waitingDialog, "Please wait a moment");

		//Gets the fields from the form including the request verification token
		var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');
		var fixtureIDField = document.getElementById('FixtureId');
		var personIDField = document.getElementById('PersonId');
		var roleInFixtureField = document.getElementById('RoleInFixture');
		var goalsField = document.getElementById('Goals');
		var yellowCardField = document.getElementById('YellowCard');
		var redCardField = document.getElementById('RedCard');
		var replacedByPersonIDField = document.getElementById('ReplacedByPersonId');
		var replacedTimeField = document.getElementById('ReplacedTime');

		//Set the data and the url for the api call
		var data = {
			FixtureId: fixtureIDField.value,
			PersonId: personIDField.value,
			RoleInFixture: roleInFixtureField.value,
			Goals: Number(goalsField.value),
			YellowCard: JSON.parse(yellowCardField.value),
			RedCard: JSON.parse(redCardField.value),
			ReplacedByPersonId: replacedByPersonIDField.value,
			ReplacedTime: Number(replacedTimeField.value)
		};

		//If the replaced by person id field is empty - ie a sub hasn't been picked
		//set the replacedByPersonID and ReplacedTime to null in the data variable
		if (replacedByPersonIDField.value == "") {
			data.ReplacedByPersonId = null;
			data.ReplacedTime = null;
		}

		var url = "/Participation/AddPersonToFixture";

		//Configuring the request
		const requestOptions = {
			method: 'POST', // Specify the request method
			headers: {
				'Content-Type': 'application/json',
				'__RequestVerificationToken': requestVerificationToken.value
			},
			body: JSON.stringify(data) // Convert the data to JSON format
		};

		//Make the post request
		fetch(url, requestOptions)
			.then(response => {
				// Check if the request was successful (status code 2xx)
				if (!response.ok) {
					throw response; // Will get picked up by the catch block
				}
				// Parse the response JSON
				return response.json();
			})
			.then(data => {
				//Handle the response data

				if (data.success) {
					//Person added succesfully to the fixture

					//Refresh the participants list
					refreshParticipantsPanel();

					//Eable the submit button and close the waiting dialog
					document.querySelector('#addPerson').disabled = false;
					closeDialog(waitingDialog);

					//Show the success dialog with the message from the server
					showDialog(successDialog, data.messageToUser);
				} else {
					//The server was succesfully reached and the request to add the person was done
					//However the person was not added successfully for some reason

					//Eable the submit button and close the waiting dialog
					document.querySelector('#addPerson').disabled = false;
					closeDialog(waitingDialog);

					//Show the warning dialog with the message from the server so the user knows what went wrong
					showDialog(warningDialog, data.messageToUser);
				}
			})
			.catch(error => {
				//Complete error - the server wasn't reached or just completely failed

				//Eable the submit button and close the waiting dialog
				document.querySelector('#addPerson').disabled = false;
				closeDialog(waitingDialog);

				//Show the warning dialog with the message from the server
				showDialog(warningDialog, error);
			});

	} 

	//Prevents the button from doing a form submit
	event.preventDefault();
}

function refreshParticipantsPanel() {
	// Refreshes the panel with the participants for the fixture
	fetch("/Participation/RefreshParticipantsPanel?fixtureId=" + document.getElementById('FixtureId').value).then(function (response) {
		return response.text();
	}).then(function (html) {
		//Puts the html returned from the view in the div
		var x = document.getElementById("participants");
		x.replaceChildren();
		x.innerHTML = html;
	});
}

function movePerson(participantId, direction) {
	//Moves a person up or down the list of participants

	//Gets the waiting dialog and displays it on the page
	const waitingDialog = document.getElementById("waitingDialog");
	showDialog(waitingDialog, "Moving person... please wait");

	//Gets the verification token from the form
	var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');

	//Set the data and the url for the api call
	var data = {
		Id: participantId
	};

	var url = "/Participation/MovePersonDown"

	//Sets the url based on the direction variable
	if (direction == "up") {
		url = "/Participation/MovePersonUp"
	}

	//Configuring the request
	const requestOptions = {
		method: 'POST', // Specify the request method
		headers: {
			'Content-Type': 'application/json',
			'__RequestVerificationToken': requestVerificationToken.value
		},
		body: JSON.stringify(data) // Convert the data to JSON format
	};

	//Make the post request
	fetch(url, requestOptions)
		.then(response => {
			// Check if the request was successful (status code 2xx)
			if (!response.ok) {
				throw response; //Will get picked up by the catch block
			}
			// Parse the response JSON
			return response.json();
		})
		.then(data => {
			//Handle the response data

			if (data.success) {
				//Person succesfully moved

				//Refresh the participants list
				refreshParticipantsPanel();

				//Close the waiting dialog
				closeDialog(waitingDialog);

				//Show the success dialog with the message from the server
				showDialog(successDialog, data.messageToUser);
			} else {
				//The server was succesfully reached and the request to move the person was done
				//However the person was not moved successfully for some reason

				//Close the waiting dialog
				closeDialog(waitingDialog);

				//Show the warning dialog with the message from the server so the user knows what went wrong
				showDialog(warningDialog, data.messageToUser);
			}
		})
		.catch(error => {
			//Complete error - the server wasn't reached or just completely failed

			//Close the waiting dialog
			closeDialog(waitingDialog);

			//Show the warning dialog with the message from the server
			showDialog(warningDialog, error);
		});


}

function editPerson(participantId) {
	// Refreshes the panel with the add participant form in it - replaces it with the edit form
	fetch("/Participation/EditParticipant?participantId=" + participantId).then(function (response) {
		return response.text();
	}).then(function (html) {
		//Puts the html returned from the view in the div
		var x = document.getElementById("addEditForm");
		x.replaceChildren();
		x.innerHTML = html;

		//Re-parse the new form - this ensures validation will work
		$.validator.unobtrusive.parse("#updateParticipant");
	});
}

function loadAddParticipantForm(fixtureID) {
	//Refreshes the panel with the add participant form in it - replaces it with the edit form
	fetch("/Participation/LoadAddParticipantForm?FixtureID=" + fixtureID).then(function (response) {
		return response.text();
	}).then(function (html) {
		//Puts the html returned from the view in the div
		var x = document.getElementById("addEditForm");
		x.replaceChildren();
		x.innerHTML = html;
	});
}

function removePerson(participantId) {
	//Removes a person from the fixture

	//Gets the waiting dialog and displays it on the page
	const waitingDialog = document.getElementById("waitingDialog");
	showDialog(waitingDialog, "Removing person... please wait");

	//Gets the verification token from the form
	var requestVerificationToken = document.querySelector('input[name="__RequestVerificationToken"]');

	//Set the data and the url for the api call
	var data = {
		Id: participantId
	};
	var url = "/Participation/RemovePersonFromFixture";

	//Configuring the request
	const requestOptions = {
		method: 'POST', // Specify the request method
		headers: {
			'Content-Type': 'application/json',
			'__RequestVerificationToken': requestVerificationToken.value
		},
		body: JSON.stringify(data) // Convert the data to JSON format
	};

	//Make the post request
	fetch(url, requestOptions)
		.then(response => {
			// Check if the request was successful (status code 2xx)
			if (!response.ok) {
				throw response; //Will get picked up by the catch block
			}
			// Parse the response JSON
			return response.json();
		})
		.then(data => {
			//Handle the response data

			if (data.success) {
				//Person removed succesfully to the fixture

				//Refresh the participants list
				refreshParticipantsPanel();

				//Close the waiting diaglog and show the success dialog
				closeDialog(waitingDialog);
				showDialog(successDialog, data.messageToUser);
			} else {
				//The server was succesfully reached and the request to remove the person was done
				//However the person was not removed successfully for some reason

				//Close the waiting dialog
				closeDialog(waitingDialog);

				//Show the warning dialog with the message from the server so the user knows what went wrong
				showDialog(warningDialog, data.messageToUser);
			}
		})
		.catch(error => {
			//Complete error - the server wasn't reached or just completely failed

			//Close the waiting dialog
			closeDialog(waitingDialog);

			//Show the warning dialog with the message from the server
			showDialog(warningDialog, error);
		});

}

function showDialog(dialog, text) {
	//Displays the modal that was sent and also replaces
	//the dialog-text span with the text that was sent as well
	var span = dialog.getElementsByClassName("dialog-text")[0];
	span.innerHTML = text;
	dialog.showModal();
}

function closeDialog(dialog) {
	//Closes the modal thwas sent
	dialog.close();
}