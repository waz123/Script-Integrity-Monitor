window.openModal = function (selector) {
    const modalElement = document.querySelector(selector);
    const modal = new bootstrap.Modal(modalElement);
    modal.show();
}

window.toggleCollapse = function (selector) {
    var element = document.querySelector(selector);
    if (element) {
        var bsCollapse = new bootstrap.Collapse(element, { toggle: true });
        bsCollapse.toggle();
    }
};