<template>
    <div id="user-profile">
        <user-card 
            :jwtToken="jwtToken"
            :meId="meId"
            :userId="userId"
            :full="fullUserCard"/>

        <b-card no-body>
            <b-tabs 
                card 
                active-nav-item-class="text-light bg-dark border-0" 
                active-tab-class="bg-dark"
                nav-wrapper-class="bg-dark"
                nav-class="bg-dark"
                align="center" 
                justified
            > 
                <b-tab title="Personal information">
                    <personal-information 
                        :userId="userId" />
                </b-tab>
                <b-tab title="Posts" active>
                    <feed 
                        v-if="userId"
                        :userId="userId"
                        :meId="meId"
                        :jwtToken="jwtToken"
                        :feedType="'posts'"/>
                </b-tab>
                <b-tab title="Media">
                    <b-card-text>Tab contents 2</b-card-text>
                </b-tab>
                <b-tab title="Follow">
                    <follow-list 
                        v-if="userId"
                        :meId="meId"
                        :userId="userId"
                        :jwtToken="jwtToken"/>
                </b-tab>
            </b-tabs>
        </b-card>

        <notification-alert :notification="notification" :jwtToken="jwtToken" :dismissNotification="dismissNotification"/>
    </div>
</template>

<script>
    export default {
        name: 'UserProfile',
        data() {
            return {
                jwtToken: '',
                meId: null,
                userId: null,

                fullUserCard: true,

                notification: null,
                dismissNotification: false,
            };
        },
        async created() {
            this.meId = this.$route.params.meId;
            if (this.meId === undefined || this.meId === null) {
                this.meId = this.$cookies.get('sn-user-id');
            }

            this.userId = this.$route.params.userId;
            //console.log(this.userId, this.meId);
            if (this.userId === undefined || this.userId === null) {
                this.$router.go(-1);
            }

            this.jwtToken = this.$route.params.jwtToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
            }
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;

            await this.$notificationHub.$on("notify", this.notify);
        },
        beforeDestroy() {
            this.$notificationHub.$off("notify", this.notify);
        },
        async mounted() {
        },
        methods: {
            async notify(noti) {
                this.notification = null;
                this.$nextTick(() => {
                    this.notification = noti;
                    this.dismissNotification = 1000;
                });
            },
        },
    }
</script>

<style scoped>
    #user-profile .card {
        border: 0;
    }
</style>