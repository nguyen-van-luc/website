(function () {
    const endpoints = {
        add: '/Cart/Add',
        remove: '/Cart/Remove',
        update: '/Cart/Update'
    };

    const currencyFormatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 0
    });

    const showToast = (type, message) => {
        if (window.toastr) {
            toastr[type === 'error' ? 'error' : 'success'](message);
        } else {
            alert(message);
        }
    };

    const requestJson = async (url, payload) => {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) {
            let message = 'Có lỗi xảy ra. Vui lòng thử lại.';
            try {
                const data = await response.json();
                if (data?.message) {
                    message = data.message;
                }
            } catch {
                // ignore parse error
            }
            throw new Error(message);
        }

        return response.json();
    };

    const renderMiniCart = (data) => {
        const countEl = document.getElementById('cart-count');
        const subtotalEl = document.getElementById('mini-cart-subtotal');
        const listEl = document.querySelector('.minicart-list');

        if (countEl) {
            countEl.textContent = data.cartCount || 0;
        }

        if (subtotalEl) {
            subtotalEl.textContent = currencyFormatter.format(data.subtotal || 0);
        }

        if (listEl) {
            if (data.items && data.items.length) {
                listEl.innerHTML = data.items.map(item => {
                    const imagePath = item.image
                        ? `/assets/images/product/medium-size/${item.image}`
                        : '/assets/images/no-image.png';
                    return `
                        <li class="minicart-product">
                            <a class="product-item_remove btn-remove-cart" href="#" data-product-id="${item.productId}">
                                <i class="pe-7s-close"></i>
                            </a>
                            <a href="javascript:void(0)" class="product-item_img">
                                <img class="img-full" src="${imagePath}" alt="${item.productName || ''}">
                            </a>
                            <div class="product-item_content">
                                <a class="product-item_title" href="javascript:void(0)">${item.productName || ''}</a>
                                <span class="product-item_quantity">${item.quantity} x ${currencyFormatter.format(item.price || 0)}</span>
                            </div>
                        </li>`;
                }).join('');
            } else {
                listEl.innerHTML = '<li class="minicart-product">Giỏ hàng của bạn đang trống.</li>';
            }
        }
    };

    const renderCartPage = (data) => {
        const tableBody = document.getElementById('cart-page-body');
        const subtotalEl = document.getElementById('cart-page-subtotal');
        const totalEl = document.getElementById('cart-page-total');

        if (!tableBody) {
            return;
        }

        if (!data.items || !data.items.length) {
            window.location.reload();
            return;
        }

        tableBody.innerHTML = data.items.map(item => {
            const imagePath = item.image
                ? `/assets/images/product/small-size/${item.image}`
                : '/assets/images/no-image.png';

            return `
                <tr data-product-id="${item.productId}">
                    <td class="product_remove">
                        <button type="button" class="btn btn-link text-danger btn-remove-cart" data-product-id="${item.productId}">
                            <i class="pe-7s-close" title="Remove"></i>
                        </button>
                    </td>
                    <td class="product-thumbnail">
                        <img src="${imagePath}" alt="Cart Thumbnail" />
                    </td>
                    <td class="product-name">${item.productName || ''}</td>
                    <td class="product-price"><span class="amount">${currencyFormatter.format(item.price || 0)}</span></td>
                    <td class="quantity">
                        <div class="cart-plus-minus">
                            <input class="cart-plus-minus-box cart-quantity-input"
                                   type="number"
                                   min="1"
                                   value="${item.quantity}"
                                   data-product-id="${item.productId}" />
                        </div>
                    </td>
                    <td class="product-subtotal"><span class="amount">${currencyFormatter.format(item.total || 0)}</span></td>
                </tr>`;
        }).join('');

        if (subtotalEl) {
            subtotalEl.textContent = currencyFormatter.format(data.subtotal || 0);
        }

        if (totalEl) {
            totalEl.textContent = currencyFormatter.format(data.subtotal || 0);
        }
    };

    const handleAddToCart = async (productId, quantity) => {
        if (!productId) {
            return;
        }

        try {
            const data = await requestJson(endpoints.add, { productId, quantity });
            renderMiniCart(data);
            renderCartPage(data);
            showToast('success', 'Đã thêm sản phẩm vào giỏ hàng.');
        } catch (error) {
            showToast('error', error.message);
        }
    };

    const handleRemoveItem = async (productId) => {
        if (!productId) {
            return;
        }

        try {
            const data = await requestJson(endpoints.remove, { productId });
            renderMiniCart(data);
            renderCartPage(data);
            showToast('success', 'Đã xóa sản phẩm khỏi giỏ hàng.');
        } catch (error) {
            showToast('error', error.message);
        }
    };

    const handleUpdateItem = async (productId, quantity) => {
        if (!productId || !quantity || quantity < 1) {
            return;
        }

        try {
            const data = await requestJson(endpoints.update, { productId, quantity });
            renderMiniCart(data);
            renderCartPage(data);
            showToast('success', 'Đã cập nhật số lượng.');
        } catch (error) {
            showToast('error', error.message);
        }
    };

    document.addEventListener('click', (event) => {
        const addBtn = event.target.closest('.js-add-to-cart');
        if (addBtn) {
            event.preventDefault();
            const productId = parseInt(addBtn.dataset.productId, 10);
            let quantity = 1;
            if (addBtn.dataset.qtyInput) {
                const input = document.querySelector(addBtn.dataset.qtyInput);
                if (input) {
                    const parsed = parseInt(input.value, 10);
                    if (!isNaN(parsed) && parsed > 0) {
                        quantity = parsed;
                    }
                }
            }
            handleAddToCart(productId, quantity);
            return;
        }

        const removeBtn = event.target.closest('.btn-remove-cart');
        if (removeBtn) {
            event.preventDefault();
            const productId = parseInt(removeBtn.dataset.productId, 10);
            handleRemoveItem(productId);
        }
    });

    document.addEventListener('change', (event) => {
        const qtyInput = event.target.closest('.cart-quantity-input');
        if (qtyInput) {
            const productId = parseInt(qtyInput.dataset.productId, 10);
            const quantity = parseInt(qtyInput.value, 10);
            if (productId && quantity > 0) {
                handleUpdateItem(productId, quantity);
            }
        }
    });
})();

