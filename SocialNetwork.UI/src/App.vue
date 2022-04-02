<template>
    <div id="app">
        <b-navbar toggleable="lg" type="dark" id="navbar">
            <b-navbar-brand href="/home"><b-icon icon="twitter"></b-icon></b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-form>
                        <b-button size="sm" class="w-10 ml-4 search-btn" type="submit"><b-icon icon="search"></b-icon></b-button>
                        <b-form-input class="w-90 search-input" size="sm" placeholder="Search"></b-form-input>
                    </b-nav-form>
                </b-navbar-nav>

                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-nav-item-dropdown right>
                        <!-- Using 'button-content' slot -->
                        <template #button-content>
                            <em>Messages</em>
                        </template>
                        <b-dropdown-item href="#">Profile</b-dropdown-item>
                        <b-dropdown-item href="#">Sign Out</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>

        <router-view id="content"></router-view>

        <div v-if="chatBoxes.length > 0">
            <chat
                v-for="chatBox in chatBoxes"
                :key="chatBox.chatId"
                :jwtToken="jwtToken"
                :myId="chatBox.myId"
                :chatId="chatBox.chatId"
                @closeChat="closeChat"/>
        </div>

        <notification-alert :notification="notification" :jwtToken="jwtToken" :dismissNotification="dismissNotification"/>
    </div>
</template>

<script>
    export default {
        name: 'app',
        data() {
            return {
                jwtToken: null,
                myId: null,

                notification: null,
                dismissNotification: false,

                getChatIdUrl: "https://localhost:6868/Chat/fromId/toId",
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

            await this.$bus.$on("getCreds", (creds) => {
                this.jwtToken = creds.jwtToken;
                this.myId = creds.myId;
                this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            });

            await this.$notificationHub.online(this.myId);
            await this.$notificationHub.$on("notify", this.notify);

            // Open chat box event listener
            await this.$bus.$on("startChat", ({ myId, userId }) => {
                this.createChatBox(myId, userId);
            });
        },
        beforeDestroy() {
            this.$notificationHub.$off("notify", this.notify);
        },
        methods: {
            async createChatBox(myId, userId) {
                let chatId = null;
                await this.$http.get(
                    this.getChatIdUrl.replace("fromId", myId).replace("toId", userId)
                ).then(res => {
                    chatId = res.data;
                }).catch(err => {
                    console.log(err);
                });

                await this.chatBoxes.push({
                    myId: myId,
                    chatId: chatId,
                });
            },

            closeChat(toId) {
                let thisChat = this.chatBoxes.find(c => c.ToId === toId);
                this.chatBoxes.splice(this.chatBoxes.indexOf(thisChat), 1);
            },

            async notify(noti) {
                this.notification = null;
                console.log(noti);
                if (!noti.verb.includes("message")) {
                    this.$nextTick(() => {
                        this.notification = noti;
                        this.dismissNotification = 1000;
                    });
                } else {
                    if (noti.toId === this.myId) {
                        this.createChatBox(noti.toId, noti.fromId);
                    }
                }
            },
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

    #navbar {
        z-index: 2;
        background-color: #000;
        position: fixed;
        top: 0;
        width: 100%;

        display: flex;
        align-items: center;
    }

    .navbar-brand {
        display: flex !important;
    }

    .search-input,
    .search-btn {
        border-radius: 10px !important;
        background-color: #111 !important;
    }

    #content {
        margin-top: 56px;
    }

    .dropdown-menu {
        background-color: #111 !important;
    }
    .dropdown-item:hover {
        background-color: #222 !important;
    }
</style>

