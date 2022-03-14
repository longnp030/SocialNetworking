<template>
    <b-card v-if="author">
        <b-media>
            <template #aside>
              <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
            </template>

            <div class="d-inline-flex author-date">
                <b class="mt-0 author"><a :href="userProfileUrl" @click="$(this).stopPropagation();">{{ author.Name }}</a></b>
                <div>・</div>
                <div class="date" :title="toReadableTime(comment.Timestamp)">{{ calcTimeTillNow(comment.Timestamp) }}</div>
            </div>
            
            <p class="mb-0 text">
              {{ comment.Text }}
            </p>
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
    import axios from 'axios';

    export default {
        name: 'CommentCard',
        props: ["commentId", "userId", "jwtToken"],
        data() {
            return {
                getCommentUrl: "https://localhost:6868/Comments/commentId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                userProfileUrl: `http://localhost:8080/user/${this.userId}/profile`,
                comment: {},
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
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. get the comment to display (text, time, id, ...)
             * 2. get author to display (name, avatar, ...)
             * 
             * 3. check if viewing user has liked the comment ?
             * 4. list all who liked the comment
             * 
             * 5. get how many children comment there are
             */
            await this.getComment();
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
                await axios.get(
                    this.getCommentUrl.replace("commentId", this.commentId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.comment = res.data;
                    //console.log(this.comment);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * get author to display (name, avatar, ...)
             * */
            async getAuthorProfile() {
                await axios.get(
                    this.getUserProfileUrl.replace("userId", this.comment.AuthorId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
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
                axios.get(
                    this.getLikesUrl.replace("commentId", this.commentId),
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

            /**
             * check if viewing user has liked the comment ?
             * */
            async haveILiked() {
                axios.get(
                    this.checkLikedUrl.replace("commentId", this.commentId).replace("userId", this.userId),
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

            /**
             * Run when user click like
             * @param e like event
             */
            async likeComment(e) {
                console.log("Like...");
                e.stopPropagation();
                await axios.post(
                    this.likeCommentUrl.replace("commentId", this.commentId).replace("userId", this.userId),
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

            /**
             * Run when user click unlike
             * @param e unlike event
             */
            async unlikeComment(e) {
                e.stopPropagation();
                await axios.post(
                    this.unlikeCommentUrl.replace("commentId", this.commentId).replace("userId", this.userId),
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

            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run this
             * @param params equivalent to parameter named "comment" sent from Backend in Class CommentHub, Task Reaction()
             */
            async commentReacted(params) {
                console.log(params);
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
                axios.get(
                    this.getChildCountUrl.replace("commentId", this.commentId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then(res => {
                    this.comments = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },  

            /** 
             * Display how much time from when the comment is posted till now
             * Still ugly coded
             * @param time somment's timestamp
             */
            calcTimeTillNow(time) {
                time = Date.now() - Date.parse(time);
                var years = Math.floor(time / 31556952000);
                if (years > 0) {
                    return years + " years ago";
                } else {
                    var months = Math.floor(time / 2629746000);
                    if (months > 0) {
                        return months + " months ago";
                    } else {
                        var weeks = Math.floor(time / 604800000);
                        if (weeks > 0) {
                            return weeks + " weeks ago";
                        } else {
                            var days = Math.floor(time / 86400000);
                            if (days > 0) {
                                return days + " days ago";
                            } else {
                                var hours = Math.floor(time / 3600000);
                                if (hours > 0) {
                                    return hours + " hours ago";
                                } else {
                                    var minutes = Math.floor(time / 60000);
                                    if (minutes > 0) {
                                        return minutes + " minutes ago";
                                    } else {
                                        var seconds = Math.floor(time / 1000);
                                        return seconds + " seconds ago";
                                    }
                                }
                            }
                        }
                    }
                }
            },

            /**
             * Convert comment's timestamp to easily readable time
             * @param time comment's timestamp
             */
            toReadableTime(time) {
                time = new Date(time);
                return (time.getDate() < 10 ? "0" + time.getDate() : time.getDate()) + "/" +
                    ((time.getMonth() + 1) < 10 ? "0" + (time.getMonth() + 1) : (time.getMonth() + 1)) + "/" +
                    time.getFullYear() + " " +
                    (time.getHours() < 10 ? "0" + time.getHours() : time.getHours()) + ":" +
                    (time.getMinutes() < 10 ? "0" + time.getMinutes() : time.getMinutes()) + ":" + 
                    (time.getSeconds() < 10 ? "0" + time.getSeconds() : time.getSeconds());
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

    .card-body .media .media-aside img {
        border-radius: 64px;
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
</style>