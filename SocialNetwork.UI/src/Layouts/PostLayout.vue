<template>
    <div>
        <h1>Post</h1>
        <PostCard :userId="userId" :jwtToken="jwtToken" :postId="postId"/>
        
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

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run the method named "postCommentedOn"
             */
            await this.$postHub.$on('post-commented-on', this.postCommentedOn);
        },
        async beforeDestroy() {
            /**
             * Destroy hub when user is not viewing the post anymore
             * */
            this.$postHub.postClosed(this.postId);
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. notify the hub that user has clicked to view the post
             *
             * 2. get how many comments there are
             */
            await this.$postHub.postOpened(this.postId);
            await this.getComments();
        },
        methods: {
            /**
             * get how many comments there are
             * */
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

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run this
             * @param params equivalent to parameter named "comment" sent from Backend in Class PostHub, Task Comment()
             */
            async postCommentedOn(comment) {
                console.log(comment);
                this.commentIds.unshift(comment.id);
            }
        },
    }
</script>

<style scoped>
    #comments {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }
</style>