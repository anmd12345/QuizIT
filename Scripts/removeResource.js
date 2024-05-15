/*document.addEventListener('keydown', function (e) {
    if (e.keyCode == 123 || // F12
        (e.ctrlKey && e.shiftKey && e.keyCode == 73) || // Ctrl+Shift+I
        (e.ctrlKey && e.keyCode == 85) // Ctrl+U
    ) {
        e.preventDefault();
        alert("Chức năng này bị vô hiệu hóa");
        return false;
    }
});*/

(function () {
    var devtools = /./;
    devtools.toString = function () {
        this.opened = true;
    };
    console.log('%c', devtools);
    if (devtools.opened) {
        alert('Developer Tools đang mở');
    }
})();

/*document.addEventListener('contextmenu', function (event) {
    alert('Chức năng này đã bị vô hiệu hóa!');
    event.preventDefault();
});*/
