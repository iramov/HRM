﻿//Adding and removing employees
$(function () {
    $('#template-row').hide();

    //Taking the last index in the table of the current employees
    var index = $('table.table tr').length;
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
