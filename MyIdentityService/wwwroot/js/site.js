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
});

; (function (window) {

	'use strict';

	// taken from mo.js demos
	function isIOSSafari() {
		var userAgent;
		userAgent = window.navigator.userAgent;
		return userAgent.match(/iPad/i) || userAgent.match(/iPhone/i);
	};

	// taken from mo.js demos
	function isTouch() {
		var isIETouch;
		isIETouch = navigator.maxTouchPoints > 0 || navigator.msMaxTouchPoints > 0;
		return [].indexOf.call(window, 'ontouchstart') >= 0 || isIETouch;
	};

	// taken from mo.js demos
	var isIOS = isIOSSafari(),
		clickHandler = isIOS || isTouch() ? 'touchstart' : 'click';

	function extend(a, b) {
		for (var key in b) {
			if (b.hasOwnProperty(key)) {
				a[key] = b[key];
			}
		}
		return a;
	}

function Animocon(el, options) {
	this.el = el;
	this.options = extend({}, this.options);
	extend(this.options, options);

	//this.checked = false;
	if (el.classList.contains('chk') == true) {
		this.checked = true;
		el.style.color = '#F35186';
	}
	else {
		this.checked = false;
		el.style.color = '#C0C1C3';
	};

	

	this.timeline = new mojs.Timeline();

	for (var i = 0, len = this.options.tweens.length; i < len; ++i) {
		this.timeline.add(this.options.tweens[i]);
	}

	var self = this;
	this.el.addEventListener(clickHandler, function () {
		if (self.checked) {
			self.options.onUnCheck();
		}
		else {
			self.options.onCheck();
			self.timeline.replay();
		}
		self.checked = !self.checked;
	});
}

Animocon.prototype.options = {
	tweens: [
		new mojs.Burst({})
	],
	onCheck: function () { return false; },
	onUnCheck: function () { return false; }
};

	function init() {

		var el = document.querySelector('.icobutton'),
		elSpan = el.querySelector('span'),
			elCounter = el.querySelector('span.icobutton__text');

		
	new Animocon(el, {
		tweens: [
			// ring animation
			new mojs.Shape({
				parent: el,
				duration: 750,
				type: 'circle',
				radius: { 0: 40 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 35: 0 },
				opacity: 0.2,
				top: '45%',
				easing: mojs.easing.bezier(0, 1, 0.5, 1)
			}),
			new mojs.Shape({
				parent: el,
				duration: 500,
				delay: 100,
				type: 'circle',
				radius: { 0: 20 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.2,
				x: 40,
				y: -60,
				easing: mojs.easing.sin.out
			}),
			new mojs.Shape({
				parent: el,
				duration: 500,
				delay: 180,
				type: 'circle',
				radius: { 0: 10 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.5,
				x: -10,
				y: -80,
				isRunLess: true,
				easing: mojs.easing.sin.out
			}),
			new mojs.Shape({
				parent: el,
				duration: 800,
				delay: 240,
				type: 'circle',
				radius: { 0: 20 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.3,
				x: -70,
				y: -10,
				easing: mojs.easing.sin.out
			}),
			new mojs.Shape({
				parent: el,
				duration: 800,
				delay: 240,
				type: 'circle',
				radius: { 0: 20 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.4,
				x: 80,
				y: -50,
				easing: mojs.easing.sin.out
			}),
			new mojs.Shape({
				parent: el,
				duration: 1000,
				delay: 300,
				type: 'circle',
				radius: { 0: 15 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.2,
				x: 20,
				y: -100,
				easing: mojs.easing.sin.out
			}),
			new mojs.Shape({
				parent: el,
				duration: 600,
				delay: 330,
				type: 'circle',
				radius: { 0: 25 },
				fill: 'transparent',
				stroke: '#F35186',
				strokeWidth: { 5: 0 },
				opacity: 0.4,
				x: -40,
				y: -90,
				easing: mojs.easing.sin.out
			}),
			// icon scale animation
			new mojs.Tween({
				duration: 1200,
				easing: mojs.easing.ease.out,
				onUpdate: function (progress) {
					if (progress > 0.3) {
						var elasticOutProgress = mojs.easing.elastic.out(1.43 * progress - 0.43);
						elSpan.style.WebkitTransform = elSpan.style.transform = 'scale3d(' + elasticOutProgress + ',' + elasticOutProgress + ',1)';
					}
					else {
						elSpan.style.WebkitTransform = elSpan.style.transform = 'scale3d(0,0,1)';
					}
				}
			})
		],
		onCheck: function () {
			let id = window.location.pathname.slice(11);
			jsAddLike(id);
			el.style.color = '#F35186';
			elCounter.innerHTML = Number(elCounter.innerHTML) + 1;
		},
		onUnCheck: function () {
			let id = window.location.pathname.slice(11);
			jsAddLike(id);
			el.style.color = '#C0C1C3';
			var current = Number(elCounter.innerHTML);
			elCounter.innerHTML = current > 1 ? Number(elCounter.innerHTML) - 1 : '';
		}

	});
	};
	init();
})(window);


	




