document.querySelector("#profile-img-file-input")&&document.querySelector("#profile-img-file-input").addEventListener("change",function(){var e=document.querySelector(".user-profile-image"),t=document.querySelector(".profile-img-file-input").files[0],n=new FileReader;n.addEventListener("load",function(){e.src=n.result},!1),t&&n.readAsDataURL(t)}),document.querySelector("#profile-img-file-input1")&&document.querySelector("#profile-img-file-input1").addEventListener("change",function(){var e=document.querySelector(".user-profile-image1"),t=document.querySelector(".profile-img-file-input1").files[0],n=new FileReader;n.addEventListener("load",function(){e.src=n.result},!1),t&&n.readAsDataURL(t)}),document.getElementById("schedule-date").flatpickr(),document.getElementById("due-date").flatpickr(),document.addEventListener("DOMContentLoaded",function(){var e=document.querySelectorAll(".plus"),t=document.querySelectorAll(".minus");function n(e,t){var n=1===t?e.previousElementSibling:e.nextElementSibling,i=parseInt(n.value),r=parseInt(n.min),e=parseInt(n.max),t=i+t;r<=t&&t<=e&&(n.value=t)}e.forEach(function(e){e.addEventListener("click",function(){n(e,1)})}),t.forEach(function(e){e.addEventListener("click",function(){n(e,-1)})})});