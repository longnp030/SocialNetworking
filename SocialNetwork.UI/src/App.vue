<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    export default {
        name: 'app',
        components: {

        },
        created() {
            this.$router.push('home', () => { });
        }
    };
</script>

<style>
    body, html {
        margin: 0px;
    }

    #app {
        height: 100vh;
        width: 100%;
    }
</style>

