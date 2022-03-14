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
        background-color: #000000;
    }

    #app {
        height: 100vh;
        width: 100%;
        background-color: #000000;
        color: var(--white);
    }

    #app a, u {
        text-decoration: none;
        color: var(--white);
    }
</style>

