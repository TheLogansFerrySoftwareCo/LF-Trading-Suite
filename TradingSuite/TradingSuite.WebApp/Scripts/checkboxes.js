// Note:  This code was shamelessly stolen from http://jsfiddle.net/xixonia/WnbNC/

(function ($) {

    $.fn.checked = function (value) {

        if (value === true || value === false) {
            // Set the value of the checkbox
            $(this).each(function () { this.checked = value; });

        } else if (value === undefined || value === 'toggle') {

            // Toggle the checkbox
            $(this).each(function () { this.checked = !this.checked; });
        }

    };

})(jQuery);