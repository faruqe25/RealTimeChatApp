
$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/signalRServerHub").build();
    connection.start();
    connection.on("refreshNewProduct", function () {
        loadNewProductCount();
        loadData();
    });
    //loadData();
    loadNewProductCount();
    function loadNewProductCount() {
        
        $.ajax({
            url: '/Home/ProductListCount',
            method: 'GET',
            success: (result) => {
                $("#NewItem").html(result);
            },
            error: (error) => {
                console.log(error);
            }
        });
    }
    function loadData() {
        var tr = '';

        $.ajax({
            url: '/Home/GetProductList',
            method: 'GET',
            success: (result) => {
               
                $.each(result, (k, v) => {

                    var isAvailable = v.avaiableStatus ? 'Available' : 'Out of Stock';
                    var className = v.avaiableStatus ? 'badge badge-pill badge-success' : 'badge badge-pill badge-warning';
                    tr = tr + `<tr>
                        <td>${v.productId}</td>
                        <td>${v.productName}</td>
                        <td>${v.price}</td>
                        <td><span class="${className}">${isAvailable}</span></td>`
                       tr = tr.concat('<td>' + '<a href="Home/Edit/'+ v.productId+'">Edit</a> | ');
                       tr = tr.concat('<a href="Home/Delete/'+ v.productId+'">Delete</a></td></tr>');
                });
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error);
            }
        });
    }
});
