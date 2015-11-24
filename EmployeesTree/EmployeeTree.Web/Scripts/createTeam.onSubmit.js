//After the entering is done and before the submit of the page Reordering the indexes of the employees
$("#beginForm").on("submit", function () {
    $("[data-container]").each(function (index, value) {
         $(this).attr("name", "Members[" + index + "].Id");
        
    });
})