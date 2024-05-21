$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/category/getall' },
        "columns": [
            { data: 'categoryName', "width": "25%" },
            { data: 'imageUrl', "width": "15%" },
           
            //{
            //    data: 'id',
            //    "render": function (data) {
            //        return `<div class="w-75 btn-group" role="group">
            //         <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"> Edit</a>               
            //         <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2"> Delete</a>
            //        </div>`
            //    },
            //    "width": "25%"
            //}
        ]
    });
}