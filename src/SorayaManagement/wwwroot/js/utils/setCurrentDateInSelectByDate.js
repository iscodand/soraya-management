function setCurrentDateInSelectByDate() {
    const currentDate = new Date();

    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

// Set current date to createdAt filter on Views/Order/Orders
document.getElementById('createdAt').value = setCurrentDateInSelectByDate();