<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    export default {
        name: 'app',
        created() {
            // If user go to 'home' -> route.name == null -> redirect to 'home'
            // Else means they jump into a complex url or refresh the page -> don't redirect to 'home'
            if (this.$route.name === null) {
                this.$router.push({
                    name: 'home'
                });
            }
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

    a, u {
        text-decoration: none;
    }
</style>

