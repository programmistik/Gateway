// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function jsAddLike(id) {

    $.ajax({
        url: '/Post/jsAddLike',
        type: 'POST',
        data: { id: id },
        success: function (data) {
            let btn = $('#like');
            if (btn.hasClass('btn-danger')) {
                btn.removeClass('btn-danger').addClass('btn-success');
            }
            else {
                btn.removeClass('btn-success').addClass('btn-danger');
            }
            //$('#like')
            //    .removeClass('btn-danger')
            //    .removeClass('btn-success')
            //    .addClass('btn-info');
        }
    });
}

function jsDelPost(id) {
    if (confirm("Are you sure you want to delete this post?")) {
        

        $.ajax({
            url: '/Post/jsDelPost',
            type: 'DELETE',
            data: { id: id },
            success: function (data) {
                //location.reload(true);
                $.ajax({
                    url: '/Profile/UserProfile',
                    data: { id: 2 },
                    success: function (data) {
                       
                    }
                });
            }
        });
    }
}

function jsAddFriend(id) {

    $.ajax({
        url: '/Profile/jsAddFriend',
        type: 'POST',
        data: { id: id },
        success: function (data) {
            let btn = $('#friend');
            if (btn.hasClass('btn-danger')) {
                btn.removeClass('btn-danger').addClass('btn-success');
                btn.html("Add to friends");
            }
            else {
                btn.removeClass('btn-success').addClass('btn-danger');
                btn.html("Delete from friends");
            }
           
        }
    });
}



