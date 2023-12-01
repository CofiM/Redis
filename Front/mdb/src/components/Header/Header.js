import React from 'react';
import {useState} from 'react';
import { Navigate } from "react-router-dom";
import style from "./Header.module.css";
import {
  MDBContainer,
  MDBNavbar,
  MDBBtn,
  MDBInputGroup  
} from 'mdb-react-ui-kit';

export default function App() {
    const [path, setPath] = useState('');
    const [callNavigate, setCallNavigate] = useState(false);
    const [topic, setTopic] = useState('');

    const onChangeTopicHandler = (event) => {
        setTopic(event.target.value);
        console.log(topic);
    }

    const onSearchHandler = (event) => {
        setPath("/Topic");
        setCallNavigate(true);
        localStorage.setItem("Topic", topic);
    }

    const handleLogOut = (event) => {

        localStorage.removeItem("Token");
        localStorage.removeItem("Username");
        localStorage.removeItem("ID");
        localStorage.removeItem("Role");

        setPath("/login");
        setCallNavigate(true);
    }
    
    const handleViewAllCommunities = (event) => {
        setPath("/Communities");
        setCallNavigate(true);
    }

    return (
        <MDBNavbar light bgColor='light'>
            <MDBContainer fluid>
                <p data-target='#start' className={style.title}>ForumApp</p> 
                <MDBInputGroup tag="form" className={style.widthText}>
                    <input 
                        className='form-control' 
                        placeholder="Type topic" 
                        aria-label="Search" 
                        type='Search' 
                        onChange={onChangeTopicHandler}
                    />
                    <MDBBtn 
                        color='secondary' 
                        className={style.buttonClass}
                        onClick={onSearchHandler}
                    >
                        Search
                        {callNavigate && <Navigate to={path} variant="body2"/>}
                    </MDBBtn>
                </MDBInputGroup>
                <MDBBtn
                    color='secondary'
                    data-target='#navbarToggleExternalContent'
                    aria-controls='navbarToggleExternalContent'
                    aria-expanded='false'
                    aria-label='Toggle navigation'
                    onClick={handleViewAllCommunities}
                >
                    Community
                    {callNavigate && <Navigate to={path} variant="body2"/>}
                </MDBBtn>
                <MDBBtn
                    color='secondary'
                    data-target='#navbarToggleExternalContent'
                    aria-controls='navbarToggleExternalContent'
                    aria-expanded='false'
                    aria-label='Toggle navigation'
                    onClick={handleLogOut}
                >
                    Log out
                    {callNavigate && <Navigate to={path} variant="body2"/>}
                </MDBBtn>
            </MDBContainer>
        </MDBNavbar>
    );
}