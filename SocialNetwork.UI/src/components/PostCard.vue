<template>
    <b-card v-if="author" @click="postCardOnClick">
        <b-media>
            <template #aside>
              <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
            </template>

            <div class="d-inline-flex">
                <h6 class="mt-0">{{ author.Name }}</h6>
                <div>{{ post.Timestamp }}</div>
            </div>
            
            <p class="mb-0">
              {{ post.Text }}
            </p>
        </b-media>
    </b-card>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'PostCard',
        props: ["post", "userId", "jwtToken"],
        data() {
            return {
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                author: null,
            }
        },
        async mounted() {
            await this.getAuthorProfile();
        },
        methods: {
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
                    console.log(this.author);
                }).catch((res) => {
                    console.log(res);
                });
            },
            postCardOnClick() {
                this.$emit('swapLayout', {
                    Name: 'PostLayout',
                    Post: this.post
                });
            },
        },
        watch: {
            post: {
                immediate: true,
                deep: true,
                handler: function () {
                    console.log(this.post);
                }
            },
        }
    }
</script>

<style scoped>
</style>