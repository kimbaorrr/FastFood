document.addEventListener("DOMContentLoaded",function(){var e=document.querySelectorAll(".plus"),o=document.querySelectorAll(".minus");function n(e,o){var n=1===o?e.previousElementSibling:e.nextElementSibling,r=parseInt(n.value),t=parseInt(n.min),e=parseInt(n.max),o=r+o;t<=o&&o<=e&&(n.value=o)}e.forEach(function(e){e.addEventListener("click",function(){n(e,1)})}),o.forEach(function(e){e.addEventListener("click",function(){n(e,-1)})})});var previewTemplate,dropzone,dropzonePreviewNode=document.querySelector("#dropzone-preview-list");dropzonePreviewNode.id="",dropzonePreviewNode&&(previewTemplate=dropzonePreviewNode.parentNode.innerHTML,dropzonePreviewNode.parentNode.removeChild(dropzonePreviewNode),dropzone=new Dropzone(".dropzone",{url:"https://httpbin.org/post",method:"post",previewTemplate:previewTemplate,previewsContainer:"#dropzone-preview"})),document.getElementById("ex-date").flatpickr();