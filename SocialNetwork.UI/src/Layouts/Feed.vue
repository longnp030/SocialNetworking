<template>
    <div>
        <PostForm :userId="userId" :jwtToken="jwtToken"/>
        <PostCard 
            :userId="userId" :jwtToken="jwtToken"
            v-for="post in posts"
            :key="post.Id"
            :post="post"
            v-on="$listeners" />
    </div>
</template>

<script>
    import axios from 'axios';
    import PostForm from "@/components/PostForm.vue";
    import PostCard from "@/components/PostCard.vue";

    export default {
        name: 'Feed',
        components: {
            PostForm,
            PostCard,
        },
        props: ["userId", "jwtToken"],
        data() {
            return {
                getFeedUrl: "https://localhost:6868/Users/userId/posts",
                posts: [],
            }
        },
        async mounted() {
            console.log("Into Feed...");
            console.log(this.userId);
            if (this.userId) {
                console.log("Found userId: ", this.userId);
                await this.getPost();
            }
        },
        methods: {
            async getPost() {
                await axios.get(
                    this.getFeedUrl.replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.posts = res.data;
                    console.log(this.posts);
                }).catch((res) => {
                    console.log(res);
                });
            }
        },
        watch: {
            userId() {
                console.log("Feed: ", this.userId);
            },
            jwtToken() {
                console.log("Feed: ", this.jwtToken);
            }
        }
    }
</script>

<style scoped>
</style>