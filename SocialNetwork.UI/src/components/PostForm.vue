<template>
    <b-card>
    <b-media>
        <template #aside>
            <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
        </template>

        <b-form @submit="onSubmit">
            <b-form-group>
                <b-form-select v-model="form.Privacy" :options="privacyOptions" size="sm" class="mt-3"></b-form-select>
            </b-form-group>

            <b-form-group>
                <b-form-textarea
                    id="textarea"
                    v-model="form.Text"
                    placeholder="Enter something..."
                    rows="3"
                    max-rows="6"
                ></b-form-textarea>
            </b-form-group>

            <b-form-group>
                  <b-form-file
                        v-model="media"
                        placeholder="Add an image..."
                  ></b-form-file>
            </b-form-group>

            <b-button type="submit" variant="primary">Submit</b-button>
        </b-form>
    </b-media></b-card>
</template>

<script>
    import axios from 'axios';

    export default {
        props: ["userId", "jwtToken"],
        data() {
            return {
                postUrl: "https://localhost:6868/Posts/",
                form: {
                    MediaPaths: [],
                },
                media: null,
                privacyOptions: [
                    { value: 0, text: 'Public' },
                    { value: 1, text: 'Private' }
                ]
            }
        },
        methods: {
            onSubmit(event) {
                event.preventDefault();
                this.form.AuthorId = this.userId;

                axios.post(
                    this.postUrl,
                    JSON.parse(JSON.stringify(this.form)),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res)
                });
            }
        },
        watch: {
            media() {
                if (this.media !== null) {
                    console.log(this.media);
                    this.form.MediaPaths.push(this.media);
                    this.$nextTick(() => {
                        this.media = null;
                    });
                }
            }
        }
    }
</script>

<style scoped>
</style>