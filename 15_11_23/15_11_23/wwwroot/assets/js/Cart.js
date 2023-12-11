const cartItemHolder = document.querySelector(".cart-item-holder");
const addToCartButtons = document.querySelectorAll(".add-to-cart");
const deleteFromCartButtons = document.querySelectorAll(".delete-from-cart");
const cartCountElementi = document.querySelector(".cartItemCounti");
const cartCountElementI = document.querySelector(".cartItemCountI");
const ammount = document.querySelector(".all-ammount");

addToCartButtons.forEach(button =>
    button.addEventListener("click", ev => {
        ev.preventDefault();

        const href = ev.target.parentElement.getAttribute("href");

        fetch(href)
            .then(res => res.text())
            .then(data => {
                cartItemHolder.innerHTML = data;
                updateCartItemCount();
                ammountValueGet()
            })
            .catch(error => console.error("Error fetching data:", error));
    })
);


deleteFromCartButtons.forEach(button =>
    button.addEventListener("click", ev => {
        ev.preventDefault();

        const href = ev.target.parentElement.getAttribute("href");

        fetch(href)
            .then(res => res.text())
            .then(data => {
                cartItemHolder.innerHTML = data;
                updateCartItemCount();
                ammountValueGet()
            })
            .catch(error => console.error("Error fetching data:", error));
    })
);

function updateCartItemCount() {
    const cartItems = document.querySelectorAll(".getCartItemCount");
    cartItems.forEach(item => {
        const countValue = item.dataset.count;
        cartCountElementi.textContent = countValue;
        cartCountElementI.textContent = countValue;

    });

}

function ammountValueGet() {
    const cartItems = document.querySelectorAll(".getAmount");
    cartItems.forEach(item => {
        const ammountValue = item.dataset.ammount;
        ammount.textContent = ammountValue;
    });

}
