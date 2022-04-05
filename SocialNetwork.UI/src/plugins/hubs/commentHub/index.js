import {
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import Vue from 'vue';

export default {
    install() {
        // init connection to backend hub ('commentsocket')
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:6868/commentsocket')
            .configureLogging(LogLevel.Information)
            .build();

        // init vue shortcut
        const commentHub = new Vue();
        Vue.prototype.$commentHub = commentHub;

        // handle users like comment
        connection.on('Reaction', (commentId, userId, like) => {
            commentHub.$emit('comment-react', { commentId, userId, like })
        });

        // handle users reply to comment
        connection.on('Reply', (replyId) => {
            commentHub.$emit('comment-replied-to', replyId)
        });

        // handle users edit a reply
        connection.on('Edit', (reply) => {
            commentHub.$emit('reply-edited', reply)
        });

        // handle users edit a reply
        connection.on('Delete', (replyId) => {
            commentHub.$emit('reply-deleted', replyId)
        });

        // handle tracking users view comment
        commentHub.commentOpened = commentId => {
            return startedPromise
                .then(() => connection.invoke('ViewingComment', commentId))
                .catch(console.error);
        }
        commentHub.commentClosed = commentId => {
            return startedPromise
                .then(() => connection.invoke('LeavingComment', commentId))
                .catch(console.error);
        }

        // attempt to reconnect if fails
        let startedPromise = null
        function start() {
            startedPromise = connection.start().catch(err => {
                console.error('Failed to connect with hub', err)
                return new Promise((resolve, reject) =>
                    setTimeout(() => start().then(resolve).catch(reject), 5000))
            })
            return startedPromise
        }
        connection.onclose(() => start());

        start();
    }
};