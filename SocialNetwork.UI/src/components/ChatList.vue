<template>
    <b-nav-item-dropdown right no-caret
        menu-class="chat-list-container"
        @toggle="$emit('toggle')"
        @hidden="$emit('hidden')"
    >
        <template #button-content>
            <em><b-icon icon="chat-fill"></b-icon>
            <b-badge pill variant="danger">{{unreadCount}}</b-badge></em>
        </template>
        <b-dropdown-item
            block
            v-for="msg in msgs"
            :key="msg.Id"
        >
            <chat-card 
                :jwtToken="jwtToken"
                :myId="myId"
                :msg="msg" />
        </b-dropdown-item>
    </b-nav-item-dropdown>
</template>

<script>
import { forEach } from "lodash";

    export default {
        name: "ChatList",
        props: ["jwtToken", "myId", "start"],
        data() {
            return {
                getChatListUrl: "https://localhost:6868/Chat/userId/list",
                msgs: [],
            }
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
        },
        methods: {
            async getChatList() {
                await this.$http.get(
                    this.getChatListUrl.replace("userId", this.myId)
                ).then(res => {
                    console.log(res.data);
                    this.msgs = res.data;
                }).catch(err => {
                    console.log(err.response);
                });
            }
        },
        watch: {
            start: {
                deep: true,
                immediate: true,
                handler: async function () {
                    if (this.start) {
                        await this.getChatList();
                    }
                }
            }
        },
        computed: {
            unreadCount() {
                let count = 0;
                for (var msg in this.msgs) {
                    if (msg.UserId === this.myId && !msg.Read) {
                        count++;
                    }
                }
                return count;
            }
        }
    }
</script>

<style>
    .chat-list-container {
        width: 360px !important;
        box-shadow: 0 0 6px #9ecaed;
    }
</style>