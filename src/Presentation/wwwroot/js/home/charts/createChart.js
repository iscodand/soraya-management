import { fetchData } from './api/api.js';
import { createOrUpdateBarChart } from './chartTypes/barChart.js';
import { createOrUpdateDoughnutChart } from './chartTypes/doughnutChart.js';
import { createOrUpdateLineChart } from './chartTypes/lineChart.js';
import { formatDate } from '../../utils/formatDate.js'

// Elements for Charts
let mealsChartElement = document.getElementById('mealsChart').getContext('2d');
let customersChartElement = document.getElementById('customersChart').getContext('2d');
let ordersChartElement = document.getElementById('ordersChart').getContext('2d');
let ordersProfitChartElement = document.getElementById('ordersProfitChart').getContext('2d');

// Elements for Boxes
let newOrdersCount = document.getElementById("home-new-orders-count");
let totalOrdersSum = document.getElementById("home-total-orders-sum");
let paidOrdersSum = document.getElementById("home-paid-orders-sum");
let unpaidOrdersSum = document.getElementById("home-unpaid-orders-sum");
let paidOrdersCount = document.getElementById("home-paid-orders-count");
let unpaidOrdersCount = document.getElementById("home-unpaid-orders-count");

// Charts
let mealsChart;
let customersChart;
let ordersChart;
let ordersProfitChart;

// Arrays for keep up data from api request (try to read 'fetchData' request from './api.js' file)
let meals = [];
let mealsDescription = [];
let mealsOrdersCount = [];

let customers = [];
let customersName = [];
let customersOrdersCount = [];

let orders = [];

function calculateSum(array) {
    return array.reduce((accumulator, value) => {
        return accumulator + value;
    }, 0);
}

function fillOrdersTable(data) {
    const tableBody = document.getElementById("orders-tableBody");

    tableBody.innerHTML = "";

    const lastOrders = data.slice(-3);
    lastOrders.reverse();

    lastOrders.forEach(order => {
        const row = tableBody.insertRow();
        row.classList.add("table-row");

        const createdAtCell = row.insertCell();
        const customerCell = row.insertCell();
        const mealCell = row.insertCell();
        const priceCell = row.insertCell();
        const paymentTypeCell = row.insertCell();
        const isPaidCell = row.insertCell();

        createdAtCell.textContent = formatDate(order.createdAt);
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

// Get date to orders by day chart
function getDateWithoutTime(createdAt) {
    const date = new Date(createdAt);
    date.setHours(0, 0, 0, 0);
    return date.toISOString().split('T')[0];  // returns (aaaa-mm-dd) format
}

function getOrdersDataByDay(orders, calculateProfit) {
    orders.slice(-7);

    let earliestDate = null;
    let recentDate = null;

    orders.forEach(order => {
        const createdAtDate = getDateWithoutTime(order.createdAt);
        if (!earliestDate || createdAtDate < earliestDate) {
            earliestDate = createdAtDate;
        }
        if (!recentDate || createdAtDate > recentDate) {
            recentDate = createdAtDate;
        }
    })

    const ordersByDay = {};

    const currentDate = new Date(earliestDate);

    while (currentDate <= new Date(recentDate)) {
        const formatedDate = currentDate.toISOString().split('T')[0];
        ordersByDay[formatedDate] = 0;
        currentDate.setDate(currentDate.getDate() + 1);
    }

    orders.forEach(order => {
        const createdAtDate = getDateWithoutTime(order.createdAt);
        if (calculateProfit) {
            ordersByDay[createdAtDate] += order.price;
        } else {
            ordersByDay[createdAtDate]++;
        }
    });

    return ordersByDay;
}

export function createChart(selectedValue, initialDate, finalDate) {
    fetchData(selectedValue, initialDate, finalDate)
        .then((data) => {
            meals = data['meals'];
            customers = data['customers'];
            orders = data['orders'];

            // Fill recent orders table
            const ordersTable = document.getElementById("orders-table");
            const noOrdersFound = document.getElementById("no-orders-found");

            if (orders.length > 0) {
                fillOrdersTable(orders);
                noOrdersFound.classList.add("hidden");
                ordersTable.classList.remove("hidden");
            } else {
                ordersTable.classList.add("hidden");
                noOrdersFound.classList.remove("hidden");
            }

            // Get quantity of orders per day
            const ordersByDay = getOrdersDataByDay(orders, false);

            // Get value sum of orders per day
            const ordersProfitByDay = getOrdersDataByDay(orders, true);

            // Cleaning the arrays for keep up new data (when the charts was updated)
            mealsOrdersCount = [];
            mealsDescription = [];
            customersName = [];
            customersOrdersCount = [];

            for (const meal of meals) {
                if (meal.ordersCount > 0) {
                    // Get the name and quantity of meals ordered
                    mealsDescription.push(meal.description);
                    mealsOrdersCount.push(meal.ordersCount);
                }
            }

            for (const customer of customers) {
                if (customer.ordersCount > 0) {
                    // Get the name and quantity of customers whos order
                    customersName.push(customer.name);
                    customersOrdersCount.push(customer.ordersCount);
                }
            }

            let paidOrdersSumValue = 0;
            let unpaidOrdersSumValue = 0;
            let paidOrdersCountValue = 0;
            let unpaidOrdersCountValue = 0;

            for (const order of orders) {
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
            totalOrdersSum.textContent = `R$ ${(paidOrdersSumValue + unpaidOrdersSumValue).toFixed(2)}`;
            paidOrdersSum.textContent = `R$ ${paidOrdersSumValue.toFixed(2)}`;
            unpaidOrdersSum.textContent = `R$ ${unpaidOrdersSumValue.toFixed(2)}`;
            paidOrdersCount.textContent = paidOrdersCountValue;
            unpaidOrdersCount.textContent = unpaidOrdersCountValue;

            // Create or Update all charts (update when it exists)
            mealsChart = createOrUpdateDoughnutChart(
                mealsChartElement,
                mealsChart,
                'Pedidos',
                mealsDescription,
                mealsOrdersCount,
                'Sabores mais Pedidos'
            );

            customersChart = createOrUpdateDoughnutChart(
                customersChartElement,
                customersChart,
                'Pedidos',
                customersName,
                customersOrdersCount,
                'Clientes que Mais Pediram'
            );

            ordersChart = createOrUpdateBarChart(
                ordersChartElement,
                ordersChart,
                'Pedidos',
                [],
                ordersByDay,
                'Quantidade de Pedidos'
            );

            ordersProfitChart = createOrUpdateLineChart(
                ordersProfitChartElement,
                ordersProfitChart,
                'Valor em R$',
                [],
                ordersProfitByDay,
                'Lucro Total'
            );
        })
        .catch((error) => {
            alert(error);
        });
}