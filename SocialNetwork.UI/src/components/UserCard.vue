<template>
    <b-card
        v-if="full && profile"
        img-src="https://picsum.photos/600/300/?image=25"
        img-alt="Image"
        img-top
        tag="article"
        style="max-width: 20rem;"
        class="mb-2"
        id="profile-header"
    >
        <div id="basic-info">
            <b-avatar id="avatar" variant="info" src="https://placekitten.com/300/300" size="15rem"></b-avatar>
            <div id="info">
                <div id="name-follow">
                    <h1 id="name">{{profile.Name}}</h1>
                    <div id="actions" v-if="myId !== userId">
                        <b-button variant="outline-danger" v-if="iFollowed" @click="unfollow">
                            <b-icon icon="person-dash"></b-icon></b-button>
                        <b-button variant="outline-success" v-else @click="follow">
                            <b-icon icon="person-plus"></b-icon></b-button>
                        <b-button variant="outline-info" @click="startChat(jwtToken, myId, userId)"><b-icon icon="chat-text"></b-icon></b-button>
                    </div>
                </div>
                <div id="about-me"><b-icon icon="info-circle"></b-icon>{{profile.SelfIntroduction}}</div>
                <div id="address"><b-icon icon="geo-alt"></b-icon>{{profile.CurrentLocation}}</div>
                <div id="follow"><b-icon icon="people"></b-icon>
                    <div>{{followerCount}} followers<div>・</div>{{followeeCount}} following</div>
                </div>
                <div id="mutual">Mo foro- sarete imasu</div>
            </div>
        </div>
    </b-card>

    <b-card v-else>
        <div class="user-card-small" v-if="profile">
            <div class="avatar-name">
                <b-avatar class="mr-3"></b-avatar>
                <span class="mr-auto">{{profile.Name}}</span>
            </div>
            <div class="actions" v-if="myId !== userId">
                <b-button variant="outline-danger" v-if="iFollowed" @click="unfollow">
                    <b-icon icon="person-dash"></b-icon></b-button>
                <b-button variant="outline-success" v-else @click="follow">
                    <b-icon icon="person-plus"></b-icon></b-button>
                <b-button variant="outline-info" @click="startChat(jwtToken, myId, userId)"><b-icon icon="chat-text"></b-icon></b-button>
            </div>
        </div>
    </b-card>
</template>

<script>
    export default {
        name: 'UserCard',
        props: ["jwtToken", "myId", "userId", "full"],
        data() {
            return {
                getProfileUrl: "https://localhost:6868/Users/userId/profile",
                profile: null,

                followUrl: "https://localhost:6868/Users/fromId/follow/toId",
                getFollowerCountUrl: "https://localhost:6868/Users/userId/followers/count",
                getFolloweeCountUrl: "https://localhost:6868/Users/userId/followees/count",
                iFollowed: false,
                followerCount: null,
                followeeCount: null,
            };
        },
        async created() {
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;  
        },
        async mounted() {
            await this.getProfile();
            await this.haveIFollowed();

            await this.getFollowerCount();
            await this.getFolloweeCount();
        },
        methods: {
            async getProfile() {
                await this.$http.get(
                    this.getProfileUrl.replace("userId", this.userId)
                ).then((res) => {
                    this.profile = res.data;
                    //console.log(this.profile);
                }).catch((res) => {
                    console.log(res.response);
                });
            },

            async haveIFollowed() {
                await this.$http.get(
                    this.followUrl.replace("fromId", this.myId).replace("toId", this.userId)
                ).then(res => {
                    this.iFollowed = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async getFollowerCount() {
                await this.$http.get(
                    this.getFollowerCountUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followerCount = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async getFolloweeCount() {
                await this.$http.get(
                    this.getFolloweeCountUrl.replace("userId", this.userId)
                ).then(res => {
                    this.followeeCount = res.data;
                }).catch(res => {
                    console.log(res.response);
                });
            },

            async follow() {
                await this.$http.post(
                    this.followUrl.replace("fromId", this.myId).replace("toId", this.userId),
                    null
                ).then(res => {
                    console.log(res);
                    this.iFollowed = true;
                    this.followerCount += 1;
                }).catch(res => {
                    console.log(res.response);
                })
            },

            async unfollow() {
                await this.$http.post(
                    this.followUrl.replace("follow", "unfollow").replace("fromId", this.myId).replace("toId", this.userId),
                    null
                ).then(res => {
                    console.log(res);
                    this.iFollowed = false;
                    this.followerCount -= 1;
                }).catch(res => {
                    console.log(res.response);
                })
            },

            startChat(jwtToken, myId, userId) {
                this.$bus.$emit('startChat', { jwtToken, myId, userId });
            },
        }
    }
</script>

<style scoped>
    #profile-header {
        width: 100%;
        max-width: 100% !important;
        height: 100%;
        background-color: #111;
        color: var(--white);
        padding: 0 50px;
        margin: 0 !important;
    }

    .card-img-top {
        margin: 0 0 10px 0;
        max-height: 400px;
        border-radius: 10px;
        border-top-left-radius: 0px;
        border-top-right-radius: 0px;
    }

    #basic-info {
        display: flex;
        flex-direction: row;
        gap: 50px;
        align-items: center;
        width: 100%;
        padding: 68px 68px 10px 68px;
        margin-top: -150px;
    }

    #info {
        width: 100%;
    }

    #name-follow {
        display: flex;
        justify-content: space-between;
        align-items: end;
        width: 100%;
    }

    #name-follow #actions {
        display: flex;
        gap: 5px;
        align-items: end;
        margin-bottom: 18px;
    }

    #name-follow #actions .btn {
        max-height: 30px;
        display: flex;
        align-items: center;
    }

    #actions .btn .b-icon {
        margin: 0;
    }

    #name {
        margin-top: 68px;
    }

    #follow, #follow div {
        display: flex;
        flex-direction: row;
        gap: 10px;
    }

    #follow .b-icon {
        margin: 4px 0 0 0;
    }

    #name, #address, #about-me, #follow {
        margin-bottom: 10px;
    }

    .b-icon {
        margin-right: 10px;
    }

    .user-card-small {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        max-height: 20px;
        align-items: center;
    }
    .user-card-small .actions {
        display: flex;
        gap: 10px;
        align-items: end;
    }
    .user-card-small .actions .b-icon {
        margin: 0;
    }
</style>