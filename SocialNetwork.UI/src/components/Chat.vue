<template>
    <div class="chat" v-if="msgs">
        <div class="chat__header">
            <span class="chat__header__greetings">
                안녕하세요!
            </span>
        </div>

        <chat-history :msgs="msgs" :meId="meId"/>
        <chat-form @sendMessage="sendMessage"/>
    </div>
</template>

<script>
    import _ from 'lodash';
    export default {
        props: ["meId", "jwtToken", "userId"],
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
            await this.$chatHub.chatOpened(this.meId);
            await this.$chatHub.chatOpened(this.userId);

            await this.getChatHistory();
        },
        methods: {
            async getChatHistory() {
                await this.$http.get(
                    this.getChatHistoryUrl.replace("fromId", this.meId).replace("toId", this.userId)
                ).then(res => {
                    this.msgs = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async sendMessage(msg) {
                msg.FromId = this.meId;
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
            }
        },
    };
</script>

<style scoped>
    .chat {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

    .chat__header {
        background: #ffffff;
        box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.05);
        border-radius: 24px 24px 0px 0px;
        padding: 1.8rem;
        font-size: 16px;
        font-weight: 700;
    }

    .chat__header__greetings {
        color: #292929;
    }
</style>