<template>
    <b-card v-if="author" @click="postCardOnClick">
        <b-media>
            <template #aside>
              <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
            </template>

            <div class="d-inline-flex author-date">
                <b class="mt-0 author"><a :href="userProfileUrl" @click="$(this).stopPropagation();">{{ author.Name }}</a></b>
                <div>・</div>
                <div class="date" :title="toReadableTime(post.Timestamp)">{{ calcTimeTillNow(post.Timestamp) }}</div>
            </div>
            
            <p class="mb-0 text">
              {{ post.Text }}
            </p>
        </b-media>

        <b-button-group class="d-flex like-cmt-share">
            <div class="share">
                <b-button variant="outline"><b-icon icon="share"></b-icon></b-button>
                <div class="count">{{shares}}</div>
            </div>
            <div class="cmt">
                <b-button variant="outline"><b-icon icon="chat"></b-icon></b-button>
                <div class="count">{{comments}}</div>
            </div>
            <div class="like">
                <b-button variant="outline" v-if="iLiked" @click="unlikePost"><b-icon icon="heart-fill" variant="danger"></b-icon></b-button>
                <b-button variant="outline" v-else @click="likePost"><b-icon icon="heart"></b-icon></b-button>
                <div class="count" :class="{'liked': iLiked}">{{likes}}</div>
            </div>
        </b-button-group>
    </b-card>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'PostCard',
        props: ["postId", "userId", "jwtToken"],
        data() {
            return {
                getPostUrl: "https://localhost:6868/Posts/postId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                userProfileUrl: `http://localhost:8080/user/${this.userId}/profile`,
                post: {},
                author: null,

                getLikesUrl: "https://localhost:6868/Posts/postId/likes",
                checkLikedUrl: "https://localhost:6868/Posts/postId/likes/userId",
                likePostUrl: "https://localhost:6868/Posts/postId/likes/userId/like",
                unlikePostUrl: "https://localhost:6868/Posts/postId/likes/userId/unlike",

                iLiked: false,
                whoLiked: [],
                likes: 0,

                getCommentCountUrl: "https://localhost:6868/Posts/postId/comments/count",
                comments: 0,
                shares: 0,
            }
        },
        async created() {
            /**
             * Hub listener
             * whenever an event named "post-react" triggered
             * run the method named "postReacted"
             */
            await this.$postHub.$on('post-react', this.postReacted);
        },
        async mounted() {
            /**
             * What to do when component is mounted:
             * 1. get the post to display (text, time, id, ...)
             * 2. get author to display (name, avatar, ...)
             *
             * 3. check if viewing user has liked the post ?
             * 4. list all who liked the post
             *
             * 5. get how many comments there are
             */
            await this.getPost();
            await this.getAuthorProfile();

            await this.haveILiked();
            await this.getWhoLiked();

            await this.getCommentCount();
        },
        methods: {
            /**
             * get the post to display (text, time, id, ...)
             * */
            async getPost() {
                await axios.get(
                    this.getPostUrl.replace("postId", this.postId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.post = res.data;
                    //console.log(this.post);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * get author to display (name, avatar, ...)
             * */
            async getAuthorProfile() {
                await axios.get(
                    this.getUserProfileUrl.replace("userId", this.post.AuthorId),
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
             * list all who liked the post
             * */
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

            /**
             * check if viewing user has liked the comment ?
             * */
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

            /**
             * Run when user click like
             * @param e like event
             */
            async likePost(e) {
                e.stopPropagation();
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

            /**
             * Run when user click unlike
             * @param e unlike event
             */
            async unlikePost(e) {
                e.stopPropagation();
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

            /**
             * Hub listener
             * whenever an event named "post-react" triggered
             * run this
             * @param params equivalent to parameters {postId, userId, like} sent from Backend in Class PostHub, Task Reaction()
             */
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

            /**
             * Count how many replies this comment has
             * */
            async getCommentCount() {
                axios.get(
                    this.getCommentCountUrl.replace("postId", this.postId),
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
             * Redirect to PostLayout single page -> Full view of the post
             * */
            postCardOnClick() {
                if (this.$route.path !== `/post/${this.postId}`) {
                    this.$router.push({
                        name: 'post',
                        params: {
                            postId: this.postId,
                            userId: this.userId,
                            jwtToken: this.jwtToken
                        }
                    })
                }
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
            post: {
                immediate: true,
                deep: true,
                handler: function () {
                    //console.log(this.post);
                }
            },
        }
    }
</script>

<style scoped>
    a, u {
        text-decoration: none !important;
    }

    .card-body {
        padding: 10px;
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