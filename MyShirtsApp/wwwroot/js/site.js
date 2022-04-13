function changeVisibility(event) {
    let tr = document.getElementById(event);
    let changeVisibilityButton = tr.querySelector("a.change-visibility");
    let isPublic = tr.querySelector("td.shirt-public");
    isPublic.textContent = isPublic.textContent == "Yes" ? "No" : "Yes"
    if (isPublic.classList.contains("text-success")) {
        isPublic.classList.replace("text-success", "text-danger");
        changeVisibilityButton.textContent = "Show";
        changeVisibilityButton.classList.replace("bgcolor-orange", "btn-success");
    } else {
        isPublic.classList.replace("text-danger", "text-success");
        changeVisibilityButton.textContent = "Hide";
        changeVisibilityButton.classList.replace("btn-success", "bgcolor-orange");
    }

    $.ajax({
        type: "GET",
        url: `/Admin/Shirts/ChangeVisibility/${event}`
    })
}

function onDelete(event) {
    let choice = confirm("Are you sure ?");

    if (choice) {
        $.ajax({
            type: "GET",
            url: `/Shirts/Delete/${event}`
        })

        document.getElementById(event).remove();
    }
}

function onDelivery(event) {
    document.getElementById("with-card").style.display = "none";
    document.getElementById("pay-on-delivery-div").style.display = "block";
}

function withCard(event) {
    document.getElementById("with-card").style.display = "block";
    document.getElementById("pay-on-delivery-div").style.display = "none";
}

function clearCart() {
    let choice = confirm("Are you sure ?")

    if (choice) {
        document.querySelectorAll(".shirt-element").forEach(x => x.remove())
        document.getElementById("empty-cart-message").style.display = "block";
        document.getElementById("non-empty-cart-info").style.display = "none";
    }

    $.ajax({
        type: "GET",
        url: "/Carts/Clear",
    })
}

function deleteShirtFromCart(event) {
    let choice = confirm("Are you sure ?");
    let shirtElement = document.getElementById(event).closest(".shirt-element");

    if (choice) {
        let sizeName = shirtElement.querySelector("input.size-name").value.replace(" ", "-");

        $.ajax({
            type: "GET",
            url: `/Carts/DeleteShirt?shirtId=${event}&sizeName=${sizeName}&flag=True`
        })

        let count = parseInt(shirtElement.querySelector("input.count").value);

        let priceElement = shirtElement.querySelector(".price");

        let price = parseFloat(priceElement.textContent.substring(1, priceElement.textContent.indexOf("/"))) * count;

        let totalPriceElement = document.getElementById("total-price");

        let totalPrice = parseFloat(totalPriceElement.textContent.substring(14));

        totalPriceElement.textContent = `Total Price: $${totalPrice - price}`;

        shirtElement.remove();

        let elementsCount = document.querySelectorAll(".shirt-element").length;

        if (elementsCount == 0) {
            document.getElementById("empty-cart-message").style.display = "block";
            document.getElementById("non-empty-cart-info").style.display = "none";
        }
    }
}

function removeShirtFromCart(event) {
    let choice = confirm("Are you sure ?");
    let shirtElement = document.getElementById(event).closest(".shirt-element");
    let sizeName = shirtElement.querySelector("input.size-name").value

    if (choice) {
        $.ajax({
            type: "GET",
            url: `/Carts/DeleteShirt?shirtId=${event}&sizeName=${sizeName}&flag=True`
        })

        shirtElement.remove();

        let elementsCount = document.querySelectorAll(".shirt-element").length;
        let infoMessage = document.getElementById("info-message");

        infoMessage.classList.replace("text-danger", "text-success");
        infoMessage.textContent = ":("

        if (elementsCount == 0) {
            setTimeout(function () {
                document.location.href = "/Carts/Mine";
            }, 1000)
        }
    }
}

function deleteUnnecessary(event) {
    let shirtElement = document.getElementById(event).closest(".shirt-element");
    let sizeName = shirtElement.querySelector("input.size-name").value

    $.ajax({
        type: "GET",
        url: `/Carts/DeleteShirt?shirtId=${event}&sizeName=${sizeName}&flag=False`
    })

    shirtElement.remove();

    let elementsCount = document.querySelectorAll(".shirt-element").length;
    let infoMessage = document.getElementById("info-message");

    infoMessage.classList.replace("text-danger", "text-success");
    infoMessage.textContent = "Good..."

    if (elementsCount == 0) {
        setTimeout(function () {
            document.location.href = "/Carts/Mine";

        }, 1000)
    }
}

function onDeleteFromDetails(event) {
    let choice = confirm("Are you sure ?");

    if (choice) {
        $.ajax({
            type: "GET",
            url: `/Shirts/Delete/${event}`
        })

        document.querySelector("main").innerHTML = `<h2 class="alert-success text-center">Successfully deleted !</h2>`;

        setTimeout(function () {
            window.location.href = "/Shirts/Mine";
        }, 1000);
    }
}

function details(event) {
    window.location.href = `/Shirts/Details/${event.id}/${event.alt.replace(" ", "-")}`
}

function addOrRemoveFromFavorites(event, isMine) {
    let anchors = event.parentElement.querySelectorAll("a");
    let params = event.id;
    let id = params.slice(0, params.indexOf("-"));
    let name = params.slice(params.indexOf("-") + 1);

    if (anchors[0].classList.contains("unfav")) {
        anchors[0].classList.replace("unfav", "fav");
        anchors[1].classList.replace("fav", "unfav");
    } else {
        anchors[0].classList.replace("fav", "unfav");
        anchors[1].classList.replace("unfav", "fav");
    }

    let isMinePage = isMine;

    if (isMinePage == true) {
        let target = document.getElementById(id);
        target.animate({ opacity: '0' }, 500);
        setTimeout(function () {
            target.remove();
            let divs = document.querySelectorAll("div.shirt-element");
            if (divs.length == 0) {
                document.getElementById("no-shirts-message").style.display = "block";
            }
        }, 400)
    }

    $.ajax({
        type: "GET",
        url: `/Favorites/Action/${id}/${name}`,
    })
}

function addToCart(event) {
    let params = event.id;
    let id = params.slice(0, params.indexOf("-"));
    let size = params.slice(params.indexOf("-") + 1);
    let buttons = document.querySelectorAll("a.rounded-circle");
    buttons.forEach(x => x.classList += " custom-disabled");

    $.ajax({
        type: "GET",
        url: `/Carts/Add/${id}?size=${size}`,
    })
    let message = document.getElementById("message");
    message.parentElement.style.display = "inline-block";

    setTimeout(function () {
        message.parentElement.style.display = "none";
        buttons.forEach(x => x.classList.remove("custom-disabled"));
    }, 1000);
}