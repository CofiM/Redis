import React, { useState } from 'react';
import style from "./SideBar.module.css"
import Community from '../Community/Community';
import { Navigate, NavLink } from "react-router-dom";

export default function SideBarItem(name) {
        const [callNavigate, setCallNavigate] = useState(false);
        const [path, setPath] = useState('');


        const handleOnClickTopic = (event) => {
                setCallNavigate(true);
                setPath("/Community");
                localStorage.setItem("CommunityID", name.ID);
                localStorage.setItem("CommunityName", name.name);
        }

        console.log(name);
        return (
                <a 
                        onClick={handleOnClickTopic}
                > 
                {name.name}
                {callNavigate && <Navigate to={path}/> }
                </a>
        );
  }