function updateTableCaption(type, quantity) {
    const caption = $(".styled-table-caption");
    caption.text(`${type} Encontrados: ${quantity}`);
}