<template>
    <div class="chat__body" id="chat__body">
        <chat-message
            v-for="(msg, index) in msgs"
            :key="msg.Id"
            :msg="msg"
            :prev="[index == 0 ? null : msgs[index - 1]]"
            :meId="meId"/>
  </div>
</template>

<script>
    export default {
        props: ["msgs", "meId"],
    };
</script>

<style scoped>
    .chat__body {
        padding: 2rem;
        overflow: scroll;
        scroll-behavior: smooth;
    }

    .chat__body::-webkit-scrollbar {
        display: none;
    }
</style>