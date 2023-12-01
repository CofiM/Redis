import React, { useEffect ,useState} from 'react';
import { Navigate } from "react-router-dom";
import style from './Post.module.css'
import {
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBCardHeader,
  MDBCardFooter,
  MDBBtn
} from 'mdb-react-ui-kit';

async function addLike(idPost, setLikes, setDislikes, likedPost, setLikedPost, dislikedPost, setDislikedPost)
{
  
  console.log("Kliknuo sam lajk");

  const token = localStorage.getItem('Token');
  const idUser = localStorage.getItem('ID');
  console.log(token);

  if (!likedPost)
  {
    const response = await fetch('https://localhost:44368/AddLikePost/'+ idUser +'/' + idPost ,
    {
      method : 'POST',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        x.json()
          .then(obj => {
            setLikes(obj.likes);
            setDislikes(obj.dislikes);
          })
      }
    });
  }

  if (likedPost)
  {
    const response = await fetch('https://localhost:44368/DeleteLikePost/'+ idUser +'/' + idPost ,
    {
      method : 'DELETE',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        x.json()
          .then(obj => {
            setLikes(obj);
          })
      }
    });
  }

  if (dislikedPost)
  {
    setDislikedPost(!dislikedPost);
  }

  setLikedPost(!likedPost);
}

async function addDislike(idPost, setLikes, setDislikes, dislikedPost, setDislikedPost, likedPost, setLikedPost)
{
  const token = localStorage.getItem('Token');
  const idUser = localStorage.getItem('ID');

  if (!dislikedPost)
  {
    const response = await fetch('https://localhost:44368/AddDislikePost/'+ idUser +'/' + idPost ,
    {
      method : 'POST',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        x.json()
          .then(obj => {
            setLikes(obj.likes);
            setDislikes(obj.dislikes);
          })
      }
    });
  }
  if (dislikedPost)
  {
    const response = await fetch('https://localhost:44368/DeleteDislikePost/'+ idUser +'/' + idPost ,
    {
      method : 'DELETE',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        x.json()
          .then(obj => {
            setDislikes(obj);
          })
      }
    });
  }

  if (likedPost)
  {
    setLikedPost(!likedPost);
  }

  setDislikedPost(!dislikedPost);

}

  


 
export default function App(props) {
  const [path, setPath] = useState('');
  const [callNavigate, setCallNavigate] = useState(false);

  const [likes, setLikes] = useState(props.likes);
  const [dislikes, setDislikes] = useState(props.dislikes);

  const [likedPost, setLikedPost] = useState(props.IsLiked);
  const [dislikedPost, setDislikedPost] = useState(props.IsDisliked);

  async function openComments(idPost)
  {
    localStorage.setItem("postId", idPost);
    const idUser = localStorage.getItem("ID");
    localStorage.setItem("userId", idUser)
    setPath("/ViewComment");
    setCallNavigate(true);
  }
  
  localStorage.setItem("title", props.title);
  localStorage.setItem("text", props.text);
  localStorage.setItem("date", props.date);
  console.log(props);

  return (
    <div className={style.divMain}>
    <MDBCard alignment='center'>
      <MDBCardHeader>{props.community}</MDBCardHeader>
      <MDBCardBody>
        <MDBCardTitle>{props.title}</MDBCardTitle>
        <MDBCardText>{props.text}</MDBCardText>
        <div className={style.Buttons}>
            <div className={style.Comment}> <i class="far fa-comment-alt" onClick={() => openComments(props.ID)} ></i><label>{props.commentsCount}</label></div>
            {likedPost && <div className={style.Like}><i class="fas fa-thumbs-up" onClick={() => addLike(props.ID, setLikes, setDislikes, likedPost, setLikedPost, dislikedPost, setDislikedPost)}></i><label>{likes}</label></div>}
            {!likedPost &&<div className={style.Like}><i class="far fa-thumbs-up" onClick={() => addLike(props.ID, setLikes, setDislikes, likedPost, setLikedPost, dislikedPost, setDislikedPost)}></i><label>{likes}</label></div>}
            {dislikedPost && <div className={style.Dislike}><i class="fas fa-thumbs-down" onClick={() => addDislike(props.ID, setLikes, setDislikes, dislikedPost, setDislikedPost, likedPost, setLikedPost)}></i><label>{dislikes}</label></div>}
            {!dislikedPost && <div className={style.Dislike}><i class="far fa-thumbs-down" onClick={() => addDislike(props.ID, setLikes, setDislikes, dislikedPost, setDislikedPost, likedPost, setLikedPost)}></i><label>{dislikes}</label></div>}
            {callNavigate && <Navigate to={path} variant="body2"/>}
        </div>
      </MDBCardBody>
      <MDBCardFooter className='text-muted'>{props.date}</MDBCardFooter>
    </MDBCard>
    </div>
  );
}