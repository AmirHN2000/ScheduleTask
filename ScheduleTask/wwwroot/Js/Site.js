$('#ExportExcel').on('click', function() {
    $.ajax({
        url:'/Task/CreateLinkForExcelFile',
        method:'Post',
        data:{"dateP":GetDateP, "fromDate":GetFromDate, "toDate":GetToDate, "status":GetStatus, "userId":GetSelectUser},
        success:function(data) {
            window.location.href=data.link;
        }
    });
});
function GetSelectUser() {
    return $('#SelectUser').val();
}
function GetDateP() {
    return $('#fromDate1').val();
}
function GetFromDate() {
    return $('#fromDate1').val();
}
function GetToDate() {
    return $('#toDate1').val();
}
function GetStatus(){
    return $('#status').val();
}

$('#btnClearAllFilter').on('click', function() {
    $('#fromDate1').val('');
    $('#toDate1').val('');
    $('#status').val(-1);
    $('#SelectUser').val("-1");
    $('#dateFilter').text('');
    $('#toDateFilter').text('');
    $('#statusFilter').text('');
    $('#userFilter').text('');
});

$('#btnClearDates').on('click', function() {
    $('#toDate1').val('');
    $('#toDateFilter').text('');
});

$('#btnClear').on('click', function() {
    $('#fromDate1').val('');
    $('#dateFilter').text('');
});

function changeDate() {
    let value=$('#fromDate1').val();

    if (value!==''){
        $('#dateFilter').text('تاریخ : '+value+'  ');
    }
    else {
        $('#dateFilter').text('  ');
    }
}
function changeToDate() {
    let value=$('#toDate1').val();

    if (value!==''){
        $('#toDateFilter').text('تا تاریخ : '+value+'  ');
    }
    else {
        $('#toDateFilter').text('  ');
    }
}
function changeStatus() {
    let value=$('#status').val();
    
    if (value==='0'){
        $('#statusFilter').text('وضعیت : درحال انجام ');
    }
    else if (value==='1'){
        $('#statusFilter').text('وضعیت : انجام شده ');
    }
    else{
        $('#statusFilter').text('  ');
    }
}

function changeUser() {
    let text=$("#SelectUser option:selected").text();
    let val=$("#SelectUser").val();
    if (val!=="-1"){
        $('#userFilter').text('کاربر : '+text+'  ');
    }
    else{
        $('#userFilter').text('  ');
    }
}

$('#mydiv').on('click', '.btnShow', function() {
    let id=$(this).data('id');

    $.ajax({
        url:'/Task/ShowTask',
        type:'Post',
        datatype:'json',
        data:{taskId : id},
        success:function(data) {
            $('.modal-body').empty();
            $('.modal-body').append(data);
            $('#mymodal').modal('show');
        },
        error:function() {
            alert('خطا در نمایش کارها');
        }
    });
});

$('#btnSearch').on('click', function() {
    table.ajax.reload();
});

$('#mydiv').on('click', '.btnDelete', function() {
    let id=$(this).data('id');

    $.ajax({
        url:'/Task/DeleteTask',
        type:'Post',
        datatype:'json',
        data:{taskId : id},
        success:function(data) {
            if (data.status){
                table.ajax.reload();
            }
            else {
                $.confirm({
                    title: 'خطا!!!',
                    content: data.message,
                    type: 'red',
                    typeAnimated: true,
                    buttons: {
                        close: {
                            text: 'Ok',
                            btnClass: 'btn-red',
                            action: function(){
                            }
                        },
                    }
                });
            }
        },
        error:function() {
            alert('خطا در حذف کار');
        }
    });
});