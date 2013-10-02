$(document).ready(function () {

    // Click event for the "Flag as Excluded" link.
    $("a#flag_excluded_action").click(function (e) {

        // Cancel the default click event
        e.preventDefault();

        // Create an array of ticker symbols that were checked by the user and package them into a json object.
        var checkedTickers = [];
        $("input[name='Value.IsExcluded']:checked").map(
            function () { checkedTickers.push($(this).val()); }
        );
        var tickersJson = { tickers: checkedTickers };

        // POST the tickers to the server to be flagged as excluded.
        $.ajax({
            type: "POST",
            traditional: true,
            url: $(this).attr('href'),
            data: tickersJson,
            dataType: "json",
            success: function (data) {
                // Remove the ticker symbols from the page that were successfully processed by the server.
                $.each(data, function (index, item) {
                    console.log("index: " + index + "; item: " + item);
                    $("input[value=" + item + "]").closest("tr").remove();
                });

                if ($("#ticker_update_errors tr").length <= 1) {
                    $("#ticker_update_errors").replaceWith($("<p>No failed tickers are available.</p>"));
                }
            }
        });
    });

    // Click event for Select All
    $("#select_all").click(function (e) {

        // Cancel the default click event.
        e.preventDefault();

        // Check all checkboxes
        $("input[name='Value.IsExcluded']").checked(true);
    });

    // Click event for Select None
    $("#select_none").click(function (e) {

        // Cancel the default click event.
        e.preventDefault();

        // Check all checkboxes
        $("input[name='Value.IsExcluded']").checked(false);
    });
});