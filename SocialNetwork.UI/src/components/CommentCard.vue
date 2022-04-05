<template>
    <div class="comment-wrapper" v-if="!isEditing">
        <b-card class="w-100">
            <b-media>
                <template #aside>
                    <b-avatar 
                        v-if="avatar"
                        :src="avatar" 
                        size="4rem"
                        button
                        @click="openUserProfile"
                        :id="commentId" 
                    ></b-avatar>
                </template>

                <b-popover 
                    :target="commentId" 
                    triggers="hover" 
                    placement="top"
                    custom-class="wide-popover"
                >
                    <user-card
                        :jwtToken="jwtToken"
                        :myId="myId"
                        :userId="comment.AuthorId"
                        :size="userCardSize"/>
                </b-popover>

                <b-container class="d-inline-flex author-date-opts" fluid>
                    <div class="author-date">
                        <b class="mt-0 author">{{ name }}</b>
                        <div>・</div>
                        <div class="date" :title="toReadableTime(comment.Timestamp)">{{ calcTimeTillNow(comment.Timestamp) }}</div>
                    </div>
                    <div class="opts" v-if="myId===comment.AuthorId">
                        <b-dropdown size="md" variant="link" toggle-class="text-decoration-none" no-caret right>
                            <template #button-content><b-icon icon="three-dots"></b-icon></template>
                            <b-dropdown-item @click="onEditClick"><b-icon icon="pen-fill"></b-icon>&nbsp;Edit</b-dropdown-item>
                            <b-dropdown-item @click="onDeleteClick"><b-icon icon="trash-fill"></b-icon>&nbsp;Delete</b-dropdown-item>
                        </b-dropdown>
                    </div>
                </b-container>
            
                <p class="mb-0 text">{{ comment.Text }}</p>

                <b-carousel
                    id="carousel-comment"
                    controls
                    indicators
                    v-if="commentMedia.length > 0"
                >
                    <b-carousel-slide 
                        v-for="media in commentMedia"
                        :key="media"
                        :img-src="media">
                    </b-carousel-slide>
                </b-carousel>

                <b-button-group class="d-flex like-cmt">
                    <div class="cmt">
                        <b-button variant="outline" @click="showChildren=!showChildren"><b-icon icon="chat" variant="light"></b-icon></b-button>
                        <div class="count">{{children.length}}</div>
                    </div>
                    <div class="like">
                        <b-button variant="outline" v-if="iLiked" @click="unlikeComment"><b-icon icon="heart-fill" variant="danger"></b-icon></b-button>
                        <b-button variant="outline" v-else @click="likeComment"><b-icon icon="heart" variant="light"></b-icon></b-button>
                        <div class="count" :class="{'liked': iLiked}">{{likes}}</div>
                    </div>
                </b-button-group>
            </b-media>
        </b-card>
        <div class="mt-1 sub-comments" v-if="showChildren">
            <h3>Replies</h3>
            <comment-form 
                :myId="myId" 
                :jwtToken="jwtToken" 
                :postId="postId"
                :commentId="commentId"/>
            <comment-card 
                :postId="postId"
                :myId="myId" :jwtToken="jwtToken"
                v-for="childId in children"
                :key="childId"
                :commentId="childId"/>
        </div>
    </div>
    <comment-form 
        v-else
        :comment="comment"
        :myId="myId"
        :jwtToken="jwtToken"/>
</template>

