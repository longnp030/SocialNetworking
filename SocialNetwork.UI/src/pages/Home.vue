<template>
    <b-container fluid>
        <b-row cols="3">
            <b-col cols="3">Column</b-col>
            <b-col cols="6">
                <feed
                    v-if="myId"
                    :userId="myId"
                    :myId="myId"
                    :jwtToken="jwtToken"
                    :feedType="'feed'"/>
            </b-col>
            <b-col cols="3">Column</b-col>
        </b-row>

        <notification-alert :notification="notification" :jwtToken="jwtToken" :dismissNotification="dismissNotification"/>
    </b-container>
</template>

<script>
    export default {
        name: 'Home',
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/userId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: "",
                myId: "",
                notification: null,
                dismissNotification: false,
            };
        },
        async created() {
            this.jwtToken = this.$route.params.authToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken === null) {
                    this.$router.push('login');
                }
            }
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            await this.$notificationHub.$on("notify", this.notify);
        },
        async mounted() {
            await this.getUser();
            await this.getUserProfile();

            await this.$notificationHub.online(this.myId);
        },
        beforeDestroy() {
            this.$notificationHub.$off("notify", this.notify);
        },
        methods: {
            logout() {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken !== null) {
                    this.$cookies.remove('sn-auth-token');
                }
                this.$nextTick(function () {
                    this.$router.push('login');
                });
            },

            async getUser() {
                this.myId = this.$cookies.get('sn-user-id');
                if (this.myId !== null) {
                    await this.$http.get(
                        this.getUserUrl.replace("userId", this.myId)
                    ).then((res) => {
                        this.user = res.data;
                    }).catch((res) => {
                        console.log(res);
                    });
                }
            },
    
            async getUserProfile() {
                var self = this;
                await this.$http.get(
                    this.getUserProfileUrl.replace("userId", this.myId)
                ).then(res => {
                    if (!res.data.Timestamp) {
                        this.$router.push({
                            name: 'updateprofile',
                            params: {
                                userProfile: res.data,
                                myId: self.myId,
                                jwtToken: self.jwtToken
                            }
                        });
                    }
                }).catch(res => {
                    console.log(res.response);
                });
            },

            startChat(jwtToken, myId, userId) {
                this.$bus.$emit('startChat', { jwtToken, myId, userId });
            },

            async notify(noti) {
                this.notification = null;
                //console.log(noti);
                if (!noti.verb.includes("message")) {
                    this.$nextTick(() => {
                        this.notification = noti;
                        this.dismissNotification = 1000;
                    });
                } else {
                    if (noti.toId === this.myId) {
                        this.startChat(this.jwtToken, noti.toId, noti.fromId);
                    }
                }
            },
        },
        watch: {
        },
    };
</script>

<style scoped>
    
</style>