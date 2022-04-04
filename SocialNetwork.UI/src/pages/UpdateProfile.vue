<template>
    <div>
        <h1>Update profile</h1>
        <b-form @submit="onSubmit">
            <b-form-group>
                <b-form-input
                    id="input-name"
                    v-model="form.Name"
                    placeholder="Name"
                    required
                ></b-form-input>
            </b-form-group>

            <b-form-group>
                <b-form-datepicker id="input-dob" v-model="form.DateOfBirth" class="mb-2"></b-form-datepicker>
            </b-form-group>

            <b-form-group>
                <b-form-textarea
                    id="textarea"
                    v-model="form.SelfIntroduction"
                    placeholder="About yourself..."
                    rows="3"
                    max-rows="6"
                ></b-form-textarea>
            </b-form-group>

            <b-form-group>
                <b-form-radio-group
                    v-model="form.Gender"
                    :options="options"
                    class="mb-3"
                    value-field="value"
                    text-field="name"
                ></b-form-radio-group>
            </b-form-group>

            <b-form-group>
                <b-form-input
                    id="input-location"
                    v-model="form.CurrentLocation"
                    placeholder="Address"
                ></b-form-input>
            </b-form-group>

            <b-button type="submit" variant="primary">Submit</b-button>
        </b-form>
        <b-card class="mt-3" header="Form Data Result">
            <pre class="m-0">{{ form }}</pre>
        </b-card>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                profileUrl: "https://localhost:6868/Users/userId/profile",
                form: {},
                options: [
                    { value: 0, name: 'Male' },
                    { value: 1, name: 'Female' },
                    { value: null, name: 'Prefer not to share'},
                ],
                jwtToken: null,
                myId: null,
            }
        },
        created() {
            this.originalform = this.$route.params.userProfile;
            this.form = this.$route.params.userProfile;
            this.myId = this.$route.params.myId;
            this.jwtToken = this.$route.params.jwtToken;
            this.$http.defaults.headers.common["Authorization"] = this.jwtToken;
        },
        methods: {
            async onSubmit(event) {
                event.preventDefault();
                this.form.Timestamp = new Date();
                await this.$http.patch(
                    this.profileUrl.replace("userId", this.myId),
                    JSON.parse(JSON.stringify(this.form)),
                ).then((res) => {
                    this.$router.push({
                        name: 'home',
                        params: {
                            jwtToken: this.jwtToken,
                            myId: this.myId,
                        }
                    });
                }).catch((res) => {
                    console.log(res.response);
                })
            },
        }
    }
</script>

<style scoped>
</style>