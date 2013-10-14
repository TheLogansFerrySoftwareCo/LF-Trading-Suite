$(document).ready(function() {

    // Update the Is Excluded flag of a selected stock when the user changes the Is Excluded check box.
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
});