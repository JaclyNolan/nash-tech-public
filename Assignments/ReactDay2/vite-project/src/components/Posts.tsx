import { FC } from "react";

export interface Post {
    userId: number,
    id: number,
    title: string,
    body: string
}

interface Props {
    post: Post
}
const PostComponent: FC<Props> = ({post}) => {
    return (
        <div>
            <span>User Id: {post.userId}</span><br/>
            <span>Post Id: {post.id}</span><br/>
            <span>Title: {post.title}</span><br/>
            <p>Body: {post.body}</p>
        </div>
    )
}

export default PostComponent;