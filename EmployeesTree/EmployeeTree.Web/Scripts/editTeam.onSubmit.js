//After the entering is done and before the submit of the page Reordering the indexes of the employees
//and checking if the user wants to delete the current team project
$("#beginForm").on("submit", function () {
    if ($("#cbProject").is(":checked")) {
        $("[data-project]").attr("disabled", "disabled");
    }

    $("[data-container]").each(function (index, value) {
        $(this).attr("name", "Members[" + index + "].Id");
    });
})