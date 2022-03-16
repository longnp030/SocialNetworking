<template>
    <b-media>
        <template #aside>
            <b-avatar variant="info" src="https://placekitten.com/300/300" size="4rem"></b-avatar>
        </template>
        
        <div class="noti-body">
            <p class="mb-0 text">{{ notificationText }}</p>
            <p class="time">{{calcTimeTillNow(notification.timestamp)}}</p>
        </div>
    </b-media>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'NotificationCard',
        props: ["notification", "jwtToken"],
        data() {
            return {
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                notificationText: '',
                from: null,
                to: null,
                entity: null,
            };
        },
        async mounted() {
            console.log(this.notification);
            await this.getProfile(this.notification.fromId, 'from');
            await this.getProfile(this.notification.toId, 'to');
            console.log(this.from, this.to);
            await this.makeNotificationText();
        },
        methods: {
            /**
             * get user to display (name, avatar, ...)
             * */
            async getProfile(id, where) {
                await axios.get(
                    this.getUserProfileUrl.replace("userId", id),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    if (where === 'from') {
                        this.from = res.data;
                    } else {
                        this.to = res.data;
                    }
                }).catch((res) => {
                    console.log(res);
                });
            },

            async makeNotificationText() {
                this.notificationText = `${this.from.Name} ${this.notification.verb} your ${this.notification.entityId}`;
            },

            /**
             * Display how much time from when the comment is posted till now
             * Still ugly coded
             * @param time somment's timestamp
             */
            calcTimeTillNow(time) {
                time = Date.now() - Date.parse(time);
                var years = Math.floor(time / 31556952000);
                if (years > 0) {
                    return years + " years ago";
                } else {
                    var months = Math.floor(time / 2629746000);
                    if (months > 0) {
                        return months + " months ago";
                    } else {
                        var weeks = Math.floor(time / 604800000);
                        if (weeks > 0) {
                            return weeks + " weeks ago";
                        } else {
                            var days = Math.floor(time / 86400000);
                            if (days > 0) {
                                return days + " days ago";
                            } else {
                                var hours = Math.floor(time / 3600000);
                                if (hours > 0) {
                                    return hours + " hours ago";
                                } else {
                                    var minutes = Math.floor(time / 60000);
                                    if (minutes > 0) {
                                        return minutes + " minutes ago";
                                    } else {
                                        var seconds = Math.floor(time / 1000);
                                        return seconds + " seconds ago";
                                    }
                                }
                            }
                        }
                    }
                }
            },
        },
    }
</script>

<style scoped>
    .noti-body .time {
        margin: 20px 0 0 0;
    }
</style>