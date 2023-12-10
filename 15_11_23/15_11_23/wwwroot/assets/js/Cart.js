$(".delete-button").on("click", function (e) {
    e.preventDefault();
    var itemId = $(this).data("item-id");

    $.post("/Cart/DeleteItem/" + itemId, function (data) {
        $("#cart-container").html(data);
    });
});

