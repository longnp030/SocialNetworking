import Vue from 'vue';
import App from './App.vue';

Vue.config.productionTip = true;

/** Chat Socket **/
import PostSocket from './plugins/hubs/postHub';
Vue.use(PostSocket);
/** End Chat Socket **/

/** Vue router **/
import router from './plugins/router';
import VueRouter from 'vue-router';
Vue.use(VueRouter);
/** End Vue router **/

/** Vue cookies **/
import VueCookies from 'vue-cookies';
Vue.use(VueCookies);
/** End Vue cookies **/

import { BootstrapVue, BootstrapVueIcons } from 'bootstrap-vue';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
Vue.use(BootstrapVue);
Vue.use(BootstrapVueIcons);

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
