class TouchSpin{constructor(){this.body=document.getElementsByTagName("body")[0],this.window=window}initTouchspin(){document.querySelectorAll('[data-toggle="touchspin"]').forEach(function(a){a.getAttribute("value");var e,n,t=a.getAttribute("data-bts-prefix"),i=a.getAttribute("data-bts-postfix"),u=a.getAttribute("data-bts-prefix-extra-class"),o=a.getAttribute("data-bts-prefix-extra-class"),s=a.getAttribute("data-bts-button-down-class"),r=a.getAttribute("data-bts-button-up-class");const l=a.getAttribute("data-step")??1,d=a.getAttribute("data-bts-max"),c=a.getAttribute("data-decimals")??0,p=document.createElement("div");a.parentNode.appendChild(p),a.classList.add("form-control"),p.setAttribute("class","input-group bootstrap-touchspin bootstrap-touchspin-injected");const b=document.createElement("button");b.setAttribute("class","bootstrap-touchspin-down input-group-text ".concat(s||"btn btn-primary")),b.type="button",b.innerHTML="-";let v;t&&(v=document.createElement("span"),v.setAttribute("class","input-group-text ".concat(u||"")),v.innerHTML=t);let A;i&&(A=document.createElement("span"),A.setAttribute("class","input-group-text ".concat(o?u:"")),A.innerHTML=i);const m=document.createElement("button");m.setAttribute("class","bootstrap-touchspin-up ".concat(r||"btn btn-primary")),m.type="button",m.innerHTML="+",p.appendChild(b),v&&p.appendChild(v),p.appendChild(a),A&&p.appendChild(A),p.appendChild(m),b.addEventListener("click",function(t){let e;e=isNaN(parseFloat(a.getAttribute("value")))?0:parseFloat(a.getAttribute("value")),0<=e-l?(a.setAttribute("value",(e-l).toFixed(c)),a.value=(e-l).toFixed(c)):(a.setAttribute("value",0),a.value=0)}),b.addEventListener("mousedown",function(t){e=setInterval(function(){let t;t=isNaN(parseFloat(a.getAttribute("value")))?0:parseFloat(a.getAttribute("value")),0<=t-l?(a.setAttribute("value",(t-l).toFixed(c)),a.value=(t-l).toFixed(c)):(a.setAttribute("value",0),a.value=0)},150)}),b.addEventListener("mouseup",function(t){clearInterval(e)}),b.addEventListener("mouseleave",function(t){clearInterval(e)}),b.addEventListener("mouseout",function(t){clearInterval(e)}),m.addEventListener("click",function(t){let e;e=isNaN(parseFloat(a.getAttribute("value")))?0:parseFloat(a.getAttribute("value")),(null==d||parseFloat(e)+parseFloat(l)<=d)&&(a.setAttribute("value",(parseFloat(e)+parseFloat(l)).toFixed(c)),a.value=(parseFloat(e)+parseFloat(l)).toFixed(c))}),m.addEventListener("mousedown",function(t){n=setInterval(function(){let t;t=isNaN(parseFloat(a.getAttribute("value")))?0:parseFloat(a.getAttribute("value")),(null==d||parseFloat(t)+parseFloat(l)<=d)&&(a.setAttribute("value",(parseFloat(t)+parseFloat(l)).toFixed(c)),a.value=(parseFloat(t)+parseFloat(l)).toFixed(c))},150)}),m.addEventListener("mouseup",function(t){clearInterval(n)}),m.addEventListener("mouseleave",function(t){clearInterval(n)}),m.addEventListener("mouseout",function(t){clearInterval(n)}),a.addEventListener("input",function(t){a.setAttribute("value",a.value)})})}init=()=>{this.initTouchspin()}}document.addEventListener("DOMContentLoaded",function(t){(new TouchSpin).init()});