import { fetchData } from './api.js';
import { createOrUpdateChart } from './charts.js';

// Elements for Charts
let mealsChartElement = document.getElementById('mealsChart').getContext('2d');
let customersChartElement = document.getElementById('customersChart').getContext('2d');
let ordersChartElement = document.getElementById('ordersChart').getContext('2d');

// Charts objects (try to read 'createOrUpdateChat' function from './chart.js' file)
let mealsChart;
let customersChart;
let ordersChart;

// Arrays for keep up data from api request (try to read 'fetchData' request from './api.js' file)
let meals = [];
let mealsDescription = [];
let mealsOrdersCount = [];

let customers = [];
let customersName = [];
let customersOrdersCount = [];

let orders = [];
let ordersDays = ['domingo', 'segunda', 'terça', 'quarta', 'quinta', 'sexta', 'sábado'];
let ordersPerDayCount = [];

// When DOM loads, get data and create chart
window.addEventListener('DOMContentLoaded', () => {
    createChart(dateRangeSelected.value);
});

// Filter by date in select
let dateRangeSelected = document.getElementById("dateRangeSelected");
$(dateRangeSelected).on("change", function () {
    createChart(dateRangeSelected.value);
});

function createChart(selectedValue) {
    fetchData(selectedValue)
        .then((data) => {
            meals = data['meals'];
            customers = data['customers'];
            orders = data['orders'];

            // Cleaning the arrays for keep up new data (if updating the chart)
            mealsOrdersCount = [];
            mealsDescription = [];
            customersName = [];
            customersOrdersCount = [];
            ordersPerDayCount = [];

            for (const meal of meals) {
                mealsDescription.push(meal.description)
                mealsOrdersCount.push(meal.ordersCount);
            }

            for (const customer of customers) {
                customersName.push(customer.name);
                customersOrdersCount.push(customer.ordersCount);
            }

            for (const order of orders) {
                ordersPerDayCount.push(order);
            }

            // Create or Update all charts (update when it exists)
            mealsChart = createOrUpdateChart(mealsChartElement, mealsChart, 'bar', 'Pedidos',
                mealsDescription, mealsOrdersCount, 'Sabores mais Pedidos');

            customersChart = createOrUpdateChart(customersChartElement, customersChart, 'bar', 'Pedidos',
                customersName, customersOrdersCount, 'Clientes que Mais Pediram');

            ordersChart = createOrUpdateChart(ordersChartElement, ordersChart, 'bar', 'Pedidos',
                ordersDays, ordersPerDayCount, 'Quantidade de Pedidos');
        })
        .catch((error) => {
            alert(error);
        });
}