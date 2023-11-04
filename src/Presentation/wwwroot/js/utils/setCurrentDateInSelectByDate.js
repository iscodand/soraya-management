function setCurrentDateInSelectByDate() {
    const currentDate = new Date();

    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

let createdAtFilter = document.getElementById('createdAt');

if (createdAtFilter) {
    // Set current date to createdAt filter on Views/Order/Orders.cshtml
    createdAtFilter.value = setCurrentDateInSelectByDate();
} else {
    // Set current date to initial and final dates filter on Views/Home/Home.cshtml
    // document.getElementById('initialDate').value = setCurrentDateInSelectByDate();
    // document.getElementById('finalDate').value = setCurrentDateInSelectByDate();
}
