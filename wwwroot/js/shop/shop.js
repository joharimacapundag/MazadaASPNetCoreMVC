// On shop search input change
let timer;
$("#shop-search").on("input", function (e) {
    // Clear timeout to make sure the timer always reset so to avoid sending multiple request by a character count sent
    clearTimeout(timer);
    const query = $(this).val(); // Get the query from input

    // Set timer with a delay 300 ms before executing the actual get
    timer = setTimeout(async function () {
        await $.get("/shop/search?query=" + query, function (result) {
            $("#product-grid").html(result);
        }).fail(function (err) {
            $("#product-grid").html(err.responseJSON.message);
        })

    }, 300);

});

$(".category-btn").click(async function (event) {
    event.preventDefault();
    // Highlight the selected button
    $(".category-btn").removeClass("active");
    $(this).addClass("active");

    // get the url 
    let url = $(this).attr("href");

    await $.get(url,
        function (result) {
            $("#product-grid").html(result);
        },
    ).fail(function (err) {
        // Check if there is a "field" in the response
        if (err.responseJSON.field) {
            $("#" + err.responseJSON.field + "-error").text(err.responseJSON.message);

        } else {
            alert("Error: " + err.responseJSON.message);
        }

    });
});


