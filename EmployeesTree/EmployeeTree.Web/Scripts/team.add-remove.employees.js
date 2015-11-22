//Adding and removing employees
$(function () {
    $('#template-row').hide();

    //Taking the last index in the table of the current employees if we are making an edit and
    //for new record index = 0
    if ($("#beginForm").attr("class") == "createForm") {
        index = 0;
    }
    else {
        var index = $('table.table tr').length;
    }
    
    //Таking the template row replacing the word index with number and appending the new employee in the table
    //as new row with DropDownList
    $('#add-member-btn').on('click', function () {
        var html = $('#template-row').html();
        html = html.replace(/index/g, index);
        $('#tableEmployees').append('<tr>' + html + '</tr>');
        index++;
    });
    //button event for removing a row with employee
    $("#tableEmployees").on("click", "#delete-employee-btn", function () {
        $(this).closest('tr').remove();
    });
});