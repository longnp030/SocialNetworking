<template>
    <div class="chat" v-if="msgs">
        <div class="chat__header">
            <span class="chat__header__greetings">
                안녕하세요!
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
        props: ["myId", "jwtToken", "userId"],
        data() {
            return {
                getChatHistoryUrl: "https://localhost:6868/Chat/fromId/and/toId",
                sendMessageUrl: "https://localhost:6868/Chat/messages",
                msgs: null,
            };
        },
        created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;

            this.$chatHub.$on('message-received', this.messageReceived);
        },
        beforeDestroy() {
            this.$chatHub.$off('message-received', this.messageReceived);
        },
        async mounted() {
            await this.$chatHub.chatOpened(this.myId);
            await this.$chatHub.chatOpened(this.userId);

            await this.getChatHistory();
        },
        methods: {
            async getChatHistory() {
                await this.$http.get(
                    this.getChatHistoryUrl.replace("fromId", this.myId).replace("toId", this.userId)
                ).then(res => {
                    this.msgs = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async sendMessage(msg) {
                msg.FromId = this.myId;
                msg.ToId = this.userId;
                console.log(msg)
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
                this.$emit("closeChat", this.userId);
            },
        },
    };
</script>

<style scoped>
    .chat {
        display: flex;
        flex-direction: column;
        justify-content: space-between;

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