<template>
    <b-container fluid>
        <b-row cols="3">
            <b-col cols="3">Column</b-col>
            <b-col cols="6">
                <component v-if="userId" v-bind:is="currentLayout"></component>
            </b-col>
            <b-col cols="3">Column</b-col>
        </b-row>
    </b-container>
</template>

<script>
    import axios from "axios";
    import Feed from "@/Layouts/Feed.vue";
    import PostLayout from "@/Layouts/PostLayout.vue";

    export default {
        name: 'Home',
        components: {
            Feed, PostLayout
        },
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: "",
                userId: "",

                currentLayout: null,
                layouts: [Feed, PostLayout],
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
            await this.swapLayout();
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
                    } else {
                        console.log(res.data);
                    }
                }).catch((res) => {
                    console.log(res.response);
                });
            },
            async swapLayout(layout) {
                console.log(layout);
                var self = this;
                var feedLayout = {
                    render(l) {
                        return l(Feed, {
                            props: {
                                is: Feed,
                                userId: self.userId,
                                jwtToken: self.jwtToken
                            },
                            on: {
                                swapLayout: self.swapLayout
                            }
                        })
                    }
                }

                var postLayout = {
                    render(l) {
                        return l(PostLayout, {
                            props: {
                                is: PostLayout,
                                userId: self.userId,
                                jwtToken: self.jwtToken,
                                post: layout.Post
                            },
                            on: {
                                swapLayout: self.swapLayout
                            }
                        })
                    }
                };

                if (layout) {
                    switch (layout.Name) {
                        case 'Feed':
                            this.currentLayout = feedLayout;
                            break;
                        case 'PostLayout':
                            this.currentLayout = postLayout;
                            break;
                        default:
                    }
                } else {
                    this.currentLayout = feedLayout;
                }
            }
        },
        watch: {
        },
    };
</script>

<style scoped>
</style>