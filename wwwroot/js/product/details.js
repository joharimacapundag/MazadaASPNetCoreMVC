
const max = parseInt($("#quantity").attr("max"));
const min = parseInt($("#quantity").attr("min"));

$("#quantity").change(() => {

    // Get the current quantity value
    let quantity = $("#quantity").val();

    // Check the current quantity value if is greater than product stocks or not
    if (quantity > max) {

        // Set quantity value to max stocks when current value is higher
        $("#quantity").val(max);

    } else if (quantity < min) {

        // Set quantity value to min when current value is lesser than or equal to 0
        $("#quantity").val(min);
    }

});

$("#plus-btn").click(() => {
    // Get the current quantity value
    let quantity = parseInt($("#quantity").val());

    // Increment current value until max otherwise set to max
    quantity += 1;
    if (quantity >= max) {
        $("#quantity").val(max);
    } else {
        $("#quantity").val(quantity);
    }


});


$("#minus-btn").click(() => {
    // Get the current quantity value
    let quantity = parseInt($("#quantity").val());
    
    // Decrement current value until min otherwise set to min
    quantity -= 1;
    if (quantity < min) {
        $("#quantity").val(min);
    } else {
        $("#quantity").val(quantity);
    }
});


$("#add-to-cart-btn").click( async function (event) {
    // Prevent going to other link
    event.preventDefault();
    // Get the last/current quantity
    const quantity = parseInt($("#quantity").val());

    // Get the href of this button
    let url = $(this).attr("href");

    // Replace the quantity placeholder in the link
    url = url.replace("__QUANTITY__", quantity);

    // Get the json result from the cart controller
    await $.get(url, function(result){
        updateCartCount();
        alert("Success: " + result.message);
    }).fail( function (err) {  
        alert("Error: " + err.responseJSON.message);
    });
});

$("#buy-btn").click( function (event) {
    // Prevent going to other link
    event.preventDefault();
    // Get the last/current quantity
    const quantity = parseInt($("#quantity").val());

    // Get the href of this button
    let url = $(this).attr("href");

    // Replace the quantity placeholder in the link
    url = url.replace("__QUANTITY__", quantity);

    // Go to that link
    window.location.href = url;
});