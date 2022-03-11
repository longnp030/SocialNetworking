<template>
    <div></div>
</template>

<script>
    import axios from "axios";
    export default {
        name: 'Home',
        components: {
        },
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/",
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
        },
        watch: {
        },
    };
</script>

<style scoped>
</style>