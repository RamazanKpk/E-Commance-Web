document.getElementById('selectAll').addEventListener('click', function () {
    var checkboxes = document.querySelectorAll('.selectRow');
    checkboxes.forEach(function (checkbox) {
        checkbox.checked = document.getElementById('selectAll').checked;
    });
});
function getSelectRows() {
    var selected = [];
    var checkboxes = document.querySelectorAll('.selectRow:checked');
    checkboxes.forEach(function (checkbox) {
        selected.push(checkbox.getAttribute('data-id'));
    });
    return selected;
}
//document.getElementById('editButton').addEventListener('click', function () {
//    var selected = getSelectRows();
//    if (selected.length == 1) {
//        window.location.href = "UserFavoriteProduct/Edit" + '/' + selected[0];
//    } else {
//        alert('Please select exactly one user row to edit.');
//    }
//});
document.getElementById('detailsButton').addEventListener('click', function () {
    var selected = getSelectRows();
    if (selected.length == 1) {
        window.location.href = 'UserFavoriteProduct/Details' + '/' + selected[0];
    } else {
        alert('Please select exactly one user row to view details.');
    }
});
document.getElementById('deleteButton').addEventListener('click', function () {
    var selected = getSelectRows();
    if (selected.length > 0) {
        if (confirm('Are you sure you want to delete the selected row(s)?')) {
            fetch('UserFavoriteProduct/Delete', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(selected)
            })
                .then(response => response.json())
                .then(data => {
                    alert('Deletion successful', data);
                    console.log('Deletion successful', data);
                    location.reload();
                })
                .catch(error => {
                    alert('Error deleting!');
                });
        }
    } else {
        alert('Please select at least one row to delete.');
    }
});