<script>
    import _ from 'lodash';
    export default {
        name: 'CommentCard',
        props: ["commentId", "myId", "jwtToken", "postId"],
        data() {
            return {
                getAvatarNameUrl: "https://localhost:6868/Users/userId/profile/avatarname",
                avatar: null,
                name: null,
                userCardSize: 'M',

                getCommentUrl: "https://localhost:6868/Comments/commentId",
                comment: {},
                commentMedia: [],

                iLiked: false,
                whoLiked: [],
                likes: 0,
                children: [],
                shares: 0,

                showChildren: false,
                isEditing: false,
            }
        },
        async created() {
            /**
             * Hub listener
             * whenever an event named "comment-react" triggered
             * run the method named "commentReacted"
             */
            await this.$commentHub.commentOpened(this.commentId);
            await this.$commentHub.$on('comment-react', this.commentReacted);
            await this.$commentHub.$on('comment-replied-to', this.commentRepliedTo);
            await this.$commentHub.$on('reply-deleted', this.replyDeleted);
            await this.$postHub.$on('comment-edited', this.commentEdited);
        },
        beforeDestroy() {
            this.$commentHub.commentClosed(this.commentId);
            this.$commentHub.$off('comment-react', this.commentReacted);
            this.$commentHub.$off('comment-replied-to', this.commentRepliedTo);
            this.$commentHub.$off('reply-deleted', this.replyDeleted);
            this.$postHub.$off('comment-edited', this.commentEdited);
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
            await this.getAvatarName();

            await this.haveILiked();
            await this.getWhoLiked();

            await this.getChildren();
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

            async getAvatarName() {
                await this.$http.get(
                    this.getAvatarNameUrl.replace("userId", this.comment.AuthorId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data.Avatar}`);
                    this.name = res.data.Name
                }).catch(err => {
                    console.log(err.response);
                });
            },
            openUserProfile(e) {
                this.$router.push({
                    name: 'user',
                    params: {
                        userId: this.comment.AuthorId,
                        myId: this.myId,
                        jwtToken: this.jwtToken
                    }
                }).catch(err => {
                    this.$router.go();
                });
                e.stopPropagation();
            },

            /**
             * get post's media
             * */
            async getMedia() {
                await this.$http.get(
                    this.getCommentUrl.replace("commentId", this.commentId) + "/media"
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
             * list all who liked the comment
             * */
            async getWhoLiked() {
                await this.$http.get(
                    this.getCommentUrl.replace("commentId", this.commentId) + "/likes"
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
                    this.getCommentUrl.replace("commentId", this.commentId) + "/likes/" + this.myId
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
                    this.getCommentUrl.replace("commentId", this.commentId) + "/likes/" + this.myId + "/like",
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
                    this.getCommentUrl.replace("commentId", this.commentId) + "/likes/" + this.myId + "/unlike",
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
                    if (this.myId === params.userId) {
                        this.iLiked = true;
                    }
                } else {
                    this.likes -= 1;
                    if (this.myId === params.userId) {
                        this.iLiked = false;
                    }
                }
            },

            /**
             * Count how many replies this comment has
             * */
            async getChildren() {
                await this.$http.get(
                    this.getCommentUrl.replace("commentId", this.commentId) + "/comments"
                ).then(res => {
                    this.children = res.data;
                    console.log(this.children);
                }).catch(res => {
                    console.log(res.response);
                });
            },

            onEditClick(e) {
                this.isEditing = true;
                e.stopPropagation();
            },

            async commentEdited(cmt) {
                cmt = _.mapKeys(cmt, function (v, k) {
                    return _.upperFirst(k);
                });

                if (this.comment.Id === cmt.Id) {
                    this.isEditing = false;
                    this.comment = cmt;
                }
            },

            onDeleteClick(e) {
                e.stopPropagation();
                this.$http.delete(
                    this.getCommentUrl.replace("commentId", this.commentId)
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async commentRepliedTo(replyId) {
                this.children.unshift(replyId);
            },

            async replyDeleted(replyId) {
                this.children.splice(this.children.indexOf(replyId), 1);
            },
        },
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
    }

    .author-date-opts {
        display: flex;
        justify-content: space-between;
        padding: 0;
    }

    .author-date {
        display: flex;
        gap: 10px;
        align-items: center;
    }

    .like-cmt {
        max-width: 68%;
        margin: 20px 0 0 -10px;
        justify-content: space-between;
    }

    .like-cmt .like,
    .like-cmt .cmt {
        display: inline-flex;
    }

    .like-cmt .btn {
        padding-right: 6px;
    }

    .like-cmt .count {
        line-height: 36px;
    }

    .count.liked {
        color: #dc3545;
    }

    .carousel {
        margin-top: 10px;
    }

    .wide-popover {
        min-width: max-content;
        background-color: #111;
        box-shadow: 0 0 10px #9ecaed;
    }

    .comment-wrapper {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .sub-comments {
        max-width: 86%;
    }
</style>