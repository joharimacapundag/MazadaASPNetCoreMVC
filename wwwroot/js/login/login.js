document.addEventListener("DOMContentLoaded", function () {

    $("#login-btn").click(async (event) => {
        event.preventDefault();

        let form = $("#login-form");
        let url = form.attr("action");
        let formData = form.serialize(); // ?x=...&y=...&...

        // Clear previous display errors
        $(".error").text("");
        await $.post(url, formData, (result) => {

            // show status model if success
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

})


