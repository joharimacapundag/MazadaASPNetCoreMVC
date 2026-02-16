$(document).ready(function () {
    // Handles row removal
    $(".row-remove-btn").click(async function (event) {
        // Prevent to go other link
        event.preventDefault();

        // Get the url or href from this element when clicked
        let url = $(this).attr("href");
        let rowId = $(this).data("id");

        // Get the json result from cart remove controller 
        await $.get(url, function (result) {

            remove(rowId);
            updateTotalPrice();
            updateCartCount();
            alert("Success: " + result.message);


        }).fail(function (err) {
            
            alert("Error: " + err.responseJSON.message);
        });
    });

});

// Remove row 
function remove(row) {
    $("#row-" + row).remove();
}

function onChange(row) {
    updateRowTotalPrice(row);
    updateTotalPrice();

};

function updateRowTotalPrice(row) {
    let quantity = $("#row-quantity-" + row).val();
    let productPrice = $("#row-product-price-" + row).data("product-price") || 0;
    let totalPrice = parseInt(quantity) * parseFloat(productPrice);
    // Update the row total price
    $("#row-total-price-" + row).data("row-total-price", totalPrice.toFixed(2));
    $("#row-total-price-" + row).text("₱" + totalPrice.toFixed(2));
}

// Update the all total
function updateTotalPrice() {
    // Initialize total price
    let totalPrice = 0;

    // Get all the row totals
    $(".row-total-price").each(function () {

        let val = $(this).data("row-total-price");
        totalPrice += parseFloat(val) || 0;
    });

    // Update the ui total price 
    $("#total-price").text("$" + totalPrice.toFixed(2));
}
