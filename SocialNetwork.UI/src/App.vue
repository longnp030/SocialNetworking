<template>
    <div id="app">
        <div>
            <b-navbar toggleable="lg" type="dark">
                <b-navbar-brand href="#">NavBar</b-navbar-brand>

                <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

                <b-collapse id="nav-collapse" is-nav>
                    <b-navbar-nav>
                        <b-nav-item href="#">Link</b-nav-item>
                        <b-nav-item href="#" disabled>Disabled</b-nav-item>
                    </b-navbar-nav>

                        <!-- Right aligned nav items -->
                    <b-navbar-nav class="ml-auto">
                        <b-nav-form>
                            <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
                            <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
                        </b-nav-form>

                        <b-nav-item-dropdown text="Lang" right>
                            <b-dropdown-item href="#">EN</b-dropdown-item>
                            <b-dropdown-item href="#">ES</b-dropdown-item>
                            <b-dropdown-item href="#">RU</b-dropdown-item>
                            <b-dropdown-item href="#">FA</b-dropdown-item>
                        </b-nav-item-dropdown>

                        <b-nav-item-dropdown right>
                            <!-- Using 'button-content' slot -->
                            <template #button-content>
                                <em>User</em>
                            </template>
                            <b-dropdown-item href="#">Profile</b-dropdown-item>
                            <b-dropdown-item href="#">Sign Out</b-dropdown-item>
                        </b-nav-item-dropdown>
                    </b-navbar-nav>
                </b-collapse>
            </b-navbar>
        </div>
        <router-view></router-view>
        <div v-if="chatBoxes.length > 0">
            <chat
                v-for="chatBox in chatBoxes"
                :key="chatBox.toId"
                :jwtToken="chatBox.jwtToken"
                :meId="chatBox.fromId"
                :userId="chatBox.toId"/>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'app',
        data() {
            return {
                getChatHistoryUrl: "https://localhost:6868/Chat/fromId/and/toId",
                chatBoxes: [],
            };
        },
        async created() {
            // If user goes to 'home' -> route.name == null -> redirect to 'home'
            // Else means they jump into a complex url or refresh the page -> don't redirect to 'home'
            if (this.$route.name === null) {
                this.$router.push({
                    name: 'home'
                });
            }

            // Open chat box event listener
            await this.$bus.$on("startChat", ({ jwtToken, meId, userId }) => {
                this.$http.defaults.headers.common["Authorization"] = jwtToken;
                this.createChatBox(jwtToken, meId, userId);
            });
        },
        methods: {
            async createChatBox(jwtToken, fromId, toId) {
                this.chatBoxes.push({
                    jwtToken: jwtToken,
                    fromId: fromId,
                    toId: toId
                });
            }
        },
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

