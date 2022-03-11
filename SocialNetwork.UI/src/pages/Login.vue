<template>
    <div>
        <b-form @submit="onSubmit" @reset="onReset" v-if="show">
            <b-form-group
            id="input-group-1"
            label="Email address:"
            label-for="input-1"
            >
                <b-form-input
                    id="input-1"
                    v-model="form.Email"
                    type="email"
                    placeholder="Enter email"
                    required
                ></b-form-input>
            </b-form-group>

            <b-form-group 
            id="input-group-2" 
            label="Password:" 
            label-for="input-2"
            >
                <b-form-input
                    id="input-2"
                    v-model="form.Password"
                    type="password"
                    placeholder="Enter password"
                    required
                ></b-form-input>
            </b-form-group>

            <b-form-group id="input-group-4">
                <b-form-checkbox v-model="saveCred">Remember me</b-form-checkbox>
            </b-form-group>

            <b-button type="submit" variant="primary">Submit</b-button>
            <b-button type="reset" variant="danger">Reset</b-button>
        </b-form>
    </div>
</template>

<script>
    import axios from 'axios';
    export default {
        data() {
            return {
                authUrl: "https://localhost:6868/Users/authenticate",
                form: {},
                saveCred: false,
                show: true,
            }
        },
        methods: {
            /**
             * Shortcut for saving cookies
             * @param authToken JwtToken for user authentication
             * @param userId User ID
             * @param expire Cookies expire time, delete when browser is closed -> 0
             */
            saveCookie(authToken, userId, expire) {
                this.$cookies.set('sn-auth-token', authToken, expire);
                this.$cookies.set('sn-user-id', userId, expire);
            },

            onSubmit(event) {
                event.preventDefault();

                axios.post(
                    this.authUrl,
                    JSON.parse(JSON.stringify(this.form))
                ).then((res) => {
                    var authToken = res.data.JwtToken;
                    var userId = res.data.Id;

                    if (this.saveCred) {
                        this.saveCookie(authToken, userId, -1);
                    } else {
                        this.saveCookie(authToken, userId, 0);
                    }
                    this.$nextTick(function () {
                        this.$router.push({
                            name: 'home',
                            params: {
                                authToken: authToken,
                                userId: userId,
                            }
                        });
                    });
                }).catch((res) => {
                    console.log(res)
                });
            },

            onReset(event) {
                event.preventDefault()
                // Reset our form values
                this.form = {};
                // Trick to reset/clear native browser form validation state
                this.show = false;
                this.$nextTick(() => {
                    this.show = true
                })
            }
        }
    }
</script>

<style scoped>
</style>