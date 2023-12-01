import React from 'react';
import {useState} from 'react';
import { Navigate } from "react-router-dom";
import style from "./HeaderCommunities.module.css";
import {
  MDBContainer,
  MDBNavbar,
  MDBBtn,
  MDBInputGroup  
} from 'mdb-react-ui-kit';

export default function HeaderCommunity(props) {
    const [path, setPath] = useState('');
    const [callNavigate, setCallNavigate] = useState(false);

    const handleLogOut = (event) => {

        localStorage.removeItem("Token");
        localStorage.removeItem("Username");
        localStorage.removeItem("ID");
        localStorage.removeItem("Role");

        setPath("/login");
        setCallNavigate(true);
    }

    const handleBackHome = (event) =>{
        setPath("/MixPage");
        setCallNavigate(true);
    }

    return (
        <MDBNavbar light bgColor='light'>
            <MDBContainer fluid>
                <p data-target='#start' className={style.title}>ForumApp</p> 
                <MDBBtn
                    color='secondary'
                    data-target='#navbarToggleExternalContent'
                    aria-controls='navbarToggleExternalContent'
                    aria-expanded='false'
                    aria-label='Toggle navigation'
                    onClick={handleBackHome}
                >
                    Home
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