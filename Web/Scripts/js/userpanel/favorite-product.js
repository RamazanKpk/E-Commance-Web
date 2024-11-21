$(document).ready(function () {
    $('.heart-btn').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');
        $.ajax({
            url: '/FavoriteProduct/AddFavoriteProduct',
            type: 'POST',
            data: { Id: productId },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Bir hata oluştu');
            }
        });
    });
    $('.close-btn').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');
        $.ajax({
            url: '/FavoriteProduct/RemoveFavoriteProduct',
            type: 'POST',
            data: { Id: productId },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Bir hata oluştu');
            }
        });
    });
});