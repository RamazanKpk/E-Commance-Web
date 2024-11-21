$(document).ready(function () {
    fetchProductData();
    clickDropdown()
    function fetchProductData() {
        $.ajax({
            url: 'GetProduct/Product',
            type: 'GET',
            data: {},
            dataType: 'json',
            success: function (data) {

            },
            error: function (xhr, status, error) {
                console.error('AJAX hatası:', status, error);
            }
        });
    }
    function clickDropdown() {
        $('.category-link').on('click', function (e) {
            var target = $(this).attr('href');
            if ($(target).length) {
                e.preventDefault();
                $(target).collapse('toggle');
            }
        });
    }

});