<template>
    <div>
        <h1>Post</h1>
        <PostCard :userId="userId" :jwtToken="jwtToken" :postId="postId"/>
        
        <div>
            <b-button variant="danger" v-if="iLiked" @click="unlikePost">Unlike</b-button>
            <b-button variant="primary" v-else @click="likePost">Like</b-button>
            {{likes}}
        </div>
        
        <b-container fluid id="comments">
            <h2>Comments</h2>
            <CommentForm 
                         :userId="userId" 
                         :jwtToken="jwtToken" 
                         :postId="postId"/>
            <CommentCard 
                         :userId="userId" :jwtToken="jwtToken"
                         v-for="commentId in commentIds"
                         :key="commentId"
                         :commentId="commentId"/>
        </b-container>
        
    </div>
</template>

<script>
    import axios from 'axios';
    import PostCard from "@/components/PostCard.vue";
    import CommentForm from "@/components/CommentForm.vue";
    import CommentCard from "@/components/CommentCard.vue";

    export default {
        name: 'PostLayout',
        components: {
            PostCard,
            CommentForm,
            CommentCard,
        },
        data() {
            return {
                userId: '',
                jwtToken: '',
                postId: '',
                getLikesUrl: "https://localhost:6868/Posts/postId/likes",
                checkLikedUrl: "https://localhost:6868/Posts/postId/likes/userId",
                likePostUrl: "https://localhost:6868/Posts/postId/likes/userId/like",
                unlikePostUrl: "https://localhost:6868/Posts/postId/likes/userId/unlike",
                iLiked: false,
                whoLiked: [],
                likes: 0,

                getCommentsForPostUrl: "https://localhost:6868/Posts/postId/comments",
                commentIds: [],
            }
        },
        async created() {
            this.userId = this.$route.params.userId;
            if (this.userId === undefined) {
                this.userId = this.$cookies.get('sn-user-id');
            }
            this.jwtToken = this.$route.params.jwtToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
            }
            this.postId = this.$route.params.postId;

            await this.$postHub.$on('post-react', this.postReacted);
            await this.$postHub.$on('post-commented-on', this.postCommentedOn);
        },
        async beforeDestroy() {
            console.log("B4 destroy...");
            this.$postHub.postClosed(this.postId);
        },
        async mounted() {
            await this.$postHub.postOpened(this.postId);
            await this.haveILiked();
            await this.getWhoLiked();
            await this.getComments();
        },
        methods: {
            async getWhoLiked() {
                axios.get(
                    this.getLikesUrl.replace("postId", this.postId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.likes = res.data.length;
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async haveILiked() {
                axios.get(
                    this.checkLikedUrl.replace("postId", this.postId).replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.iLiked = res.data;
                    //console.log(this.liked);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async likePost() {
                await axios.post(
                    this.likePostUrl.replace("postId", this.postId).replace("userId", this.userId),
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
                    this.unlikePostUrl.replace("postId", this.postId).replace("userId", this.userId),
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

            async postReacted(params) {
                console.log(params);
                if (this.postId !== params.postId) return;
                if (params.like) {
                    this.likes += 1;
                    if (this.userId === params.userId) {
                        this.iLiked = true;
                    }
                } else {
                    this.likes -= 1;
                    if (this.userId === params.userId) {
                        this.iLiked = false;
                    }
                }
            },

            async getComments() {
                axios.get(
                    this.getCommentsForPostUrl.replace("postId", this.postId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then(res => {
                    this.commentIds = res.data;
                    console.log(this.commentIds);
                }).catch(res => {
                    console.log(res.response);
                })
            },
            async postCommentedOn(comment) {
                console.log(comment);
                this.commentIds.unshift(comment.id);
            }
        },
    }
</script>

<style scoped>
</style>