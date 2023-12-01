import React from 'react';
import SideBar from "./SideBar/SideBar.js";
import Header from "./Header/Header.js";
import ViewAllPost from './Post/ViewAllPost.js';
import { useState ,useEffect} from "react";
import style from './MixPage.module.css';

async function fetchPostsHandler(userID, token, setAllPosts)
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

        let arrayOfPost = [];

        for(var key in data)
        {
            var dataForMapping = data[key];
            const transformedData = dataForMapping.map((post) => {
                return {
                        ID: post.id,
                        Title: post.title,
                        Text: post.text,
                        Likes: post.likes,
                        Dislikes: post.dislikes,
                        Date: post.date,
                        Community: post.communityName,
                        CommentsCount: post.commentsCount,
                        IsLiked: post.usersWhoLikedThisPost.some(p => p == userID),
                        IsDisliked: post.usersWhoDislikedThisPost.some(p => p == userID)
                    };
                });
            transformedData.map(post => {
                arrayOfPost.push(post);
            })
        }
        console.log(arrayOfPost);
        setAllPosts(arrayOfPost);
    }

    const fetchData = async (userID, setCommunities) =>
    {
        const response = await fetch("https://localhost:44368/User/GetFollowCommunity/"+userID,
        {
            method: 'GET',
            headers:{'Content-type' : 'application/json; charset=UTF-8'}
        });

        const data = await response.json();
        console.log(data);
        setCommunities(data);
    };

export default function MixPage(props){
    const [allPosts, setAllPosts] = useState([]);
    const [communities,setCommunities] = useState([]);

    const token = localStorage.getItem("Token");
    const userID = localStorage.getItem("ID");
    
    useEffect(()=>{
        fetchPostsHandler(userID, token, setAllPosts);
        fetchData(userID, setCommunities);
    },[]);

    return (
        <div>
            <Header />
            <div className={style.divStyle}>
                <ViewAllPost data={allPosts}/>
                <SideBar data={communities}/>
            </div>
        </div>
        
    );
}