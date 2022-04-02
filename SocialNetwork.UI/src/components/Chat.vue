<template>
    <div class="chat" v-if="msgs">
        <div class="chat__header">
            <span class="chat__header__greetings">
                {{chatName}}
            </span>
            <b-icon icon="x" class="close-chat-btn" @click="closeChat"></b-icon>
        </div>

        <chat-history :msgs="msgs" :myId="myId"/>
        <chat-form @sendMessage="sendMessage"/>
    </div>
</template>

<script>
    import _ from 'lodash';
    export default {
        props: ["myId", "jwtToken", "chatId"],
        data() {
            return {
                getChatNameUrl: "https://localhost:6868/Chat/chatId/name",
                getOneToOneChatBuddyId: "https://localhost:6868/Chat/chatId/userId/buddy/",
                getBuddyName: "https://localhost:6868/Users/userId/profile/name",
                getChatHistoryUrl: "https://localhost:6868/Chat/chatId/history",
                sendMessageUrl: "https://localhost:6868/Chat/messages",

                chatName: null,
                msgs: null,
            };
        },
        created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;

            this.$chatHub.$on('message-received', this.messageReceived);
        },
        async mounted() {
            await this.$chatHub.chatOpened(this.chatId);
            await this.getChat();
            await this.getChatHistory();
        },
        beforeDestroy() {
            this.$chatHub.$off('message-received', this.messageReceived);
        },
        methods: {
            async getChat() {
                await this.$http.get(
                    this.getChatNameUrl.replace("chatId", this.chatId)
                ).then(res => {
                    if (res.data === "") {
                        this.$http.get(
                            this.getOneToOneChatBuddyId.replace("chatId", this.chatId).replace("userId", this.myId)
                        ).then(res => {
                            let buddyId = res.data;
                            this.$http.get(
                                this.getBuddyName.replace("userId", buddyId)
                            ).then(res => {
                                this.chatName = res.data;
                            }).catch(err => {
                                console.log(err.response);
                            });
                        }).catch(err => {
                            console.log(err.response);
                        });
                    } else {
                        this.chatName = res.data;
                    }
                }).catch(err => {
                    console.log(err);
                });
            },
            async getChatHistory() {
                await this.$http.get(
                    this.getChatHistoryUrl.replace("chatId", this.chatId)
                ).then(res => {
                    this.msgs = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async sendMessage(msg) {
                msg.UserId = this.myId;
                msg.ChatId = this.chatId;
                //console.log(msg)
                await this.$http.post(
                    this.sendMessageUrl,
                    msg
                ).then(res => {
                    console.log(res);
                }).catch(res => {
                    console.log(res.response);
                });
            },

            messageReceived(message) {
                message = _.mapKeys(message, function (v, k) {
                    return _.upperFirst(k);
                });
                console.log(message);
                this.msgs.push(message);
            },

            closeChat() {
                this.$emit("closeChat", this.chatId);
            },
        },
    };
</script>

<style scoped>
    .chat {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        box-shadow: 0 0 6px #9ecaed;
        border-radius: 4px;
        position: fixed;
        bottom: 0;
        right: 20px;
        z-index: 1;
    }

    .chat__header {
        background-color: #343a40;
        box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.05);
        border-radius: 4px 4px 0 0;
        padding: 10px;
        font-size: 16px;
        font-weight: 700;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .chat__header__greetings {
        color: var(--white);
    }

    .close-chat-btn {
        cursor: pointer;
    }
</style>