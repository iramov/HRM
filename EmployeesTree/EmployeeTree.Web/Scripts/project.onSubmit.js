//After the entering is done and before the submit of the page Reordering the indexes of the teams
$("#beginForm").on("submit", function () {
    $("[data-container]").each(function (index, value) {
        //$(this).prop("name").replaceWith("Members[" + index + "].Id");
        $(this).attr("name", "Teams[" + index + "].Id");

    });
})