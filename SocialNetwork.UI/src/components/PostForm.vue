<template>
    <b-card>
        <b-media>
            <template #aside>
                <b-img blank blank-color="#ccc" width="64" alt="placeholder"></b-img>
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
                <b-card class="mt-3" header="Form Data Result">
                <pre class="m-0">{{ form }}</pre>
                </b-card>
    </b-card>
</template>

<script>
    import axios from 'axios';

    export default {
        name: 'PostForm',
        props: ["userId", "jwtToken"],
        data() {
            return {
                postUrl: "https://localhost:6868/Posts/",
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

                axios.post(
                    this.postUrl,
                    //JSON.parse(JSON.stringify(this.form)),
                    this.form,
                    {
                        headers: {
                            Authorization: `Bearer ${this.jwtToken}`
                        }
                    }
                ).then((res) => {
                    console.log(res);
                }).catch((res) => {
                    console.log(res.response)
                });
            },

            addImg(e) {
                const file = e.target.files[0];
                this.imgUrls.push(URL.createObjectURL(file));

                this.form.MediaPaths.push(file);
            },
            removeImg(e) {
                this.imgUrls.splice(this.imgUrls.indexOf(e.target.src), 1);
                e.preventDefault();
            }
        },
        watch: {
            //media() {
            //    if (this.media !== null) {
            //        console.log(this.media);
            //        this.form.MediaPaths.push({ Media: this.media, Name: this.media.name });
            //        this.$nextTick(() => {
            //            this.media = null;
            //        });
            //    }
            //}
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