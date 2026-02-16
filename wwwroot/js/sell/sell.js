$("#add-product-btn").click(async function (event) {
    event.preventDefault();

    // Get the actual/native form [0]
    let form = $("#sell-form")[0]; 
    let url = $(form).attr("action");

    // Clear previous display errors
    $(".error").text("");

    const formData = new FormData(form); // This is an envelope with the file, a dictionary
    const response = await fetch(url, {
        method: "POST",
        body: formData
    });

    // Parse the JSON response from controller
    const result = await response.json();

    // Check if the response from controller is ok otherwise handles errors
    if (response.ok) 
    {
        // Show bootstrap modal
        showStatusModal("Success", result.message, result.redirectUrl);
    } 
    else 
    {
        // Check if there is a "field" in the result
        if (result.field) {
            $("#" + result.field + "-error").text(result.message);
        }
        else 
        {
            alert("Error: " +  result.message);
        }
    }

});

