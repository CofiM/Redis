import React from 'react';
import { useState ,useEffect} from "react";
import Header from "./HeaderTopic.js";
import ViewAllPost from '../Post/ViewAllPost.js';

export default function Community(props){
    const [allPosts, setAllPosts] = useState([]);

    const token = localStorage.getItem("Token");
    const topic = localStorage.getItem("Topic");
    const userID = localStorage.getItem("ID");
    

    async function fetchPostsHandler()
    {
        const response = await fetch("https://localhost:44368/Topic/GetAllPostForTopic/"+ topic + "/" + userID,
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
                ID: post.post.id,
                Title: post.post.title,
                Text: post.post.text,
                Likes: post.post.likes,
                Dislikes: post.post.dislikes,
                Date: post.post.date,
                Community: post.post.community,
                Comments: post.post.comments,
                IsPostLiked: post.isPostLiked,
                IsPostDisliked: post.isPostDisliked
            };
        });
        setAllPosts(transformedData);
    }

    useEffect(() => {
        fetchPostsHandler();
    },[])

    return (
        <div>
           <Header name = {topic} />
            <div>
                <ViewAllPost data={allPosts}/>
            </div> 
        </div>
        
    );

};