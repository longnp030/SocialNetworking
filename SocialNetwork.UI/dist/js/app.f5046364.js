(function(e){function t(t){for(var n,u,i=t[0],s=t[1],c=t[2],f=0,p=[];f<i.length;f++)u=i[f],Object.prototype.hasOwnProperty.call(o,u)&&o[u]&&p.push(o[u][0]),o[u]=0;for(n in s)Object.prototype.hasOwnProperty.call(s,n)&&(e[n]=s[n]);l&&l(t);while(p.length)p.shift()();return a.push.apply(a,c||[]),r()}function r(){for(var e,t=0;t<a.length;t++){for(var r=a[t],n=!0,i=1;i<r.length;i++){var s=r[i];0!==o[s]&&(n=!1)}n&&(a.splice(t--,1),e=u(u.s=r[0]))}return e}var n={},o={app:0},a=[];function u(t){if(n[t])return n[t].exports;var r=n[t]={i:t,l:!1,exports:{}};return e[t].call(r.exports,r,r.exports,u),r.l=!0,r.exports}u.m=e,u.c=n,u.d=function(e,t,r){u.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:r})},u.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},u.t=function(e,t){if(1&t&&(e=u(e)),8&t)return e;if(4&t&&"object"===typeof e&&e&&e.__esModule)return e;var r=Object.create(null);if(u.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var n in e)u.d(r,n,function(t){return e[t]}.bind(null,n));return r},u.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return u.d(t,"a",t),t},u.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},u.p="/";var i=window["webpackJsonp"]=window["webpackJsonp"]||[],s=i.push.bind(i);i.push=t,i=i.slice();for(var c=0;c<i.length;c++)t(i[c]);var l=s;a.push([0,"chunk-vendors"]),r()})({0:function(e,t,r){e.exports=r("56d7")},"034f":function(e,t,r){"use strict";r("85ec")},"41c4":function(e,t,r){"use strict";var n=r("8855"),o=r.n(n);t["default"]=o.a},"56b4":function(e,t,r){"use strict";var n=r("882b"),o=r("41c4"),a=r("2877"),u=Object(a["a"])(o["default"],n["a"],n["b"],!1,null,"31a6ee2e",null);t["default"]=u.exports},"56d7":function(e,t,r){"use strict";r.r(t);r("e260"),r("e6cf"),r("cca6"),r("a79d");var n=r("2b0e"),o=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"app"}},[r("router-view")],1)},a=[],u={name:"app",components:{},created:function(){this.$router.push("home",(function(){}))}},i=u,s=(r("034f"),r("2877")),c=Object(s["a"])(i,o,a,!1,null,null,null),l=c.exports,f=r("8c4f"),p=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",[e.show?r("b-form",{on:{submit:e.onSubmit,reset:e.onReset}},[r("b-form-group",{attrs:{id:"input-group-1",label:"Email address:","label-for":"input-1",description:"We'll never share your email with anyone else."}},[r("b-form-input",{attrs:{id:"input-1",type:"email",placeholder:"Enter email",required:""},model:{value:e.form.email,callback:function(t){e.$set(e.form,"email",t)},expression:"form.email"}})],1),r("b-form-group",{attrs:{id:"input-group-2",label:"Your Name:","label-for":"input-2"}},[r("b-form-input",{attrs:{id:"input-2",placeholder:"Enter name",required:""},model:{value:e.form.name,callback:function(t){e.$set(e.form,"name",t)},expression:"form.name"}})],1),r("b-form-group",{attrs:{id:"input-group-3",label:"Food:","label-for":"input-3"}},[r("b-form-select",{attrs:{id:"input-3",options:e.foods,required:""},model:{value:e.form.food,callback:function(t){e.$set(e.form,"food",t)},expression:"form.food"}})],1),r("b-form-group",{attrs:{id:"input-group-4"},scopedSlots:e._u([{key:"default",fn:function(t){var n=t.ariaDescribedby;return[r("b-form-checkbox-group",{attrs:{id:"checkboxes-4","aria-describedby":n},model:{value:e.form.checked,callback:function(t){e.$set(e.form,"checked",t)},expression:"form.checked"}},[r("b-form-checkbox",{attrs:{value:"me"}},[e._v("Check me out")]),r("b-form-checkbox",{attrs:{value:"that"}},[e._v("Check that out")])],1)]}}],null,!1,531429359)}),r("b-button",{attrs:{type:"submit",variant:"primary"}},[e._v("Submit")]),r("b-button",{attrs:{type:"reset",variant:"danger"}},[e._v("Reset")])],1):e._e(),r("b-card",{staticClass:"mt-3",attrs:{header:"Form Data Result"}},[r("pre",{staticClass:"m-0"},[e._v(e._s(e.form))])])],1)},d=[],m=(r("e9c4"),r("b0c0"),{data:function(){return{form:{email:"",name:"",food:null,checked:[]},foods:[{text:"Select One",value:null},"Carrots","Beans","Tomatoes","Corn"],show:!0}},methods:{onSubmit:function(e){e.preventDefault(),alert(JSON.stringify(this.form))},onReset:function(e){var t=this;e.preventDefault(),this.form.email="",this.form.name="",this.form.food=null,this.form.checked=[],this.show=!1,this.$nextTick((function(){t.show=!0}))}}}),h=m,b=Object(s["a"])(h,p,d,!1,null,"67a54ed5",null),v=b.exports,g=r("56b4"),k=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div")},w=[],y=r("1da1"),x=(r("96cf"),r("bc3a")),j=r.n(x),_={name:"Home",components:{},data:function(){return{getUserUrl:"https://localhost:5001/Users/",jwtToken:"",userId:""}},props:{},created:function(){this.jwtToken=this.$route.params.authToken,void 0===this.jwtToken&&(this.jwtToken=this.$cookies.get("omachi-auth-token"),null===this.jwtToken&&this.$router.push("login"))},mounted:function(){var e=this;return Object(y["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.next=2,e.getUser();case 2:case"end":return t.stop()}}),t)})))()},methods:{logout:function(){this.jwtToken=this.$cookies.get("omachi-auth-token"),null!==this.jwtToken&&this.$cookies.remove("omachi-auth-token"),this.$nextTick((function(){this.$router.push("login")}))},getUser:function(){var e=this;return Object(y["a"])(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:if(e.userId=e.$cookies.get("omachi-user-id"),null===e.userId){t.next=4;break}return t.next=4,j.a.get(e.getUserUrl+e.userId,{headers:{Authorization:"Bearer ".concat(e.jwtToken)}}).then((function(t){e.user=t.data})).catch((function(e){console.log(e)}));case 4:case"end":return t.stop()}}),t)})))()}},watch:{}},O=_,$=Object(s["a"])(O,k,w,!1,null,"140b339a",null),T=$.exports;n["default"].use(f["a"]);var S=[{path:"/login",name:"login",component:v},{path:"/register",name:"register",component:g["default"]},{path:"/home",name:"home",component:T}],R=new f["a"]({mode:"history",routes:S}),E=R,P=r("5f5b"),U=r("b1e0");r("f9e3"),r("2dd8");n["default"].config.productionTip=!0,n["default"].use(f["a"]),n["default"].use(P["a"]),n["default"].use(U["a"]),new n["default"]({router:E,render:function(e){return e(l)}}).$mount("#app")},"85ec":function(e,t,r){},"882b":function(e,t,r){"use strict";r.d(t,"a",(function(){return n})),r.d(t,"b",(function(){return o}));var n=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div")},o=[]},8855:function(e,t){}});
//# sourceMappingURL=app.f5046364.js.map