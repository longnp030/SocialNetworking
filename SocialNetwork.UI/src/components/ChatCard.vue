<template>
    <b-media
        :class="[unreadMsg ? 'new__msg' : '']"
        @click="startChat">
        <template #aside>
            <b-avatar v-if="avatar" :src="avatar" size="4rem"></b-avatar>
        </template>

        <b-container fluid class="w-100 p-0 msg-card-body">
            <div class="name"><h5 class="mt-0">{{name}}</h5></div>
            <div class="msg">
                <p v-if="this.myId===this.msg.UserId" class="m-0">You</p>
                <p v-else class="m-0">{{name}}</p>
                ：
                <p class="m-0">{{msg.Text}}</p>
                ・
                <p class="m-0 time">{{ calcTimeTillNow(msg.Timestamp) }}</p>
            </div>
        </b-container>
        <b-icon font-scale="0.8" icon="circle-fill" v-if="unreadMsg" class="unread-msg-icon"></b-icon>
    </b-media>
</template>

<script>
    export default {
        name: "ChatCard",
        props: ["jwtToken", "myId", "msg"],
        data() {
            return {
                getChatNameUrl: "https://localhost:6868/Chat/chatId/name",
                getOneToOneChatBuddyId: "https://localhost:6868/Chat/chatId/userId/buddy/",
                getBuddyInfo: "https://localhost:6868/Users/userId/profile/avatarname",
                name: null,
                avatar: null,
                unreadMsg: false,
            }
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
            if (this.msg.UserId !== this.myId && !this.msg.Read) {
                this.unreadMsg = true;
            }
        },
        async mounted() {
            await this.getChat();
        },
        methods: {
            async getChat() {
                await this.$http.get(
                    this.getChatNameUrl.replace("chatId", this.msg.ChatId)
                ).then(res => {
                    if (res.data === "") {
                        this.$http.get(
                            this.getOneToOneChatBuddyId.replace("chatId", this.msg.ChatId).replace("userId", this.myId)
                        ).then(res => {
                            let buddyId = res.data;
                            this.$http.get(
                                this.getBuddyInfo.replace("userId", buddyId)
                            ).then(res => {
                                this.name = res.data.Name;
                                this.avatar = require(`@/assets/${res.data.Avatar}`);
                            }).catch(err => {
                                console.log(err.response);
                            });
                        }).catch(err => {
                            console.log(err);
                        });
                    } else {
                        this.name = res.data;
                    }
                }).catch(err => {
                    console.log(err);
                });
            },

            startChat() {
                this.$bus.$emit('startChat', { myId: this.myId, chatId: this.msg.ChatId });
            },
        }
    }
</script>

<style scoped>
    .name,
    .time {
        color: var(--white) !important;
    }

    .msg,
    .media-body {
        display: flex;
        flex-direction: row;
    }
    .msg-card-body {
        display: flex;
        flex-direction: column;
        align-items: start;
    }

    .new__msg {
        color: var(--primary);
    }

    .unread-msg-icon {
        margin-top: 22px;
    }

    .name {
        margin-left: -1px;
    }
</style>