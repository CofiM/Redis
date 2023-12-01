import React from "react";
import { useState ,useEffect} from "react";
import PostCard from './PostCard.js';


const ViewAllPost = (props) =>
{
    const token = localStorage.getItem("Token");
    const userID = localStorage.getItem("ID");
    
    const [allPosts, setAllPosts] = useState([]);

    /* useEffect(() => {
        async function fetchPostsHandler()
        {
            const response = await fetch("https://localhost:44368/Post/GetPostsForUser/"+ userID,
            {
                method: 'GET',
                headers: {
                    'Content-type': 'application/json;charset=UTF-8',
                    'Authorization': `Bearer ${token}`
                }
            });
            const data = await response.json();

            console.log(data);

            const transformedData = data.map((post) => {
                return {
                  ID: post.id,
                  Title: post.title,
                  Text: post.text,
                  Likes: post.likes,
                  Dislikes: post.dislikes,
                  Date: post.date,
                  Community: post.community,
                  Comments: post.comments
                };
              });
              setAllPosts(transformedData);

        };
        fetchPostsHandler();

    },[allPosts]);
    console.log("transformed data: "); 
    console.log(allPosts); */
    console.log(props.data);
    
    return(
        <div>
        { props.data.map((post) => (
            <PostCard
                
                ID = { post.ID }
                title ={post.Title}
                text = {post.Text}
                likes = {post.Likes}
                dislikes = {post.Dislikes}
                date = {post.Date}
                community = {post.Community}
                commentsCount = {post.CommentsCount}
                IsLiked = {post.IsLiked}
                IsDisliked = {post.IsDisliked}
            />
        ))
        }
        </div>
    )

}
export default ViewAllPost;