<template>
    <b-card v-if="author">
        <b-media>
            <template #aside>
              <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
            </template>

            <div class="d-inline-flex">
                <h6 class="mt-0">{{ author.Name }}</h6>
                <div>{{ comment.Timestamp }}</div>
            </div>
            
            <p class="mb-0">
              {{ comment.Text }}
            </p>
        </b-media>
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
                comment: {},
                author: null,
            }
        },
        async mounted() {
            await this.getComment();
            await this.getAuthorProfile();
        },
        methods: {
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
                    console.log(res.response);
                });
            },

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
                    console.log(res.response);
                });
            },
        },
        watch: {
            commentId: {
                immediate: true,
                deep: true,
                handler: function () {
                    console.log(this.commentId);
                }
            },
        }
    }
</script>

<style scoped>
</style>