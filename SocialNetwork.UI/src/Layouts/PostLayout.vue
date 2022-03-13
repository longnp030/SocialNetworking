<template>
    <div>
        <PostCard :userId="userId" :jwtToken="jwtToken" :post="post"/>
        <b-button variant="danger" v-if="liked" @click="unlikePost">Unlike</b-button>
        <b-button variant="primary" v-else @click="likePost">Like</b-button>
    </div>
</template>

<script>
    import axios from 'axios';
    import PostCard from "@/components/PostCard.vue";

    export default {
        name: 'Feed',
        components: {
            PostCard,
        },
        props: ["userId", "jwtToken", "post"],
        data() {
            return {
                getLikesUrl: "https://localhost:6868/Posts/postId/likes",
                checkLikedUrl: "https://localhost:6868/Posts/postId/likes/userId",
                likePostUrl: "https://localhost:6868/Posts/postId/likes/userId/like",
                unlikePostUrl: "https://localhost:6868/Posts/postId/likes/userId/unlike",
                liked: false,
                likes: [],
            }
        },
        async mounted() {
            this.addHashToLocation(`/post/${this.post.Id}`);
            await this.hasLiked();
            await this.getLikes();
        },
        methods: {
            addHashToLocation(params) {
                history.pushState(
                    {},
                    null,
                    this.$route.path + encodeURIComponent(params)
                );
            },

            async getLikes() {
                axios.get(
                    this.getLikesUrl.replace("postId", this.post.Id),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.likes = res.data;
                    console.log(this.likes);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async hasLiked() {
                axios.get(
                    this.checkLikedUrl.replace("postId", this.post.Id).replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.liked = res.data;
                    console.log(this.liked);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async likePost() {
                await axios.post(
                    this.likePostUrl.replace("postId", this.post.Id).replace("userId", this.userId),
                    null,
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async unlikePost() {
                await axios.post(
                    this.unlikePostUrl.replace("postId", this.post.Id).replace("userId", this.userId),
                    null,
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
            }
        },
    }
</script>

<style scoped>
</style>