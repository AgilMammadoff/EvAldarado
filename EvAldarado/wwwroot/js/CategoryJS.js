$(document).ready(function () {
    GetCategory();
});

/* Read Data */
function GetCategory() {
    $.ajax({
        url: '/category/GetCategory',
        type: 'get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                var object = '';
                object += '<tr>';
                object += '<td colspan="4">' + 'Category not available' + '</td>'; // Corrected colspan and closed <td> tag
                object += '</tr>'; // Closed <tr> tag
                $('#tblbody').html(object); // Added missing closing parenthesis
            } else {
                var object = ''; // Moved object initialization here to avoid scoping issues
                $.each(response, function (index, item) {
                    object += '<tr>';
                    object += '<td>' + item.id + '</td>';
                    object += '<td>' + item.name + '</td>'; // Added missing concatenation operator (+)
                    object += '<td>' + item.isActive + '</td>'; // Added missing concatenation operator (+)
                    object += '<td>'; // Added opening <td> tag for edit, delete buttons
                    object += '<button onclick="EditCategory(' + item.id + ')">Edit</button>'; // Assuming you have an EditCategory function
                    object += '<button onclick="DeleteCategory(' + item.id + ')">Delete</button>'; // Assuming you have a DeleteCategory function
                    object += '</td>'; // Closed <td> tag for edit, delete buttons
                    object += '</tr>';
                });
                $('#tblbody').html(object); // Moved here to set the table body content once all rows are created
            }
        },
        error: function () {
            alert('Unable');


        }
    });
}
