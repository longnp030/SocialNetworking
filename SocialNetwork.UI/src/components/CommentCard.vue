<template>
    <b-card v-if="author">
        <b-media>
            <template #aside>
                <b-avatar variant="info" src="https://placekitten.com/300/300" size="4rem"></b-avatar>
            </template>

            <div class="d-inline-flex author-date">
                <b class="mt-0 author"><a :href="userProfileUrl" @click="$(this).stopPropagation();">{{ author.Name }}</a></b>
                <div>・</div>
                <div class="date" :title="toReadableTime(comment.Timestamp)">{{ calcTimeTillNow(comment.Timestamp) }}</div>
            </div>
            
            <p class="mb-0 text">
              {{ comment.Text }}
            </p>

            <b-carousel
                id="carousel-comment"
                :interval="4000"
                controls
                indicators
                img-width="1024"
                img-height="480"
                v-if="commentMedia.length > 0"
            >
                <b-carousel-slide 
                    v-for="media in commentMedia"
                    :key="media"
                    :img-src="media">
                </b-carousel-slide>
            </b-carousel>
        </b-media>

        <b-button-group class="d-flex like-cmt-share">
            <div class="cmt">
                <b-button variant="outline"><b-icon icon="chat" variant="light"></b-icon></b-button>
                <div class="count">{{comments}}</div>
            </div>
            <div class="like">
                <b-button variant="outline" v-if="iLiked" @click="unlikeComment"><b-icon icon="heart-fill" variant="danger"></b-icon></b-button>
                <b-button variant="outline" v-else @click="likeComment"><b-icon icon="heart" variant="light"></b-icon></b-button>
                <div class="count" :class="{'liked': iLiked}">{{likes}}</div>
            </div>
        </b-button-group>
    </b-card>
</template>

<script>
    export default {
        name: 'CommentCard',
        props: ["commentId", "userId", "jwtToken"],
        data() {
            return {
                getCommentUrl: "https://localhost:6868/Comments/commentId",
                getMediaUrl: "https://localhost:6868/Comments/commentId/media",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                userProfileUrl: `http://localhost:8080/user/${this.userId}/profile`,
                comment: {},
                commentMedia: [],
                author: null,

                getLikesUrl: "https://localhost:6868/Comments/commentId/likes",
                checkLikedUrl: "https://localhost:6868/Comments/commentId/likes/userId",
                likeCommentUrl: "https://localhost:6868/Comments/commentId/likes/userId/like",
                unlikeCommentUrl: "https://localhost:6868/Comments/commentId/likes/userId/unlike",

                iLiked: false,
                whoLiked: [],
                likes: 0,

                getChildCountUrl: "https://localhost:6868/Comments/commentId/comments/count",
                comments: 0,
                shares: 0,
            }
        },
        async created() {
            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run the method named "commentReacted"
             */
            await this.$commentHub.$on('comment-react', this.commentReacted);
            //console.log(this.$commentHub);
        },
        beforeDestroy() {
            this.$commentHub.$off('comment-react', this.commentReacted);
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. get the comment to display (text, time, id, ...)
             * 2. get comment's media
             * 3. get author to display (name, avatar, ...)
             * 
             * 4. check if viewing user has liked the comment ?
             * 5. list all who liked the comment
             * 
             * 6. get how many children comment there are
             */
            await this.getComment();
            await this.getMedia();
            await this.getAuthorProfile();

            await this.haveILiked();
            await this.getWhoLiked();

            await this.getChildCount();
        },
        methods: {
            /**
             * get the comment to display (text, time, id, ...)
             * */
            async getComment() {
                await this.$http.get(
                    this.getCommentUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    this.comment = res.data;
                    //console.log(this.comment);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * get post's media
             * */
            async getMedia() {
                await this.$http.get(
                    this.getMediaUrl.replace("commentId", this.commentId)
                ).then(res => {
                    res.data.forEach((val, idx) => {
                        //console.log(idx);
                        var media = require(`@/assets/${val}`);
                        this.commentMedia.push(media);
                    });
                }).catch(res => {
                    console.log(res.response);
                });
            },

            /**
             * get author to display (name, avatar, ...)
             * */
            async getAuthorProfile() {
                await this.$http.get(
                    this.getUserProfileUrl.replace("userId", this.comment.AuthorId)
                ).then((res) => {
                    this.author = res.data;
                    //console.log(this.author);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * list all who liked the comment
             * */
            async getWhoLiked() {
                await this.$http.get(
                    this.getLikesUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    this.likes = res.data.length;
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * check if viewing user has liked the comment ?
             * */
            async haveILiked() {
                await this.$http.get(
                    this.checkLikedUrl.replace("commentId", this.commentId).replace("userId", this.userId)
                ).then((res) => {
                    this.iLiked = res.data;
                    //console.log(this.liked);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            /**
             * Run when user click like
             * @param e like event
             */
            async likeComment(e) {
                //console.log("Like...");
                await this.$http.post(
                    this.likeCommentUrl.replace("commentId", this.commentId).replace("userId", this.userId),
                    null
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
                e.stopPropagation();
            },

            /**
             * Run when user click unlike
             * @param e unlike event
             */
            async unlikeComment(e) {
                await this.$http.post(
                    this.unlikeCommentUrl.replace("commentId", this.commentId).replace("userId", this.userId),
                    null
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
                e.stopPropagation();
            },

            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run this
             * @param params equivalent to parameter named "comment" sent from Backend in Class CommentHub, Task Reaction()
             */
            async commentReacted(params) {
                //console.log(params);
                if (this.commentId !== params.commentId) return;
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

            /**
             * Count how many replies this comment has
             * */
            async getChildCount() {
                await this.$http.get(
                    this.getChildCountUrl.replace("commentId", this.commentId)
                ).then(res => {
                    this.comments = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

        },
        watch: {
            comment: {
                immediate: true,
                deep: true,
                handler: function () {
                    //console.log(this.comment);
                }
            },
        }
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
    }

    .author-date {
        gap: 10px;
    }

    .date {
        margin-bottom: 8px;
    }

    .like-cmt-share {
        margin-top: 8px;
        justify-content: space-around;
    }

    .like-cmt-share .like,
    .like-cmt-share .cmt,
    .like-cmt-share .share {
        display: inline-flex;
    }

    .like-cmt-share .btn {
        padding-right: 6px;
    }

    .like-cmt-share .count {
        line-height: 36px;
    }

    .count.liked {
        color: #dc3545;
    }

    .carousel {
        margin-top: 10px;
    }
</style>