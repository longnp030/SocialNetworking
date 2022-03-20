<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-avatar variant="info" src="https://placekitten.com/300/300" size="4rem"></b-avatar>
            </template>

            <b-form @submit="onSubmit">
                <b-form-textarea
                    id="textarea"
                    v-model="form.Text"
                    placeholder="Enter something..."
                    rows="3"
                    max-rows="6"
                ></b-form-textarea>

                <div id="media-privacy-submit">
                    <b-form-select v-model="form.Privacy" :options="privacyOptions" required></b-form-select>
                    <b-form-file
                        v-model="media"
                        accept="image/*"
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
        name: 'PostForm',
        props: ["userId", "jwtToken"],
        data() {
            return {
                postUrl: "https://localhost:6868/Posts/",
                uploadUrl: "https://localhost:6868/Posts/upload",
                unuploadUrl: "https://localhost:6868/Posts/unupload",
                form: {
                    MediaPaths: [],
                },
                media: null,
                imgUrls: [],
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

                this.$http.post(
                    this.postUrl,
                    this.form
                ).then((res) => {
                    console.log(res);
                    this.$router.go();
                }).catch((res) => {
                    console.log(res.response)
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
                console.log(e.target.src);
                var unloadForm = new FormData();
                unloadForm.append(e.target.src.replace("blob:http://localhost:8080/", ""), "id");
                this.$http.post(
                    this.unuploadUrl,
                    unloadForm,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(res => {
                    console.log(res);
                    this.imgUrls.splice(this.imgUrls.indexOf(e.target.src), 1);
                }).catch(res => {
                    console.log(res.response);
                });
                e.preventDefault();
            }
        },
    }
</script>

<style scoped>
    .card, .card-body {
        background-color: #111;
        color: var(--white);
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

    #media-privacy-submit {
        display: flex;
        align-items: center;
        justify-content: space-around;
        gap: 20px;
        margin: 5px 0;
    }
    .custom-select {
        margin-top: 0;
        background-color: #111;
        color: var(--white);
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