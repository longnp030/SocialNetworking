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

                <div id="media-submit">
                    <b-form-file
                        v-model="media"
                        placeholder="Add an image..."
                        @change="addImg"
                        multiple
                    ></b-form-file>

                    <b-button type="submit" variant="primary">Submit</b-button>
                </div>

                <div id="preview">
                    <img v-for="url in imgUrls" :key="url" :src="url" @contextmenu="removeImg"/>
                </div>
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
                imgUrls: [],
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
            },

            addImg(e) {
                const file = e.target.files[0];
                this.imgUrls.push(URL.createObjectURL(file));
            },
            removeImg(e) {
                this.imgUrls.splice(this.imgUrls.indexOf(e.target.src), 1);
                e.preventDefault();
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
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
    }

    .card-body .media .media-aside img {
        border-radius: 64px;
    }

    #textarea {
        background-color: #111;
        border: none;
        color: var(--white);
        overflow: hidden;
    }
    #textarea::-webkit-scrollbar {
        display: none;
    }

    #media-submit {
        display: flex;
        align-items: center;
        justify-content: space-around;
        gap: 20px;
        margin: 5px 0;
    }

    #preview {
        display: flex;
        justify-content: center;
        align-items: center;
        max-width: 500px;
        overflow-x: auto;
    }
    #preview img {
        width: 200px;
        height: 200px;
        object-fit: contain;
    }
</style>