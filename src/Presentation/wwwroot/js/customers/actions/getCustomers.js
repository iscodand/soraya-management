document.addEventListener('DOMContentLoaded', function () {
    const userSearch = document.getElementById('userSearch');
    const userDropdown = document.getElementById('userDropdown');

    // Faz a chamada para o endpoint remoto e carrega todos os clientes
    fetch('http://localhost:5184/clientes/listar-clientes')
        .then(response => response.json())
        .then(result => {
            // Adiciona todos os clientes ao dropdown inicialmente

            console.log(result.data)

            result.data.forEach(user => {
                const link = document.createElement('a');
                link.classList.add('dropdown-item');
                link.href = '#';
                link.textContent = user.name;
                userDropdown.appendChild(link);
            });

            // Exibe todos os clientes
            userDropdown.style.display = 'block';
        })
        .catch(error => console.error('Erro ao buscar clientes:', error));

    userSearch.addEventListener('input', function () {
        const inputText = this.value.toLowerCase();

        // Filtra os clientes que correspondem ao texto de entrada
        const filteredUsers = Array.from(userDropdown.children)
            .filter(link => link.textContent.toLowerCase().includes(inputText));

        // Atualiza o dropdown com os clientes filtrados
        userDropdown.innerHTML = '';
        filteredUsers.forEach(link => userDropdown.appendChild(link));

        // Exibe ou oculta o dropdown conforme necessário
        userDropdown.style.display = filteredUsers.length > 0 ? 'block' : 'none';
    });

    // Fecha o dropdown se clicar fora dele
    document.addEventListener('click', function (e) {
        if (!userSearch.contains(e.target)) {
            userDropdown.style.display = 'none';
        }
    });
});