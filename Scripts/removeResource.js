document.addEventListener("keydown", function (event) {
    if (event.key === "F12" || (event.ctrlKey && event.key === "u")) {
        console.log("Người dùng đang cố gắng xem mã nguồn!");
    }
});