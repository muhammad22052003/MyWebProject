function toggleAll() {

    const checkboxes = document.querySelectorAll('#flexCheckDefault');
    const selectAllCheckbox = document.getElementById('selectAll');

    console.log(checkboxes.length)

    for (let i = 0; i < checkboxes.length; i++) {
        console.log(checkboxes[i].className)
        checkboxes[i].checked = selectAllCheckbox.checked;
    }
}