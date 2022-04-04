<template>
    <div id="app">
        <b-navbar toggleable="lg" type="dark" id="navbar">
            <b-navbar-brand href="/home"><b-icon icon="twitter"></b-icon></b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav class="g-1">
                <b-navbar-nav class="ml-auto">
                    <b-navbar-brand @click="openUserProfile" class="info">
                        <b-avatar 
                            :src="avatar" 
                            size="2rem"
                            button
                            class="mr-2"
                            @click="openUserProfile"
                        ></b-avatar>
                        <span class="text-primary text-truncate">{{name}}</span>
                    </b-navbar-brand>

                    <chat-list 
                        class="ml-2"
                        :jwtToken="jwtToken"
                        :myId="myId"
                        :start="startGetChat"/>
                        <!--@toggle="startGetChat=true"
                        @hidden="startGetChat=false"/>-->

                    <notification-list 
                        class="ml-2"
                        :jwtToken="jwtToken"
                        :myId="myId" 
                        :start="startGetNotification"/>
                        <!--@toggle="startGetNotification=true"
                        @hidden="startGetNotification=false"/>-->

                    <b-nav-item-dropdown right no-caret class="ml-2">
                        <template #button-content>
                            <em><b-icon icon="grid3x3-gap-fill" font-scale="1.5"></b-icon></em>
                        </template>
                        <b-dropdown-item @click="logout">Sign Out</b-dropdown-item>
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
    import _ from 'lodash';
    export default {
        name: 'app',
        data() {
            return {
                jwtToken: null,
                myId: null,
                getUserInfoUrl: "https://localhost:6868/Users/userId/profile/avatarname",
                avatar: null,
                name: null,

                notification: null,
                dismissNotification: false,

                audio: new Audio(require('@/assets/notification.ogg')),
                chatBoxes: [],
                startGetChat: false,
                startGetNotification: false,
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

            // get credentials
            await this.$bus.$on("getCreds", (creds) => {
                this.jwtToken = creds.jwtToken;
                this.myId = creds.myId;
                this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            });

            await this.getUserInfo();

            // get chat and notification list after getting credentials
            this.startGetChat = true;
            this.startGetNotification = true;

            // listen to notification hub
            await this.$notificationHub.online(this.myId);
            await this.$notificationHub.$on("notify", this.notify);

            // Open chat box event listener
            await this.$bus.$on("startChat", ({ myId, chatId }) => {
                this.createChatBox(myId, chatId);
            });
        },
        beforeDestroy() {
            this.$notificationHub.$off("notify", this.notify);
        },
        methods: {
            async getUserInfo() {
                await this.$http.get(
                    this.getUserInfoUrl.replace("userId", this.myId)
                ).then(res => {
                    this.avatar = require(`@/assets/${res.data.Avatar}`);
                    this.name = res.data.Name;
                }).catch(err => {
                    console.log(err.response);
                });
            },
            openUserProfile(e) {
                this.$router.push({
                    name: 'user',
                    params: {
                        userId: this.myId,
                        myId: this.myId,
                        jwtToken: this.jwtToken
                    }
                }).catch(err => {
                    this.$router.go();
                });
                e.stopPropagation();
            },
            async logout() {
                this.jwtToken = this.$cookies.get('sn-auth-token');
                if (this.jwtToken !== null) {
                    await this.$cookies.remove('sn-auth-token');
                }
                this.myId = this.$cookies.get('sn-user-id');
                if (this.myId !== null) {
                    await this.$cookies.remove('sn-user-id');
                }
                await this.$nextTick(function () {
                    this.$router.push('login');
                });
            },

            async createChatBox(myId, chatId) {
                if (this.chatBoxes.length === 1) {
                    this.chatBoxes.splice(0);
                }
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
                noti = _.mapKeys(noti, function (v, k) {
                    return _.upperFirst(k);
                });

                this.audio.play();

                if (!noti.Verb.includes("message")) {
                    this.$nextTick(() => {
                        this.notification = noti;
                        this.dismissNotification = 1000;
                    });
                } else {
                    this.createChatBox(this.myId, noti.toId);
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

    .info {
        font-weight: bolder;
        padding: 4px 8px;
        background-color: #111;
        border-radius: 24px;
        opacity: 0.68;
        cursor: pointer;
    }
    .info:hover {
        opacity: 1;
    }
</style>

