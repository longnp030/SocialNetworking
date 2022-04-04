<template>
    <b-nav-item-dropdown right no-caret
        menu-class="notification-list-container"
        @toggle="$emit('toggle')"
        @hidden="$emit('hidden')"
    >
        <template #button-content>
            <em><b-icon icon="bell-fill"></b-icon></em>
        </template>
        <b-dropdown-item
            block
            v-for="noti in notis"
            :key="noti.Id"
        >
            <notification-card 
                :jwtToken="jwtToken"
                :notification="noti"/>
        </b-dropdown-item>
    </b-nav-item-dropdown>
</template>

<script>
    export default {
        name: "NotificationList",
        props: ["jwtToken", "myId", "start"],
        data() {
            return {
                getNotificationListUrl: "https://localhost:6868/Users/userId/notification/list",
                notis: [],
            }
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
        },
        methods: {
            async getNotificationList() {
                await this.$http.get(
                    this.getNotificationListUrl.replace("userId", this.myId)
                ).then(res => {
                    console.log(res.data);
                    this.notis = res.data;
                }).catch(err => {
                    console.log(err.response);
                });
            }
        },
        watch: {
            start: {
                deep: true,
                immediate: true,
                handler: async function () {
                    if (this.start) {
                        await this.getNotificationList();
                    }
                }
            }
        }
    }
</script>

<style>
    .notification-list-container {
        width: 360px !important;
        box-shadow: 0 0 6px #9ecaed;
    }
</style>