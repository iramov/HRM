$(function () {
    $('#template-row').hide();
    var index = 0;
    $('#add-member-btn').on('click', function () {
        var html = $('#template-row').html();
        html = html.replace(/index/g, index);
        $('#tableEmployees').append('<tr>' + html + '</tr>');
        index++;
    });

    $("#tableEmployees").on("click", "#delete-employee-btn", function () {
        $(this).closest('tr').remove();
    });
});

$("#beginForm").on("submit", function () {
    $("[data-container]").each(function (index, value) {
        //$(this).prop("name").replaceWith("Members[" + index + "].Id");
        $(this).attr("name", "Members[" + index + "].Id");

    });
})
