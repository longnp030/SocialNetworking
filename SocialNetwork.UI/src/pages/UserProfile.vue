<template>
    <div id="user-profile" v-if="userProfile">
        <b-card
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
                        <h1 id="name">{{userProfile.Name}}</h1>
                        <div id="btn-actions">
                            <b-button variant="danger" v-if="iFollowed"><b-icon icon="person-dash"></b-icon></b-button>
                            <b-button variant="success" v-else><b-icon icon="person-plus"></b-icon></b-button>
                            <b-button variant="info"><b-icon icon="chat-text"></b-icon></b-button>
                        </div>
                    </div>
                    <div id="about-me"><b-icon icon="info-circle"></b-icon>{{userProfile.SelfIntroduction}}</div>
                    <div id="address"><b-icon icon="geo-alt"></b-icon>{{userProfile.CurrentLocation}}</div>
                    <div id="follow"><b-icon icon="people"></b-icon>
                        <div>40 followers<div>・</div>40 following</div>
                    </div>
                    <div id="mutual">Mo foro- sarete imasu</div>
                </div>
            </div>
            <div id="divider"></div>
            <div id="profile-nav">
                <b-nav justified card-header>
                    <b-nav-item>Personal information</b-nav-item>
                    <b-nav-item>Posts</b-nav-item>
                    <b-nav-item>Media</b-nav-item>
                    <b-nav-item>Follow</b-nav-item>
                </b-nav>
            </div>
        </b-card>
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'UserProfile',
        data() {
            return {
                getUserUrl: "https://localhost:6868/Users/userId",
                getUserProfileUrl: "https://localhost:6868/Users/userId/profile",
                jwtToken: '',
                meId: null,
                userId: null,
                user: null,
                userProfile: null,

                iFollowed: false,
            };
        },
        async created() {
            this.meId = this.$route.params.meId;
            if (this.meId === undefined || this.meId === null) {
                this.meId = this.$cookies.get('sn-user-id');
            }

            this.userId = this.$route.params.userId;
            console.log(this.userId, this.meId);
            if (this.userId === undefined || this.userId === null) {
                this.$router.go(-1);
            }

            this.jwtToken = this.$route.params.jwtToken;
            if (this.jwtToken === undefined) {
                this.jwtToken = this.$cookies.get('sn-auth-token');
            }
        },
        async mounted() {
            await this.getUser();
            await this.getProfile();
        },
        methods: {
            async getUser() {
                if (this.userId) {
                    await axios.get(
                        this.getUserUrl.replace("userId", this.userId),
                        {
                            headers: {
                                Authorization: `Bearer ${this.jwtToken}`
                            }
                        }
                    ).then((res) => {
                        this.user = res.data;
                        //console.log(this.user);
                    }).catch((res) => {
                        console.log(res.response);
                    });
                }
            },

            async getProfile() {
                await axios.get(
                    this.getUserProfileUrl.replace("userId", this.userId),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    this.userProfile = res.data;
                    console.log(this.userProfile);
                }).catch((res) => {
                    console.log(res.response);
                });
            },
        },
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
    }

    .card-img-top {
        padding: 0 0 20px 0;
        max-height: 400px;
        border-radius: 8px;
        border-top-left-radius: 0px;
        border-top-right-radius: 0px;
    }

    #basic-info {
        display: flex;
        flex-direction: row;
        gap: 50px;
        align-items: center;
        width: 100%;
        padding: 68px 68px 36px 68px;
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

    #name-follow #btn-actions {
        display: flex;
        gap: 5px;
        align-items: end;
        margin-bottom: 18px;
    }
    #name-follow #btn-actions .btn {
        max-height: 30px;
        display: flex;
        align-items: center;
    }
    #btn-actions .btn .b-icon {
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

    #divider {
        border-bottom: 1px solid var(--white);
    }
</style>