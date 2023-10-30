import { fetchData } from './api.js';
import { createOrUpdateBarChart } from './barChart.js';
import { createOrUpdateDoughnutChart } from './doughnutChart.js';

// Elements for Charts
let mealsChartElement = document.getElementById('mealsChart').getContext('2d');
let customersChartElement = document.getElementById('customersChart').getContext('2d');
let ordersChartElement = document.getElementById('ordersChart').getContext('2d');

// Elements for Boxes
let newOrdersCount = document.getElementById("home-new-orders-count");
let paidOrdersSum = document.getElementById("home-paid-orders-sum");
let unpaidOrdersSum = document.getElementById("home-unpaid-orders-sum");
let paidOrdersCount = document.getElementById("home-paid-orders-count");
let unpaidOrdersCount = document.getElementById("home-unpaid-orders-count");

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

$(initialDate).on("change", function () {
    createChart(dateRangeSelected.value, initialDate.value, finalDate.value);
});

$(finalDate).on("change", function () {
    createChart(dateRangeSelected.value, initialDate.value, finalDate.value);
});

function createChart(selectedValue, initialDate, finalDate) {
    fetchData(selectedValue, initialDate, finalDate)
        .then((data) => {
            meals = data['meals'];
            customers = data['customers'];
            orders = data['orders'];

            fillOrdersTable(orders);

            // Cleaning the arrays for keep up new data (if updating the chart)
            mealsOrdersCount = [];
            mealsDescription = [];

            customersName = [];
            customersOrdersCount = [];

            ordersPerDayCount = [];

            for (const meal of meals) {
                if (meal.ordersCount > 0) {
                    mealsDescription.push(meal.description)
                    mealsOrdersCount.push(meal.ordersCount);
                }
            }

            for (const customer of customers) {
                if (customer.ordersCount > 0) {
                    customersName.push(customer.name);
                    customersOrdersCount.push(customer.ordersCount);
                }
            }

            let paidOrdersSumValue = 0;
            let unpaidOrdersSumValue = 0;
            let paidOrdersCountValue = 0
            let unpaidOrdersCountValue = 0

            for (const order of orders) {
                ordersPerDayCount.push(order);

                if (order.isPaid == true) {
                    paidOrdersSumValue += order.price;
                    paidOrdersCountValue++;
                } else {
                    unpaidOrdersSumValue += order.price;
                    unpaidOrdersCountValue++;
                }
            }

            // Define value of boxes data
            newOrdersCount.textContent = calculateSum(mealsOrdersCount);
            paidOrdersSum.textContent = "R$ " + paidOrdersSumValue.toPrecision();
            unpaidOrdersSum.textContent = "R$ " + unpaidOrdersSumValue.toPrecision();
            paidOrdersCount.textContent = paidOrdersCountValue;
            unpaidOrdersCount.textContent = unpaidOrdersCountValue;

            // Create or Update all charts (update when it exists)
            mealsChart = createOrUpdateDoughnutChart(mealsChartElement, mealsChart, 'Pedidos',
                mealsDescription, mealsOrdersCount, 'Sabores mais Pedidos');

            customersChart = createOrUpdateDoughnutChart(customersChartElement, customersChart, 'Pedidos',
                customersName, customersOrdersCount, 'Clientes que Mais Pediram');

            ordersChart = createOrUpdateBarChart(ordersChartElement, ordersChart, 'Pedidos',
                ordersDays, ordersPerDayCount, 'Quantidade de Pedidos');
        })
        .catch((error) => {
            alert(error);
        });
}


function calculateSum(array) {
    return array.reduce((accumulator, value) => {
        return accumulator + value;
    }, 0);
}

function fillOrdersTable(data) {
    const tableBody = document.getElementById("orders-tableBody");

    tableBody.innerHTML = "";

    const lastOrders = data.slice(-3)

    lastOrders.forEach(order => {
        const row = tableBody.insertRow();
        row.classList.add("table-row");

        const createdAtCell = row.insertCell();
        const customerCell = row.insertCell();
        const mealCell = row.insertCell();
        const priceCell = row.insertCell();
        const paymentTypeCell = row.insertCell();
        const isPaidCell = row.insertCell();

        const date = new Date(order.createdAt);
        const day = date.getDate();
        const month = date.getMonth() + 1;
        const year = date.getFullYear();
        const formatedDate = `${day}/${month}/${year}`;

        createdAtCell.textContent = formatedDate;
        customerCell.textContent = order.customer;
        mealCell.textContent = order.meal;
        priceCell.innerHTML = `<b>R$ ${order.price.toFixed(2)}</b>`;
        paymentTypeCell.innerHTML = '<i class="fa-brands fa-pix fa-xl"></i>';

        if (order.isPaid === true) {
            isPaidCell.innerHTML = `<p class="status paid">Pago</p>`;
        } else {
            isPaidCell.innerHTML = `<p class="status pending">Pendente</p>`;
        }
    });
}