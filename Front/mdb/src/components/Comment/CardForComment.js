import React, { isValidElement } from 'react'
import { useState ,useEffect} from "react";
import style from '../Post/Post.module.css'


async function addLike(commentId, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)
{
  
  console.log("Kliknuo sam lajk");

  const token = localStorage.getItem('Token');
  const idUser = localStorage.getItem('ID');
  console.log(token);

  if (!likedComment)
  {
    const response = await fetch('https://localhost:44368/LikeComment/AddLikeComment/' + idUser + '/' + commentId,
    {
      method : 'POST',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
        if (x.ok)
        {
          setLikes(likes + 1)
        }
      });
  }

  if (likedComment)
  {
    const response = await fetch('https://localhost:44368/LikeComment/RemoveLikeComment/'+ idUser +'/' + commentId ,
    {
      method : 'DELETE',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        setLikes(likes - 1)
      }
    });
  }

  if (dislikedComment)
  {
    setDislikes(dislikes - 1);
    setDislikedComment(!dislikedComment);
  }

  setLikedComment(!likedComment);
}

async function addDislike(commentId, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)
{
  const token = localStorage.getItem('Token');
  const idUser = localStorage.getItem('ID');

  if (!dislikedComment)
  {
    const response = await fetch('https://localhost:44368/DislikeCommentContoller/AddDislikeComment/'+ idUser +'/' + commentId ,
    {
      method : 'POST',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        setDislikes(dislikes + 1)
      }
    });
  }
  if (dislikedComment)
  {
    const response = await fetch('https://localhost:44368/DislikeCommentContoller/RemoveDislikeComment/'+ idUser +'/' + commentId ,
    {
      method : 'DELETE',
      headers: {
        'Content-type': 'application/json;charset=UTF-8',
        'Authorization': `Bearer ${token}`
      }
    }).then(x => {
      if (x.ok)
      {
        setDislikes(dislikes - 1)
      }
    });
  }

  if (likedComment)
  {
    setLikes(likes - 1);
    setLikedComment(!likedComment);
  }

  setDislikedComment(!dislikedComment);

}

async function DeleteComment(commentId, setShownComm)
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
    setShownComm(false);
};

  


 
export default function App(props) {
  const [path, setPath] = useState('');
  const [callNavigate, setCallNavigate] = useState(false);

  const [likes, setLikes] = useState(props.data.Likes);
  const [dislikes, setDislikes] = useState(props.data.Dislikes);
  const [shownComm, setShownComm] = useState(true);
  const [showDeleteCom, setShowDeleteCom] = useState(props.data.IsCommentAutor);

  const [likedComment, setLikedComment] = useState(props.data.IsCommentLiked);
  const [dislikedComment, setDislikedComment] = useState(props.data.IsCommentDisliked);

  console.log(props);

  return (
    <div className={style.divMain}>
      {shownComm &&
        <div className={props.data.ID}>
          <div className="card-body">
              <h5 className="card-title">{props.data.Username}</h5>
              <p className="card-text">{props.data.Text}</p>
              <div className={style.Buttons}>
                  { likedComment ?  <div className={style.Like} > <i  className="fas fa-thumbs-up" onClick={() => addLike(props.data.ID, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)} ></i><label> {likes}</label></div> 
                    :
                  <div className={style.Like}><i className="far fa-thumbs-up" onClick={() => addLike(props.data.ID, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)} ></i><label> {likes}</label> </div>}

                  { dislikedComment ?   <div className={style.Dislike}><i className="fas fa-thumbs-down" onClick={() => addDislike(props.data.ID, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)}></i><label> {dislikes}</label></div>
                  :
                  <div className={style.Dislike}><i className="far fa-thumbs-down" onClick={() => addDislike(props.data.ID, setLikes, setDislikes, likedComment, setLikedComment, dislikedComment, setDislikedComment, likes, dislikes)}></i><label> {dislikes}</label></div>} 
                  {showDeleteCom ? <div className={style.Delete}><i className="fas fa-trash" onClick = {() => DeleteComment(props.data.ID, setShownComm)} ></i> </div> : <div></div> }
              </div>
          </div>
        </div>
      }
    </div>
  );
}