const daysOfWeek = [
    "domingo", "segunda-feira", "terça-feira", "quarta-feira", "quinta-feira", "sexta-feira", "sábado"
];

const months = [
    "janeiro", "fevereiro", "março", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"
];

function formatDate() {
    const today = new Date();
    const dayOfWeek = daysOfWeek[today.getDay()];
    const day = today.getDate();
    const month = months[today.getMonth()];

    return `${dayOfWeek}, ${day} de ${month}`;
}

const formatedDate = formatDate();

document.getElementById('current-date').textContent = formatedDate