$("#search-btn").click(async (event) => {
    event.preventDefault();

    let form = $("#search-form");
    let url = form.attr("action");
    let formData = form.serialize(); // query=...

    await $.get(url, formData, (result) => {
        // Go to the success link query=...
        window.location.href = url + "?" + formData;
        // new bootstrap.Modal("#successModal").show();

    }).fail(function (err) {
        // Check if there is a "field" in the response
        if (err.responseJSON.field) {
            $("#" + err.responseJSON.field + "-error").text(err.responseJSON.message);
        } else {
            alert("Error: " + err.responseJSON.message);
        }
    });

});

$("#shop-btn").click(async function (event) {
    event.preventDefault();

    let url = $(this).attr("href");

    await $.get(url, (result) => {
        // show status modal if success
        window.location.href = url;
        // showStatusModal("Success", result.message, result.redirectUrl);

    }).fail(function (err) {
        // Check if there is a "field" in the response
        if ( err.responseJSON && err.responseJSON.field) {
            $("#" + err.responseJSON.field + "-error").text(err.responseJSON.message);
        } else {
            alert("Error: " + err.responseJSON.message);
        }
    });

});

$("#logout-btn").click(async function (event) {
    event.preventDefault();

    let url = $(this).attr("href");

    await $.get(url, (result) => {
        // show status modal if success
        showStatusModal("Success", result.message, result.redirectUrl);

    }).fail(function (err) {
        // Check if there is a "field" in the response
        if (err.responseJSON.field) {
            $("#" + err.responseJSON.field + "-error").text(err.responseJSON.message);
        } else {
            alert("Error: " + err.responseJSON.message);
        }
    });

});