//Runs the penalties required function when the page loads
penaltiesRequired();

function penaltiesRequired()
{
    //Gets the penalties Fields Div
    var div = document.getElementById("PenaltyFields");

    //If the PenaltiesRequired dropdown menu is set to true 
    //Display the penalties fields
    if (document.getElementById("PenaltiesRequired").value == 'true') {
        //Show the penalty fields
        div.style.display = "block";

        console.log("Penalties required");
    }
    else {
        //PenaltiesRequired dropdown menu was set to false
        //Hide the penalties fields
        div.style.display = "none";

        console.log("Penalties not required");
    }
 }