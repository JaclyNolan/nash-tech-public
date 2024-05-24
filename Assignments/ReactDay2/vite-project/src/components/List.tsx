import { FC, useEffect, useState } from "react";
import PostComponent, { Post } from './Posts';

const ListComponent: FC = () => {
    const [posts, setPosts] = useState<Post[]>([]);
    const fetchPosts = async(): Promise<void> => {
        const response = await fetch("https://jsonplaceholder.typicode.com/posts")
        if (!response.ok) {
            throw new Error(response.status.toString())
        }
        const data: Post[] = await response.json()
        setPosts(data);
    }

    useEffect(() => {
        fetchPosts();
    }, [])
    return (
        <div>
            {(posts).map((post) => {
                return <PostComponent key={post.id} post={post}></PostComponent>
            })}
        </div>
    )
}

export default ListComponent;