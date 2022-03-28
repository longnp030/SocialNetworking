<template>
    <div>
        <h1>Post</h1>
        <post-card :userId="userId" :jwtToken="jwtToken" :postId="postId"/>
        
        <b-container fluid id="comments">
            <h2>Comments</h2>
            <comment-form 
                         :userId="userId" 
                         :jwtToken="jwtToken" 
                         :postId="postId"/>
            <comment-card 
                         :userId="userId" :jwtToken="jwtToken"
                         v-for="commentId in commentIds"
                         :key="commentId"
                         :commentId="commentId"/>
        </b-container>
        <notification-alert :notification="notification" :jwtToken="jwtToken" :dismissNotification="dismissNotification"/>
    </div>
</template>

<script>
    export default {
        name: 'PostLayout',
        data() {
            return {
                userId: '',
                jwtToken: '',
                postId: '',

                getCommentsForPostUrl: "https://localhost:6868/Posts/postId/comments",
                commentIds: [],

                notification: null,
                dismissNotification: false,
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

            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run the method named "postCommentedOn"
             */
            await this.$postHub.$on('post-commented-on', this.postCommentedOn);
            await this.$postHub.$on('comment-deleted', this.commentDeleted);

            await this.$notificationHub.$on("notify", this.notify);
        },
        async beforeDestroy() {
            /**
             * Destroy hub when user is not viewing the post anymore
             * */
            this.$postHub.postClosed(this.postId);

            await this.$postHub.$off('post-commented-on', this.postCommentedOn);
            await this.$postHub.$off('comment-deleted', this.commentDeleted);
            this.$notificationHub.$off("notify", this.notify);
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
                this.$http.get(
                    this.getCommentsForPostUrl.replace("postId", this.postId)
                ).then(res => {
                    this.commentIds = res.data;
                    //console.log(this.commentIds);
                }).catch(res => {
                    console.log(res.response);
                })
            },

            /**
             * Hub listener
             * whenever an event named "post-commented-on" triggered
             * run this
             * @param comment equivalent to parameter named "comment" sent from Backend in Class PostHub, Task Comment()
             */
            async postCommentedOn(comment) {
                console.log(comment);
                this.commentIds.unshift(comment.id);
            },

            async commentDeleted(commentId) {
                console.log(commentId)
                this.commentIds.splice(this.commentIds.indexOf(commentId), 1);
                console.log(this.commentIds)
            },

            async notify(noti) {
                this.notification = null;
                //console.log(noti);
                this.$nextTick(() => {
                    this.notification = noti;
                    this.dismissNotification = 1000;
                });
            },
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