<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-avatar variant="info" src="https://placekitten.com/300/300" size="4rem"></b-avatar>
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
    export default {
        name: 'CommentForm',
        props: ["postId", "userId", "jwtToken"],
        data() {
            return {
                commentUrl: "https://localhost:6868/Comments/",
                uploadUrl: "https://localhost:6868/Posts/upload",
                unuploadUrl: "https://localhost:6868/Posts/unupload",
                form: {
                    MediaPaths: [],
                },
                media: null,
                imgUrls: [],
            }
        },
        methods: {
            async onSubmit(event) {
                event.preventDefault();
                this.form.AuthorId = this.userId;
                this.form.PostId = this.postId,

                await this.$http.post(
                    this.commentUrl,
                    JSON.parse(JSON.stringify(this.form))
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

                var imgForm = new FormData();
                imgForm.append('image', file);
                this.$http.post(
                    this.uploadUrl,
                    imgForm,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(res => {
                    console.log(res.data);
                    this.form.MediaPaths.push(res.data);
                }).catch(res => {
                    console.log(res.response);
                })
            },
            removeImg(e) {
                this.imgUrls.splice(this.imgUrls.indexOf(e.target.src), 1);
                e.preventDefault();
            }
        },
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        border-radius: 10px;
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