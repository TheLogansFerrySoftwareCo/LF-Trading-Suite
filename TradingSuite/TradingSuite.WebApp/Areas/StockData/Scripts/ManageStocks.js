$(document).ready(function() {

    // Checkbox_Changed
    // Invoke the Update Is Excluded controller action via AJAX.
    $("input[name='Value.IsExcluded']").change(function () {
        var selectionJson = { ticker: $(this).val(), isExcluded: $(this).is(":checked") };

        $.ajax({
            type: "POST",
            traditional: true,
            url: '/StockData/ManageStocks/UpdateIsExcluded',
            data: selectionJson,
            dataType: "json",
            success: function (data) {
                if (!data) {
                    alert('Error updating the selected Is Excluded flag.');
                }
            }
        });
    });

    // UpdateButton_Click
    // Invoke the Update Stocks controller action via AJAX.
    $("#updateButton").click(function () {
        $.ajax({
            type: "POST",
            traditional: true,
            url: '/StockData/ManageStocks/UpdateStocks',
            dataType: "json",
            success: function (data) {
                alert(data + " stocks were added.");
                window.location = '/StockData/ManageStocks';
            }
        });
    });
});