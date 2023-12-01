import React, { isValidElement } from 'react'
import { useState ,useEffect} from "react";
import style from '../Post/Post.module.css'
import CardForComment from './CardForComment.js';

const CommentCard =  () =>
{ 

    const [allComments, setAllComments] = useState([]);
    useEffect(() => {
        async function fetchGetComment()
        {
            const userId = localStorage.getItem("userId");
            const postId = localStorage.getItem("postId");
            const token = localStorage.getItem('Token');
            const response = await fetch('https://localhost:44368/Comment/GetAllComments/' + postId,
                {
                    method: 'GET',
                    headers: {
                        'Content-type': 'application/json;charset=UTF-8',
                        'Authorization': `Bearer ${token}`
                    }
                });
    
            const data = await response.json();
            console.log(data);
            const comments= data.map((comment)=>{
                return{
                    ID: comment.id,
                    Text: comment.text,
                    IsCommentAutor: comment.userId == userId,
                    Likes: comment.userWhoLikedComment.length,
                    Dislikes: comment.userWhoDislikedComment.length,
                    IsCommentDisliked: comment.userWhoDislikedComment.some(p => p == userId),
                    IsCommentLiked: comment.userWhoLikedComment.some(p => p == userId)
                };
            });
            setAllComments(comments);
            console.log(comments);
        };
        fetchGetComment();
    },[]);  
    console.log(allComments);


    async function DeleteComment(commentId)
    {
       
        const token = localStorage.getItem('Token');
        const userId = localStorage.getItem('userId');
        

        const response = await fetch('https://localhost:44368/Comment/DeleteComment/'+ userId + '/' + commentId,
        {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`

            }
        })
        console.log(response);
    };

    async function LikeComment(commentId)
    {
        const token = localStorage.getItem('Token');
        const userId = localStorage.getItem('userId');

        const response = await fetch('https://localhost:44368/LikeComment/AddLikeComment/' + userId + '/' + commentId,
        {
            method: 'POST',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`
            }
        });
        
        console.log(response);
    }

    async function DislikeComment(commentId)
    {
        const token = localStorage.getItem('Token');
        const userId = localStorage.getItem('userId');

        const response = await fetch('https://localhost:44368/DislikeCommentContoller/AddDislikeComment/' + userId + '/' + commentId,
        {
            method: 'POST',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`
            }
        });
        
        console.log(response);
    }

    async function RemoveLikeComment(commentId)
    {
        const token = localStorage.getItem('Token');
        const userId = localStorage.getItem('userId');

        const response = await fetch('https://localhost:44368/LikeComment/RemoveLikeComment/' + userId + '/' + commentId,
        {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`
            }
        });
        
        console.log(response);
    }

    async function RemoveDislikeComment(commentId)
    {
        const token = localStorage.getItem('Token');
        const userId = localStorage.getItem('userId');

        const response = await fetch('https://localhost:44368/DislikeCommentContoller/RemoveDislikeComment/' + userId + '/' + commentId,
        {
            method: 'DELETE',
            headers: {
                'Content-type': 'application/json;charset=UTF-8',
                'Authorization': `Bearer ${token}`
            }
        });
        
        console.log(response);
    }

    const [isCommentLiked, setIsCommnetLiked] = useState(false);
    const [isCommentDisliked, setIsCommnetDisliked] = useState(false);

    return(
    <div>
        { allComments.map((comment) =>(
            <CardForComment data = { comment } />
        ))}
    </div>
    )
};
export default CommentCard;