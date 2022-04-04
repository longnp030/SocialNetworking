<template>
    <b-container fluid id="personal-information" v-if="profile">
        <b-row class="my-4" id="edit-profile">
            <b-button v-if="!edit" variant="outline-danger" @click="edit = !edit">Edit</b-button>
            <b-button v-if="edit" variant="outline-secondary" @click="edit = !edit">Cancel</b-button>
            <b-button v-if="edit" variant="outline-success" @click="saveChanges">Save</b-button>
        </b-row>

        <b-row class="my-4">
            <b-col sm="3"><label for="Name">Name</label></b-col>
            <b-col sm="9">
                <b-form-input 
                    id="Name" 
                    type="text" 
                    v-model="profile.Name"
                    :class="{'edit': edit}"
                    :readonly="!edit"></b-form-input>
            </b-col>
        </b-row>
        
        <b-row class="my-4">
            <b-col sm="3"><label for="Address">Address</label></b-col>
            <b-col sm="9">
                <b-form-input 
                    id="Address" 
                    type="text" 
                    :class="{'edit': edit}"
                    v-model="profile.CurrentLocation"
                    :readonly="!edit"></b-form-input>
            </b-col>
        </b-row>
        
        <b-row class="my-4">
            <b-col sm="3"><label for="SelfIntro">Self introduction</label></b-col>
            <b-col sm="9">
                <b-form-input 
                    id="SelfIntro" 
                    type="text" 
                    :class="{'edit': edit}"
                    v-model="profile.SelfIntroduction"
                    :readonly="!edit"></b-form-input>
            </b-col>
        </b-row>

        <b-row class="my-4">
            <b-col sm="3"><label for="DateOfBirth">Date of birth</label></b-col>
            <b-col sm="9">
                <b-form-datepicker 
                    id="DateOfBirth" 
                    v-model="profile.DateOfBirth" 
                    class="mb-2"
                    :readonly="!edit"></b-form-datepicker>
            </b-col>
        </b-row>
        
        <b-row class="my-4">
            <b-col sm="3"><label for="Gender">Gender</label></b-col>
            <b-col sm="9">
                <b-form-radio-group
                    id="Gender"
                    v-model="profile.Gender"
                    :options="options"
                    name="DateOfBirth"
                    :disabled="!edit"></b-form-radio-group>
            </b-col>
        </b-row>

        {{profile}}
  </b-container>
</template>

<script>
    export default {
        props: ["userId"],
        data() {
            return {
                getProfileUrl: "https://localhost:6868/Users/userId/profile",
                profile: null,
                options: [
                    { text: 'Male', value: 0 },
                    { text: 'Female', value: 1 },
                    { text: 'Prefer not to share', value: 2 }
                ],
                edit: false,
            }
        },
        async mounted() {
            await this.getProfile();
        },
        methods: {
            async getProfile() {
                await this.$http.get(
                    this.getProfileUrl.replace("userId", this.userId)
                ).then(res => {
                    this.profile = res.data;
                    //console.log(this.userProfile);
                }).catch(err => {
                    console.log(err.response);
                });
            },

            async saveChanges() {
                await this.$http.patch(
                    this.getProfileUrl.replace("userId", this.userId),
                    this.profile
                ).then(res => {
                    console.log(res);
                    this.$router.go();
                }).catch(err => {
                    console.log(err.response);
                });
            }
        }
    }
</script>

<style scoped>
    #edit-profile{
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-around;
    }

    #personal-information {
        padding: 0 200px;
    }

    #personal-information,
    #personal-information input {
        background-color: #111;
        color: var(--white);
        border-color: #111;
    }
    #personal-information input.edit {
        border-color: var(--white);
    }

    #personal-information label {
        margin-top: 8px;
    }
</style>