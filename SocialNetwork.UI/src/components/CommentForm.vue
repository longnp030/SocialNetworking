<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
            </template>

            <b-form @submit="onSubmit" ref="form">
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
        </b-media>
    </b-card>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'CommentForm',
        props: ["postId", "userId", "jwtToken"],
        data() {
            return {
                commentUrl: "https://localhost:6868/Comments/",
                form: {
                    MediaPaths: [],
                },
                media: null,
            }
        },
        methods: {
            async onSubmit(event) {
                event.preventDefault();
                this.form.AuthorId = this.userId;
                this.form.PostId = this.postId,

                await axios.post(
                    this.commentUrl,
                    JSON.parse(JSON.stringify(this.form)),
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                    this.$refs.form.reset();
                }).catch((res) => {
                    console.log(res.response);
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