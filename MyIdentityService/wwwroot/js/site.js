// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('#back-to-top').fadeIn();
        } else {
            $('#back-to-top').fadeOut();
        }
    });
    // scroll body to 0px on click
    $('#back-to-top').click(function () {
        $('#back-to-top').tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    //$('#back-to-top').tooltip('show');

});

function jsAddLike(id) {

    $.ajax({
        url: '/Post/jsAddLike',
        type: 'POST',
        data: { id: id },
        success: function (data) {
            //let btn = $('#like');
            //if (btn.hasClass('btn-danger')) {
            //    btn.removeClass('btn-danger').addClass('btn-success');
            //}
            //else {
            //    btn.removeClass('btn-success').addClass('btn-danger');
            //}
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
                document.getElementById(id).remove();
                //$.ajax({
                //    url: '/Profile/UserProfile',
                //    data: { id: 2 },
                //    success: function (data) {
                       
                //    }
                //});
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


function readURL(input) {
    if (input.files && input.files[0]) {

        var image = input.files[0]; // FileList object
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            var reader = new FileReader();
            // Closure to capture the file information.
            reader.addEventListener("load", function (e) {
                const imageData = e.target.result;
                window.loadImage(imageData, function (img) {
                    if (img.type === "error") {
                        console.log("couldn't load image:", img);
                    } else {
                        window.EXIF.getData(img, function () {
                            console.log("done!");
                            var orientation = window.EXIF.getTag(this, "Orientation");
                            var canvas = window.loadImage.scale(img, { orientation: orientation || 0, canvas: true });
                            document.getElementById("container2").appendChild(canvas);
                            canvas.style.marginLeft = "10px";

                            if (orientation == 1)
                                canvas.style.width = "500px";
                            else
                                canvas.style.height = "500px";
                        });
                    }
                });
            });
            reader.readAsDataURL(image);
        } else {
            console.log('The File APIs are not fully supported in this browser.');
        }

    }
}
$("#inputGroupFile01").change(function () {
    $('canvas:nth-of-type(1)').remove();
    readURL(this);
    //getDataUrl(this);
});

// we need to save the total rotation angle as a global variable 
var current_rotation = 0;

// change CSS transform property on click
document.querySelector("#rotate-button").addEventListener('click', function () {
    // update total rotation
    // if angle is positive, rotation happens clockwise. if negative, rotation happens anti-clockwise.
    current_rotation += 90;

    // rotate clockwise by 90 degrees
    document.querySelector("#container2").style.transform = 'rotate(' + current_rotation + 'deg)';
});



function jsShowNews(model) {
	console.log(model);
}


//----------------------

let tree = {
    label: 'root',
    nodes: [
        {
            label: 'item1',
            nodes: [
                {
                    label: 'item1.1'
                },
                {
                    label: 'item1.2',
                    nodes: [
                        {
                            label: 'item1.2.1'
                        }
                    ]
                }
            ]
        },
        {
            label: 'item2'
        }
    ]
}

Vue.component('tree-menu', {
    props: ['label', 'nodes', 'depth'],
    name: 'tree-menu',
    computed: {
        indent() {
            return { transform: `translate(${this.depth * 50}px)` }
        }
    },
    template: `
<div class="tree-menu">
    <div :style="indent">{{ label }}</div>
    <tree-menu 
      v-for="node in nodes" 
      :nodes="node.nodes" 
      :label="node.label"
      :depth="depth + 1"
    >
    </tree-menu>
  </div>
`
});




new Vue({
    el: "#app",
    data: {
        tree
    }

});

