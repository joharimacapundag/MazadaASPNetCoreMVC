$("#save-btn").click(async (event) => {
    event.preventDefault();

    // Get the actual/native form [0]
    let form = $("#edit-form")[0]; 
    let url = $(form).attr("action");

    // Clear previous display errors
    $(".error").text("");

    const formData = new FormData(form); // This is an envelope with the file, a dictionary
    const response = await fetch(url, {
        method: "POST",
        body: formData,
    });

    // Parse the JSON response from controller
    const result = await response.json();

    // Check if the response from controller is ok otherwise handles errors
    if (response.ok) 
    {
        // Show status modal if success 
        showStatusModal("Success", result.message);
    } 
    else 
    {
        // Check if there is a "field" in the response
        if (result.field) {
            $("#" + result.field + "-error").text(result.message);
        }
        else 
        {
            alert("Error: " +  result.message);
        }
    }

    

});