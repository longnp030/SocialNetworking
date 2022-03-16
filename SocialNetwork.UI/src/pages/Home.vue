<template>
    <b-container fluid>
        <div>
            <b-navbar toggleable="lg" type="dark">
                <b-navbar-brand href="#">NavBar</b-navbar-brand>

                <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

                <b-collapse id="nav-collapse" is-nav>
                    <b-navbar-nav>
                        <b-nav-item href="#">Link</b-nav-item>
                        <b-nav-item href="#" disabled>Disabled</b-nav-item>
                    </b-navbar-nav>

                        <!-- Right aligned nav items -->
                    <b-navbar-nav class="ml-auto">
                        <b-nav-form>
                            <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
                            <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
                        </b-nav-form>

                        <b-nav-item-dropdown text="Lang" right>
                            <b-dropdown-item href="#">EN</b-dropdown-item>
                            <b-dropdown-item href="#">ES</b-dropdown-item>
                            <b-dropdown-item href="#">RU</b-dropdown-item>
                            <b-dropdown-item href="#">FA</b-dropdown-item>
                        </b-nav-item-dropdown>

                        <b-nav-item-dropdown right>
                            <!-- Using 'button-content' slot -->
                            <template #button-content>
                                <em>User</em>
                            </template>
                            <b-dropdown-item href="#">Profile</b-dropdown-item>
                            <b-dropdown-item href="#">Sign Out</b-dropdown-item>
                        </b-nav-item-dropdown>
                    </b-navbar-nav>
                </b-collapse>
            </b-navbar>
        </div>

        <b-row cols="3">
            <b-col cols="3">Column</b-col>
            <b-col cols="6">
                <Feed
                      v-if="userId"
                      :userId="userId"
                      :jwtToken="jwtToken"/>
            </b-col>
            <b-col cols="3">Column</b-col>
        </b-row>

        <b-alert
            v-if="notification"
            :show="dismissNotificationAlert"
            dismissible
            variant="dark"
            class="position-fixed fixed-bottom"
            @dismissed="dismissCountDown=0"
        >
            <NotificationCard :notification="notification" :jwtToken="jwtToken"/>
        </b-alert>
    </b-container>
</template>

<script>
    import axios from "axios";
    import Feed from "@/layouts/Feed.vue";
    import NotificationCard from "@/components/NotificationCard.vue"

    export default {
        name: 'Home',
        components: {
            Feed, NotificationCard
        },
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/userId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: "",
                userId: "",
                notification: null,
                dismissNotificationAlert: false,
            };
        },
        props: {
        },
        async created() {
            this.jwtToken = this.$route.params.authToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken === null) {
                    console.log(this.jwtToken);
                    this.$router.push('login');
                }
            }
            await this.$notificationHub.$on("notify", this.notify);
        },
        async mounted() {
            await this.getUser();
            await this.getUserProfile();
            await this.$notificationHub.online(this.userId);
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
                    await axios.get(
                        this.getUserUrl.replace("userId", this.userId),
                        {
                            headers: {
                                Authorization: `Bearer ${this.jwtToken}`
                            }
                        }
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
                await axios.get(
                    this.getUserProfileUrl.replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    if (!res.data.Timestamp) {
                        this.$router.push({
                            name: 'updateprofile',
                            params: {
                                userProfile: res.data,
                                userId: self.userId,
                                jwtToken: self.jwtToken
                            }
                        });
                    } else {
                        //console.log(res.data);
                    }
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async notify(notification) {
                //console.log(notification);
                this.notification = notification;
                this.dismissNotificationAlert = 1000;
            },
        },
        watch: {
        },
    };
</script>

<style scoped>
    .alert {
        color: var(--white);
        background-color: #111;
        border: 1px solid var(--white);
        max-width: 30%;
        margin-left: 20px;
    }
</style>