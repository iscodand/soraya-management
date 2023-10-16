window.addEventListener("load", function () {
    const preloader = document.querySelector(".preloader");
    preloader.style.opacity = "0";
    setTimeout(function () {
        preloader.style.display = "none";

        // Adiciona uma classe para aplicar a transição de fade no conteúdo da página
        const content = document.querySelector(".fade-out-content");
        content.style.opacity = "1";
    }, 1000);
});