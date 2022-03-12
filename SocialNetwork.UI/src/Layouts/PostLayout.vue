<template>
    <div>
        <PostCard :userId="userId" :jwtToken="jwtToken" :post="post"/>
    </div>
</template>

<script>
    import PostCard from "@/components/PostCard.vue";

    export default {
        name: 'Feed',
        components: {
            PostCard,
        },
        props: ["userId", "jwtToken", "post"],
    }
</script>

<style scoped>
</style>