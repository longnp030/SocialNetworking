<template>
    <b-container fluid>
        <b-row cols="3">
            <b-col cols="3">Column</b-col>
            <b-col cols="6">
                <feed
                    v-if="userId"
                    :userId="userId"
                    :meId="userId"
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
                userId: "",
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
            await this.$notificationHub.online(this.userId);
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
                this.userId = this.$cookies.get('sn-user-id');
                if (this.userId !== null) {
                    await this.$http.get(
                        this.getUserUrl.replace("userId", this.userId)
                    ).then((res) => {
                        this.user = res.data;
                        //console.log(this.user);
                    }).catch((res) => {
                        console.log(res);
                    });
                }
            },
    
            async getUserProfile() {
                var self = this;
                await this.$http.get(
                    this.getUserProfileUrl.replace("userId", this.userId)
                ).then(res => {
                    if (!res.data.Timestamp) {
                        this.$router.push({
                            name: 'updateprofile',
                            params: {
                                userProfile: res.data,
                                userId: self.userId,
                                jwtToken: self.jwtToken
                            }
                        });
                    }
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async notify(noti) {
                this.notification = null;
                console.log(noti);
                this.$nextTick(() => {
                    this.notification = noti;
                    this.dismissNotification = 1000;
                });
            },
        },
        watch: {
        },
    };
</script>

<style scoped>
    
</style>