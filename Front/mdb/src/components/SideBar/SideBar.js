import React from 'react';
import style from "./SideBar.module.css";
import SideBarItem from "./SideBarItem.js";
import { useEffect,useState } from "react";

export default function SideBar(props) {
    /* const [communities,setCommunities] = useState([]);
    const count = 0;
    const userID = localStorage.getItem("ID");
    const fetchData = async () =>
    {
        const response = await fetch("https://localhost:44368/User/GetFollowCommunity/"+userID,
        {
            method: 'GET',
            headers:{'Content-type' : 'application/json; charset=UTF-8'}
        });

        console.log(response);
        const data = await response.json();
        console.log(data);
        setCommunities(data);
    };
    fetchData();
    
    console.log(communities);

    useEffect(()=>{
        fetchData();
    },[count]);

    
    console.log(communities); */
    console.log("DATA " + props.data);
    return (
        <div className={style.sidebar}>            
            <label className={style.LeftMargin}>Communities</label>
            { props.data.map((p)=>(
                <div>
                    <SideBarItem 
                        name={p.name}
                        ID = {p.id}
                    />
                </div>
            ))}
        </div>
    );
}