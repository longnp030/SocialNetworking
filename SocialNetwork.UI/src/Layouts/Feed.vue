<template>
    <div id="feed">
        <PostForm :userId="userId" :jwtToken="jwtToken"/>
        <PostCard 
            :userId="userId" :jwtToken="jwtToken"
            v-for="postId in postIds"
            :key="postId"
            :postId="postId"/>
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
                getFeedUrl: "https://localhost:6868/Users/userId/feed",
                postIds: [],
            }
        },
        async mounted() {
            //console.log("Into Feed...");
            //console.log(this.userId);
            if (this.userId) {
                //console.log("Found userId: ", this.userId);
                await this.getFeed();
            }
        },
        methods: {
            async getFeed() {
                await axios.get(
                    this.getFeedUrl.replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.postIds = res.data;
                    //console.log(this.postIds);
                }).catch((res) => {
                    console.log(res);
                });
            }
        },
        //watch: {
        //    userId() {
        //        console.log("Feed: ", this.userId);
        //    },
        //    jwtToken() {
        //        console.log("Feed: ", this.jwtToken);
        //    }
        //}
    }
</script>

<style scoped>
    #feed {
        display: flex;
        flex-direction: column;
        gap: 20px;
    }    
</style>