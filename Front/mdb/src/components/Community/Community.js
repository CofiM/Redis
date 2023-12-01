import React from 'react';
import Header from "./HeaderCommunity.js"
import ViewAllPost from '../Post/ViewAllPost.js';
import { useState ,useEffect} from "react";
import AddPostSection from '../Post/AddPostSection.js';
import style from "./HeaderCommunity.module.css";


export default function Community(props){
    const [allPosts, setAllPosts] = useState([]);

    const token = localStorage.getItem("Token");
    const userID = localStorage.getItem("ID");

    const communityID = localStorage.getItem("CommunityID");
    console.log(communityID);
    const communityName = localStorage.getItem("CommunityName");
    console.log("NAME: " + communityName);
    async function fetchPostsHandler()
    {
        const response = await fetch("https://localhost:44368/Post/GetPostsForCommunity/"+ communityID + "/" + userID,
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
                CommentsCount: post.post.commentsCount,
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
            <Header name = {communityName} />
            <div className={style.centralItem}>
                <AddPostSection userId={userID} communityId={communityID}/>
                <ViewAllPost data={allPosts}/>
            </div>
        </div>
        
    );

};