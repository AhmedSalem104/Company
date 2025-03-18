let searchTimeout;
document.getElementById("searchInput").addEventListener("keyup", function () {
    clearTimeout(searchTimeout); 
    searchTimeout = setTimeout(() => {
        let query = this.value.trim(); 
        if (query.length > 0) {
            document.getElementById("searchForm").submit();
        }
    }, 500); 
});