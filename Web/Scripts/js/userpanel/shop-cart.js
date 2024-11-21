$(document).ready(function () {

        $('.cart-btn').on('click', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            $.ajax({
                url: '/ShopCart/AddCart',
                type: 'POST',
                data: { Id: productId },
                success: function (response) {
                    if (response.success) {
                        location.reload();
                    } else {
                        alert('Ürün eklenirken bir hata oluştu');
                    }
                },
                error: function () {
                    alert('Bir hata oluştu');
                }
            });
        });
    $('.cart-close-btn').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id');
        $.ajax({
            url: '/ShopCart/DeleteCart',
            type: 'POST',
            data: { Id: productId },
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    location.reload();
                }
            },
            error: function () {
                alert('Bir hata oluştu');
            }
        });
    });

     $('.qty-up').on('click', function () {
            var input = $(this).parent().prev('.quantity').find('.qty-input');
            var currentValue = parseInt(input.val());
            input.val(currentValue + 1);
            updateCartItem(input);
        });

        // Eksi butonuna tıklandığında
        $('.qty-down').on('click', function () {
            var input = $(this).parent().prev('.quantity').find('.qty-input');
            var currentValue = parseInt(input.val());
            if (currentValue > 1) {
                input.val(currentValue - 1);
                updateCartItem(input);
            }
        });

        // Input alanında herhangi bir değişiklik olduğunda
        $('.qty-input').on('change', function () {
            updateCartItem($(this));
        });

        // Sepet öğesinin güncellenmesi işlevi
        function updateCartItem(input) {
            var quantity = parseInt(input.val());
            var price = parseFloat(input.data('price'));
            var total = quantity * price;
            input.closest('tr').find('.cart__total-item').text(total.toFixed(2));

            // Toplam tutarların güncellenmesi
            var subtotal = 0;
            $('.cart__total-item').each(function () {
                subtotal += parseFloat($(this).text().replace('$', '').replace(',', ''));
            });
            $('.cart-subtotal').text('$' + subtotal.toFixed(2));
            $('.cart-total').text('$' + subtotal.toFixed(2));
        }
});