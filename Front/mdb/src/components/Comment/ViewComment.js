import React from 'react';
import style from '../Post/Post.module.css'
import CommentCard from './CommentCard.js'


import {
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBCardHeader,
  MDBCardFooter,
  MDBBtn
} from 'mdb-react-ui-kit';



  async function PostComment()
  {
      const UserID = localStorage.getItem("userId");
      const PostID = localStorage.getItem("postId");
      const TextComment = document.getElementById("textAreaExample").value;
      const token = localStorage.getItem("Token");
    

      const response = await fetch('https://localhost:44368/Comment/AddComment/'+ UserID + '/' + PostID + '/' + TextComment,
          {
              method: 'POST',
              headers: {
                  'Content-type': 'application/json;charset=UTF-8',
                  'Authorization': `Bearer ${token}`
              }
          });
      console.log(response);
      window.location.reload(false);
  };




export default function App() {
  const community = localStorage.getItem("CommunityName");
  const title = localStorage.getItem("title");
  const text = localStorage.getItem("text");
  const date = localStorage.getItem("date");

  //const
  return (
    <div className={style.divMain}>
    <MDBCard alignment='center'>
      <MDBCardHeader>{community}</MDBCardHeader>
      <MDBCardBody>
        <MDBCardTitle>{title}</MDBCardTitle>
        <MDBCardText>{text}</MDBCardText>
        {/* <div className={style.Buttons}>
            <div className={style.Comment}> <i class="far fa-comment-alt" ></i></div>
            <div className={style.Like}><i class="fas fa-thumbs-up" ></i></div>
            <div className={style.Dislike}><i class="fas fa-thumbs-down" ></i></div>
        </div> */}
        {/* <MDBBtn href='#'>Button</MDBBtn> */}
        <div class="form-outline" >
        <textarea class="form-control" id="textAreaExample" rows="4" placeholder="text"></textarea>
        <label class="form-label" for="textAreaExample" className={style.mess} onClick={() => PostComment()} >Input comment</label>
        </div>
      </MDBCardBody>
      <MDBCardFooter className='text-muted'>{date}</MDBCardFooter>
    </MDBCard>
    <CommentCard/>
    </div>

    
 



  );
}