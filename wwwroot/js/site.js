// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function showStatusModal(title, message, redirectUrl = null) { // redirectUrl is optional
    let modalElement = document.querySelector("#status-modal");

    // Set the text dynamically
    document.querySelector("#status-modal-title").innerText = title;
    document.querySelector("#status-modal-message").innerText = message;

    let modal = new bootstrap.Modal(modalElement);

    // Listen for the modal closing
    modalElement.addEventListener("hidden.bs.modal", () => {
        if (redirectUrl)
        {
            window.location.href = redirectUrl; // Go to the page from controller json results redirectUrl
        } 
        else 
        {
            // If no redirect, just reload the current page
            location.reload();
        }
    }, { once: true });

    modal.show();
}


async function updateCartCount() {
    await $.get("/cart/count", function(result) {
        $("#cart-count").text(result.count);
        
        if (result.count > 0){
            $("#cart-count").show();
        }
        else
        {
            $("#cart-count").hide();
        }
    });
}