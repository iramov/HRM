//After the entering is done and before the submit of the page Reordering the indexes of the employees
$("#beginForm").on("submit", function () {
    //count = 0;
    $("[data-container]").each(function (index, value) {
        //if ($(this).find("option:selected").text() == "Select team member") {
        //    $(this).remove();
        //}
        //else {
        //    $(this).attr("name", "Members[" + count + "].Id");
        //    count++;
        //}
        $(this).attr("name", "Members[" + index + "].Id");
        
    });
})