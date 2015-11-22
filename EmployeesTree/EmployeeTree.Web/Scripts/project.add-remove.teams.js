//Adding and removing employees
$(function () {
    $('#template-row').hide();

    //Taking the last index in the table of the current employees
    var index = $('table.table tr').length;
    //Таking the html template row replacing the word index with number and appending the new employee in the table
    //as new row with DropDownList
    $('#add-team-btn').on('click', function () {
        var html = $('#template-row').html();
        html = html.replace(/index/g, index);
        $('#tableTeams').append('<tr>' + html + '</tr>');
        index++;
    });

    $("#tableTeams").on("click", "#delete-team-btn", function () {
        $(this).closest('tr').remove();
    });
});