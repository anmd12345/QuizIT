// Bắt sự kiện nhấn phím trên trang
document.addEventListener('keydown', function (event) {
    // Kiểm tra nếu phím F12 được nhấn
    if (event.keyCode === 123) { // 123 là mã phím F12
        alert('Chức năng này đã bị vô hiệu hóa!');
        event.preventDefault(); // Ngăn chặn hành động mặc định của phím (mở Developer Tools)
    }
});

// Bắt sự kiện chuột phải trên trang
document.addEventListener('contextmenu', function (event) {
    // Hiển thị thông báo khi chuột phải được nhấn
    alert('Chức năng này đã bị vô hiệu hóa!');
    event.preventDefault(); // Ngăn chặn hành động mặc định của chuột phải (hiển thị context menu)
});
