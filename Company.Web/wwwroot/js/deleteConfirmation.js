function confirmDelete(id, controller) {

    Swal.fire({
        title: "تأكيد الحذف",
        text: "هل أنت متأكد أنك تريد حذف هذا العنصر؟ لا يمكن التراجع عن هذا الإجراء!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#D9534F",
        cancelButtonColor: "#5BC0DE",
        confirmButtonText: "حذف",
        cancelButtonText: "إلغاء",
        reverseButtons: true,
        background: "#f8f9fa"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: "جاري الحذف...",
                text: "يرجى الانتظار...",
                icon: "info",
                showConfirmButton: false,
                timer: 1200,
                background: "#f8f9fa"
            });

            $.ajax({
                url: `/${controller}/DeleteConfirmed`,
                type: "POST",
                contentType: "application/x-www-form-urlencoded",
                data: { id: id },
                success: function () {
                    Swal.fire({
                        title: "تم الحذف بنجاح",
                        text: "تم حذف العنصر بنجاح.",
                        icon: "success",
                        confirmButtonColor: "#28a745",
                        confirmButtonText: "موافق",
                        background: "#f8f9fa"
                    }).then(() => location.reload());
                },
                error: function () {
                    Swal.fire({
                        title: "حدث خطأ",
                        text: "لم يتم الحذف، حاول مرة أخرى.",
                        icon: "error",
                        confirmButtonColor: "#D9534F",
                        confirmButtonText: "إعادة المحاولة",
                        background: "#f8f9fa"
                    });
                }
            });
        }
    });
}