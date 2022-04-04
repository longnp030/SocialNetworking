<template>
    <b-media
        :class="[unreadNoti ? 'new__noti' : '']"
        @click="notificationCardOnClick">
        <template #aside>
            <b-avatar :src="avatar" size="4rem"></b-avatar>
        </template>
        
        <div class="noti-body">
            <p class="mb-0 text">{{notification.Verb}}</p>
            <p class="time">{{calcTimeTillNow(notification.Timestamp)}}</p>
        </div>
        <b-icon font-scale="0.8" icon="circle-fill" v-if="unreadNoti" class="unread-noti-icon"></b-icon>
    </b-media>
</template>

<script>
    export default {
        name: 'NotificationCard',
        props: ["notification", "jwtToken"],
        data() {
            return {
                getAvatarUrl: "https://localhost:6868/Users/userId/profile/avatar",
                getEntityTypeUrl: "https://localhost:6868/Entity/entityId",
                avatar: null,
                unreadNoti: true,
            };
        },
        created() {
            if (!this.notification.Read) {
                this.unreadNoti = true;
            }
        },
        async mounted() {
            await this.getAvatar();
        },
        methods: {
            /**
             * get user to display (name, avatar, ...)
             * */
            async getAvatar() {
                await this.$http.get(
                    this.getAvatarUrl.replace("userId", this.notification.FromId)
                ).then((res) => {
                    this.avatar = require(`@/assets/${res.data}`);
                }).catch((res) => {
                    console.log(res);
                });
            },

            /**
             * redirect to event's object if notification is clicked on
             * */
            async notificationCardOnClick() {
                await this.$http.get(
                    this.getEntityTypeUrl.replace("entityId", this.notification.EntityId)
                ).then(res => {
                    this.$router.push(`/${res.data}/${this.notification.EntityId}`);
                }).catch(res => {
                    console.log(res.response);
                })
            },
        },
    }
</script>

<style scoped>
    .noti-body .time {
        margin: 20px 0 0 0;
        color: var(--white);
    }

    .media-body {
        display: flex;
        flex-direction: row;
        gap: 10px;
    }

    .unread-noti-icon {
        margin-top: 24px;
    }

    .new__noti {
        color: var(--primary);
    }
</style>