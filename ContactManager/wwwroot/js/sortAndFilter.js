$(document).ready(function () {
    $("#searchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#contactsTable tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
});

var sortOrder = true;

function sortTable(columnIndex) {
    var table = document.getElementById("contactsTable");
    var tbody = table.getElementsByTagName('tbody')[0];
    var rows = Array.from(tbody.rows);

    sortOrder = !sortOrder;

    rows.sort(function (a, b) {
        var cellA = a.cells[columnIndex].textContent.trim();
        var cellB = b.cells[columnIndex].textContent.trim();

        if (isValidDate(cellA) && isValidDate(cellB)) {
            return compareDates(cellA, cellB);
        } else if (isNumeric(cellA) && isNumeric(cellB)) {
            return compareNumbers(cellA, cellB);
        } else {
            return compareStrings(cellA, cellB);
        }
    });
    
    while (tbody.firstChild) {
        tbody.removeChild(tbody.firstChild);
    }
    
    rows.forEach(function (row) {
        tbody.appendChild(row);
    });
}

function isNumeric(value) {
    value = value.replace(/[^0-9.-]+/g,"");
    return !isNaN(parseFloat(value)) && isFinite(value);
}

function isValidDate(dateString) {
    var date = new Date(dateString);
    return !isNaN(date.getTime());
}

function parseDate(dateString) {
    return new Date(dateString).getTime();
}

function compareDates(a, b) {
    var dateA = parseDate(a);
    var dateB = parseDate(b);
    return sortOrder ? dateA - dateB : dateB - dateA;
}

function compareNumbers(a, b) {
    var numA = parseFloat(a.replace(/[^0-9.-]+/g,""));
    var numB = parseFloat(b.replace(/[^0-9.-]+/g,""));
    return sortOrder ? numA - numB : numB - numA;
}

function compareStrings(a, b) {
    return sortOrder ? a.localeCompare(b) : b.localeCompare(a);
}
