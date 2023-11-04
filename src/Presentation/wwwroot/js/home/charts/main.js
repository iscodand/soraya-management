import { createChart } from "./createChart.js";

// When DOM loads, get data and create chart
window.addEventListener('DOMContentLoaded', () => {
    createChart(dateRangeSelected.value);
});

// Filter by date in select
let dateRangeSelected = document.getElementById("dateRangeSelected");
$(dateRangeSelected).on("change", function () {
    createChart(dateRangeSelected.value);
});

// $(initialDate).on("change", function () {
//     createChart(dateRangeSelected.value, initialDate.value, finalDate.value);
// });

// $(finalDate).on("change", function () {
//     createChart(dateRangeSelected.value, initialDate.value, finalDate.value);
// });