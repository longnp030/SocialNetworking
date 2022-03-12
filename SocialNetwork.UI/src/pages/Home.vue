<template>
    <b-container fluid>
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
    </b-container>
</template>

<script>
    import axios from "axios";
    import Feed from "@/Layouts/Feed.vue";

    export default {
        name: 'Home',
        components: {
            Feed,
        },
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: "",
                userId: "",
            };
        },
        props: {
        },
        created() {
            this.jwtToken = this.$route.params.authToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken === null) {
                    console.log(this.jwtToken);
                    this.$router.push('login');
                }
            }
        },
        async mounted() {
            await this.getUser();
            await this.getUserProfile();
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
                        this.getUserUrl + this.userId,
                        {
                            headers: {
                                Authorization: `Bearer ${this.jwtToken}`
                            }
                        }
                    ).then((res) => {
                        this.user = res.data;
                        console.log(this.user);
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
                    }
                }).catch((res) => {
                    console.log(res.response);
                });
            },
        },
        watch: {
        },
    };
</script>

<style scoped>
</style>