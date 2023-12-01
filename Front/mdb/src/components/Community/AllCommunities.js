import React from 'react';
import {useState, useEffect} from 'react';
import { Navigate } from "react-router-dom";
import { 
    MDBCard, 
    MDBCardBody,
    MDBBtn
} from 'mdb-react-ui-kit';
import style from "./AllCommunities.module.css";




export default function App(props) {
    const [path, setPath] = useState('');
    const [callNavigate, setCallNavigate] = useState(false);
    const [hiden, setHiden] = useState(false);

    const handleFollowCommunity = (event) =>
    {
        console.log("Kliknuo sam " + props.ID);
        fetchFollowCommunity();
        setHiden(true);
    }

    const fetchFollowCommunity = async () => {
        const userID = localStorage.getItem("ID");

        const response = await fetch("https://localhost:44368/FollowCommunity/" + userID + "/" + props.ID,
        {
            method: 'PUT',
            headers:{
                'Content-type' : 'text/plain; charset=UTF-8'
            }
        });
    }

    const fetchFollowedCommunity = async () => {
        const userID = localStorage.getItem("ID");
        const response = await fetch("https://localhost:44368/User/GetFollowCommunity/"+ userID,
        {
            method: 'GET',
            headers:{'Content-type' : 'application/json; charset=UTF-8'}
        });

        const data = await response.json();
        console.log(data);

        data.forEach(el => {
            console.log(el.name, props.Name);
            if(el.name == props.Name){
                setHiden(true);
            }
        });
    }

    useEffect(()=>{
        fetchFollowedCommunity();
    },[]);

    return (
        <MDBCard className={style.mainStyle} id={props.ID}>
            <MDBCardBody>{props.Name}</MDBCardBody>
            {!hiden &&
                <MDBBtn
                    color='secondary'
                    data-target='#navbarToggleExternalContent'
                    aria-controls='navbarToggleExternalContent'
                    aria-expanded='false'
                    aria-label='Toggle navigation'
                    onClick={handleFollowCommunity}
                >
                    Follow
                    {callNavigate && <Navigate to={path} />}
                </MDBBtn> 
            }
        </MDBCard>
    );
}